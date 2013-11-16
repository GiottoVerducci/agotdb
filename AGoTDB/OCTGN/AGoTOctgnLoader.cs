using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Xml;

using AGoTDB.BusinessObjects;
using GenericDB.DataAccess;
using GenericDB.Helper;
using GenericDB.OCTGN;

namespace AGoTDB.OCTGN
{
    public class AgotOctgnLoader: OctgnLoader<AgotOctgnCard>
    {
        private static string Strip(string name)
        {
            char[] arr = name.ToCharArray();
            return new string(Array.FindAll<char>(arr, (c => (char.IsLetterOrDigit(c)))));
        }

        private static readonly List<string> _outputSummary = new List<string>();

        private static AgotOctgnCard FindCard(DataRow row, Dictionary<int, SetInformation> setInformations, Dictionary<OctgnSetData, AgotOctgnCard[]> octgnSets)
        {
            var universalId = (int)row["UniversalId"];
            var setId = universalId / 10000;
            SetInformation setInformation;
            if (setInformations.TryGetValue(setId, out setInformation))
            {
                var originalName = Strip(row["OriginalName"].ToString());
                if (setInformation.OctgnName != null)
                {
                    var octgnSetData = octgnSets.FirstOrDefault(kvp => kvp.Key.Name == setInformation.OctgnName);
                    if (octgnSetData.Value != null)
                    {
                        var octgnSet = octgnSetData.Value;
                        var cards = octgnSet.Where(c => string.Equals(Strip(c.Name), originalName, StringComparison.InvariantCultureIgnoreCase)).ToList();
                        if (cards.Count == 1)
                            return cards[0];
                        if (cards.Count > 1)
                            return cards.OrderByDescending(card => GetMatchingScore(row, card)).First();
                        // try searching approximative names in the set
                        var distanceCards = octgnSet
                            .Select(c => new Tuple<int, AgotOctgnCard>(ComputeLevenshteinDistance(Strip(c.Name), originalName), c))
                            .Where(distanceCard => distanceCard.Item1 <= 2)
                            .OrderBy(distanceCard => distanceCard.Item1)
                            .ToList();
                        if (distanceCards.Count > 0)
                        {
                            // for debugging purpose: keep track of the mismatch values
                            var output = string.Format("{0}\t{1} -> {2}", universalId, row["OriginalName"], distanceCards.First().Item2.Name);
                            _outputSummary.Add(output);
                            return distanceCards.First().Item2;
                        }
                    }
                }
                var scoreCards = octgnSets.Values
                    .SelectMany(c => c)
                    .Where(c => string.Equals(Strip(c.Name), originalName, StringComparison.InvariantCultureIgnoreCase))
                    .Select(card => new Tuple<int, AgotOctgnCard>(GetMatchingScore(row, card), card))
                    .Where(card => card.Item1 >= 0)
                    .OrderByDescending(card => card.Item1)
                    .ToList();
                if (scoreCards.Count > 0)
                    return scoreCards.First().Item2;
                return null;
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

        public static int GetMatchingScore(DataRow row, AgotOctgnCard card)
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
            foreach (DataRow row in setTable.Rows)
            {
                if (backgroundWorker.CancellationPending)
                    return new ImportResult { IsSuccessful = false };
                backgroundWorker.ReportProgress(progress * 100 / setTable.Rows.Count, OctgnLoaderTask.MatchSet);
                if ((int)row["Id"] >= 0)
                {
                    var si = new SetInformation { OriginalName = row["OriginalName"].ToString() };
                    var name = Strip(si.OriginalName);
                    var octgnSetData = octgnSets.Keys.FirstOrDefault(k => Strip(k.Name).EndsWith(name, StringComparison.InvariantCultureIgnoreCase));
                    if (octgnSetData == null)
                        continue;
                    si.OctgnName = octgnSetData.Name;
                    setInformations.Add(Convert.ToInt32(row["SetId"]), si);
                }
                ++progress;
            }
            backgroundWorker.ReportProgress(100, OctgnLoaderTask.MatchSet);

            object errorObject = null;

            ApplicationSettings.Instance.DatabaseManager.UpdateCards((row, cardProgress) =>
                {
                    if (backgroundWorker.CancellationPending)
                        return DatabaseManager.OperationResult.Abort;
                    var card = FindCard(row, setInformations, octgnSets);
                    if (card != null)
                        row["OctgnId"] = card.Id;
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
