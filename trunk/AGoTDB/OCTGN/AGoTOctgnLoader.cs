using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using AGoTDB.BusinessObjects;
using GenericDB.DataAccess;
using GenericDB.OCTGN;

namespace AGoTDB.OCTGN
{
    public class AgotOctgnLoader : OctgnLoader<AgotOctgnCard>
    {
        private static string Strip(string name)
        {
            char[] arr = name.ToCharArray();
            return new string(Array.FindAll<char>(arr, (c => (char.IsLetterOrDigit(c)))));
        }

        private static readonly List<string> _outputSummary = new List<string>();

        /// <summary>Find in the octgn data the cards matching a row.</summary>
        /// <param name="row">The row from the database.</param>
        /// <param name="setInformations">The octgn data set informations.</param>
        /// <param name="octgnSets">The octgn data sets.</param>
        /// <returns>The cards matching the row (the card may appear more than once in the octgn data).</returns>
        private static ICollection<AgotOctgnCard> FindCards(IDataRow row, Dictionary<int, SetInformation> setInformations, Dictionary<OctgnSetData, AgotOctgnCard[]> octgnSets)
        {
            var universalId = (int)row["UniversalId"];
            //var setId = universalId / 10000;

            var sets = row["Set"].ToString()
                .Split('/')
                .Select(w =>
                    {
                        var index = w.IndexOf('-');
                        if (index == -1)
                            index = w.IndexOf('(');
                        return w.Substring(0, index);
                    })
                .Select(w => w.Trim())
                .ToArray();

            var setsIds = setInformations
                .Where(kvp => sets.Contains(kvp.Value.ShortName))
                .Select(kvp => kvp.Key)
                .OrderByDescending(setId => setId)
                .ToArray();

            var result = new List<Tuple<bool, AgotOctgnCard>>();

            foreach (var setId in setsIds)
            {
                SetInformation setInformation;
                if (setInformations.TryGetValue(setId, out setInformation))
                {
                    string name = row["OriginalName"].ToString();
                TryAgain: // temporary, until OCTGN data are correct
                    var originalName = Strip(name);
                    if (setInformation.OctgnName != null)
                    {
                        var octgnSetData = octgnSets.FirstOrDefault(kvp => kvp.Key.Name == setInformation.OctgnName);
                        if (octgnSetData.Value != null)
                        {
                            var octgnSet = octgnSetData.Value;
                            var cards = octgnSet.Where(c => string.Equals(Strip(c.Name), originalName, StringComparison.InvariantCultureIgnoreCase)).ToList();
                            if (cards.Count == 1)
                            {
                                result.Add(new Tuple<bool, AgotOctgnCard>(true, cards[0])); // perfect match
                                continue;
                            }
                            if (cards.Count > 1)
                            {
                                var matchingCards = cards.GroupBy(card => GetMatchingScore(row, card)).OrderByDescending(group => group.Key);
                                var bestMatchingCards = matchingCards.First().ToArray();
                                // there are now two possibilities:
                                // 1) either the same card appear twice (eg. Carrion Birds)
                                // 2) either two different cards share the same name (eg. The Kingsroad)
                                if (bestMatchingCards.Length == 1 || bestMatchingCards.Skip(1).All(c => c.Equals(bestMatchingCards[0])))
                                {
                                    // case 1)
                                    result.AddRange(bestMatchingCards.Select(card => new Tuple<bool, AgotOctgnCard>(true, card)));
                                }
                                else
                                {
                                    // case 2)
                                    var universalIdOrder = universalId % 1000; // keep the last 3 digits
                                    var bestMatch = bestMatchingCards.FirstOrDefault(c => 
                                        {
                                            int order;
                                            return int.TryParse(c.Id.Substring(c.Id.Length - 3), out order) && order == universalIdOrder;
                                        });
                                    if (bestMatch != null)
                                    {
                                        result.Add(new Tuple<bool, AgotOctgnCard>(true, bestMatch));
                                    }
                                }
                                continue;
                            }
                            // try searching approximative names in the set
                            var distanceCards = octgnSet
                                .GroupBy(c => ComputeLevenshteinDistance(Strip(c.Name), originalName))
                                .Where(group => group.Key <= 6)
                                .OrderBy(group => group.Key)
                                .ToArray();
                            if (distanceCards.Length > 0)
                            {
                                // for debugging purpose: keep track of the mismatch values
                                foreach (var card in distanceCards[0])
                                {
                                    var output = string.Format("{0}\t{1} -> {2}", universalId, row["OriginalName"], card.Name);
                                    _outputSummary.Add(output);
                                }
                                result.Add(new Tuple<bool, AgotOctgnCard>(false, distanceCards[0].First()));
                            }
                            else // try again with variation because the OCTGN card set contains shit ("When Intrigue Woke" instead of "When I Woke", really!?)
                            {
                                if (name.Contains(" I "))
                                {
                                    name = Strip(name.Replace(" I ", " Intrigue "));
                                    goto TryAgain;
                                }
                            }
                        }
                    }
                    //var scoreCards = octgnSets.Values
                    //    .SelectMany(c => c)
                    //    .Where(c => string.Equals(Strip(c.Name), originalName, StringComparison.InvariantCultureIgnoreCase))
                    //    .Select(card => new Tuple<int, AgotOctgnCard>(GetMatchingScore(row, card), card))
                    //    .Where(card => card.Item1 >= 0)
                    //    .OrderByDescending(card => card.Item1)
                    //    .ToList();
                    //if (scoreCards.Count > 0)
                    //    return new[] { scoreCards.First().Item2 };
                    //return null;
                }
            }

            if (result.Any(c => c.Item1)) // at least 1 perfect match
            {
                return result.Where(c => c.Item1).Select(c => c.Item2).ToArray();
            }
            if (result.Count >= 1)
            {
                return result.Select(c => c.Item2).ToArray();
            }
            return null;
        }

        /// <summary>
        /// Compute the distance between two strings.
        /// </summary>
        public static int ComputeLevenshteinDistance(string s, string t)
        {
            int n = s.Length;
            int m = t.Length;
            int[,] d = new int[n + 1, m + 1];

            // Step 1
            if (n == 0)
                return m;

            if (m == 0)
                return n;

            // Step 2
            for (int i = 0; i <= n; d[i, 0] = i++) ;
            for (int j = 0; j <= m; d[0, j] = j++) ;

            // Step 3
            for (int i = 1; i <= n; i++)
            {
                //Step 4
                for (int j = 1; j <= m; j++)
                {
                    // Step 5
                    int cost = (t[j - 1] == s[i - 1]) ? 0 : 1;

                    // Step 6
                    d[i, j] = Math.Min(
                        Math.Min(d[i - 1, j] + 1, d[i, j - 1] + 1),
                        d[i - 1, j - 1] + cost);
                }
            }
            // Step 7
            return d[n, m];
        }

        public static int GetMatchingScore(IDataRow row, AgotOctgnCard card)
        {
            var result = 0;
            if (card.House != null)
            {
                var oHouses = card.House.Split('.', ' ', '/').Select(Strip).Where(h => !string.IsNullOrEmpty(h)).ToList();
                result += oHouses.Where(house => (bool)row["House" + house]).Sum(house => 10);
            }
            if (card.Type != null)
            {
                var aType = (AgotCard.CardType)Convert.ToInt32(row["Type"]);
                if (!string.Equals(card.Type, aType.ToString(), StringComparison.InvariantCultureIgnoreCase))
                    result -= 1000;
            }

            Func<string, string, bool> boolEquals = (oValue, columnName) => !string.IsNullOrWhiteSpace(oValue) && (oValue == "1") == ((bool)row[columnName]);

            if (boolEquals(card.Unique, "Unique"))
                result += 10;

            if (!string.IsNullOrWhiteSpace(card.Cost))
            {
                var oCost = OctgnInt(card.Cost);
                var aCost = DbInt(row["Cost"]);
                //result += 5 - Math.Abs(oCost - aCost);
                if (oCost == aCost)
                    result += 5;
            }
            if (!string.IsNullOrWhiteSpace(card.Strength))
            {
                var oStrength = OctgnInt(card.Strength);
                var aStrength = DbInt(row["Strength"]);
                //result += 5 - Math.Abs(oStrength - aStrength);
                if (oStrength == aStrength)
                    result += 5;
            }

            if (boolEquals(card.Military, "Military"))
                result++;
            if (boolEquals(card.Intrigue, "Intrigue"))
                result++;
            if (boolEquals(card.Power, "Power"))
                result++;

            return result;
        }

        public static ImportResult UpdateCards(Dictionary<OctgnSetData, AgotOctgnCard[]> octgnSets, BackgroundWorker backgroundWorker)
        {
            var setTable = ApplicationSettings.Instance.DatabaseManager.GetExpansionSets();
            var setInformations = new Dictionary<int, SetInformation>();

            var progress = 0;
            var remainingOctgnSets = new Dictionary<OctgnSetData, AgotOctgnCard[]>(octgnSets);
            foreach (DataRow row in setTable.Rows)
            {
                if (backgroundWorker.CancellationPending)
                    return new ImportResult { IsSuccessful = false };
                backgroundWorker.ReportProgress(progress * 100 / setTable.Rows.Count, OctgnLoaderTask.MatchSet);
                if ((int)row["Id"] >= 0)
                {
                    var si = new SetInformation { OriginalName = row["OriginalName"].ToString(), ShortName = row["ShortName"].ToString() };
                    var name = Strip(si.OriginalName);
                    var octgnSetData = remainingOctgnSets.Keys.FirstOrDefault(k => Strip(k.Name).EndsWith(name, StringComparison.InvariantCultureIgnoreCase));
                    if (octgnSetData != null)
                    {
                        si.OctgnName = octgnSetData.Name;
                        remainingOctgnSets.Remove(octgnSetData);
                    }
                    setInformations.Add(Convert.ToInt32(row["SetId"]), si);
                }
                ++progress;
            }

            // try to match the remaining sets
            foreach (var kvp in setInformations.Where(kvp => kvp.Value.OctgnName == null).ToArray())
            {
                var si = kvp.Value;
                var name = Strip(si.OriginalName);
                var octgnSetData = remainingOctgnSets.Keys.FirstOrDefault(k => 
                    {
                        var stripped = Strip(k.Name);
                        stripped = new string(stripped.SkipWhile(c => !char.IsLetter(c)).ToArray());
                        return ComputeLevenshteinDistance(stripped, name) <= 4;
                    });
                if (octgnSetData != null)
                {
                    si.OctgnName = octgnSetData.Name;
                    remainingOctgnSets.Remove(octgnSetData);
                }
                else
                {
                    setInformations.Remove(kvp.Key);
                }
            }

            backgroundWorker.ReportProgress(100, OctgnLoaderTask.MatchSet);

            object errorObject = null;

            ApplicationSettings.Instance.DatabaseManager.UpdateCards((row, cardProgress) =>
                {
                    if (backgroundWorker.CancellationPending)
                        return DatabaseManager.OperationResult.Abort;
                    var cards = FindCards(row, setInformations, octgnSets);
                    if (cards != null)
                        row["OctgnId"] = string.Join(",", cards.Select(card => card.Id));
                    if (cardProgress < 100)
                        backgroundWorker.ReportProgress(cardProgress, OctgnLoaderTask.FindCard);
                    else
                    {
                        backgroundWorker.ReportProgress(100, OctgnLoaderTask.FindCard);
                        backgroundWorker.ReportProgress(66, OctgnLoaderTask.UpdateDatabase); // arbitrary, we don't get progress notification when the dataset is updated
                    }
                    return DatabaseManager.OperationResult.Ok;
                });
            if (backgroundWorker.CancellationPending)
                return new ImportResult { IsSuccessful = false };

            if (errorObject is OctgnSetData)
                return new ImportResult { IsSuccessful = false, SetNotFound = errorObject as OctgnSetData };

            backgroundWorker.ReportProgress(100, OctgnLoaderTask.UpdateDatabase);
            return new ImportResult { IsSuccessful = true };
        }

        public override void ExtractSetIdAndCardIdFromOctgnId(string octgnId, out int setId, out int cardId)
        {
            //var setInformations = octgnId.Substring(32);
            setId = octgnId.Substring(0, 33).GetHashCode();
            cardId = Convert.ToInt32(octgnId.Substring(33));
        }

    }
}
