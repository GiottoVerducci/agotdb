using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Xml;
using GenericDB.DataAccess;
using GenericDB.OCTGN;
using NRADB.BusinessObjects;
using GenericDB.Helper;

namespace NRADB.OCTGN
{
    public static class OctgnLoader
    {
        private static readonly List<Tuple<PropertyInfo, OctgnCardDataAttribute>> _metadata = new List<Tuple<PropertyInfo, OctgnCardDataAttribute>>();

        static OctgnLoader()
        {
            PropertyInfo[] propertyInfos = typeof(OctgnCard).GetProperties();
            foreach (var propertyInfo in propertyInfos)
            {
                var excelDataAttribute = GetAttribute<OctgnCardDataAttribute>(propertyInfo);
                if (excelDataAttribute == null)
                    continue;
                _metadata.Add(new Tuple<PropertyInfo, OctgnCardDataAttribute>(propertyInfo, excelDataAttribute));
            }
        }

        public static TAttribute GetAttribute<TAttribute>(PropertyInfo propertyInfo)
        {
            if (propertyInfo == null)
                return default(TAttribute);

            var attributes = propertyInfo.GetCustomAttributes(typeof(TAttribute), true);
            return (attributes.Length > 0) ? attributes.OfType<TAttribute>().FirstOrDefault() : default(TAttribute);
        }

        private class SetInformation
        {
            public string OriginalName;
            public string OctgnName;
        }

        private static string Strip(string name)
        {
            char[] arr = name.ToCharArray();
            return new string(Array.FindAll<char>(arr, (c => (char.IsLetterOrDigit(c)))));
        }

        private static readonly List<string> _outputSummary = new List<string>();

        //private static OctgnCard FindCard(DataRow row, Dictionary<int, SetInformation> setInformations, Dictionary<string, List<OctgnCard>> octgnSets)
        //{
        //    var universalId = (int)row["UniversalId"];
        //    var setId = universalId / 10000;
        //    SetInformation setInformation;
        //    if (setInformations.TryGetValue(setId, out setInformation))
        //    {
        //        var originalName = Strip(row["OriginalName"].ToString());
        //        List<OctgnCard> octgnSet;
        //        if (setInformation.OctgnName != null && octgnSets.TryGetValue(setInformation.OctgnName, out octgnSet))
        //        {
        //            var cards = octgnSet.Where(c => string.Equals(Strip(c.Name), originalName, StringComparison.InvariantCultureIgnoreCase)).ToList();
        //            if (cards.Count == 1)
        //                return cards[0];
        //            if (cards.Count > 1)
        //                return cards.OrderByDescending(card => GetMatchingScore(row, card)).First();
        //            // try searching approximative names in the set
        //            var distanceCards = octgnSet
        //                .Select(c => new Tuple<int, OctgnCard>(ComputeLevenshteinDistance(Strip(c.Name), originalName), c))
        //                .Where(distanceCard => distanceCard.Item1 <= 2)
        //                .OrderBy(distanceCard => distanceCard.Item1)
        //                .ToList();
        //            if (distanceCards.Count > 0)
        //            {
        //                // for debugging purpose: keep track of the mismatch values
        //                var output = string.Format("{0}\t{1} -> {2}", universalId, row["OriginalName"], distanceCards.First().Item2.Name);
        //                _outputSummary.Add(output);
        //                return distanceCards.First().Item2;
        //            }

        //        }
        //        var scoreCards = octgnSets.Values
        //            .SelectMany(c => c)
        //            .Where(c => string.Equals(Strip(c.Name), originalName, StringComparison.InvariantCultureIgnoreCase))
        //            .Select(card => new Tuple<int, OctgnCard>(GetMatchingScore(row, card), card))
        //            .Where(card => card.Item1 >= 0)
        //            .OrderByDescending(card => card.Item1)
        //            .ToList();
        //        if (scoreCards.Count > 0)
        //            return scoreCards.First().Item2;
        //        return null;
        //    }
        //    return null;
        //}

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

        //public static int GetMatchingScore(DataRow row, OctgnCard card)
        //{
        //    var result = 0;
        //    if (card.House != null)
        //    {
        //        var oHouses = card.House.Split('.', ' ', '/').Select(Strip).Where(h => !string.IsNullOrEmpty(h)).ToList();
        //        result += oHouses.Where(house => (bool)row["House" + house]).Sum(house => 10);
        //    }
        //    if (card.Type != null)
        //    {
        //        var aType = (AgotCard.CardType)Convert.ToInt32(row["Type"]);
        //        if (!string.Equals(card.Type, aType.ToString(), StringComparison.InvariantCultureIgnoreCase))
        //            result -= 1000;
        //    }

        //    Func<string, string, bool> boolEquals = (oValue, columnName) => !string.IsNullOrWhiteSpace(oValue) && (oValue == "1") == ((bool)row[columnName]);

        //    if (boolEquals(card.Unique, "Unique"))
        //        result += 10;

        //    if (!string.IsNullOrWhiteSpace(card.Cost))
        //    {
        //        var oCost = OctgnInt(card.Cost);
        //        var aCost = DbInt(row["Cost"]);
        //        //result += 5 - Math.Abs(oCost - aCost);
        //        if (oCost == aCost)
        //            result += 5;
        //    }
        //    if (!string.IsNullOrWhiteSpace(card.Strength))
        //    {
        //        var oStrength = OctgnInt(card.Strength);
        //        var aStrength = DbInt(row["Strength"]);
        //        //result += 5 - Math.Abs(oStrength - aStrength);
        //        if (oStrength == aStrength)
        //            result += 5;
        //    }

        //    if (boolEquals(card.Military, "Military"))
        //        result++;
        //    if (boolEquals(card.Intrigue, "Intrigue"))
        //        result++;
        //    if (boolEquals(card.Power, "Power"))
        //        result++;

        //    return result;
        //}

        public static int OctgnInt(string value)
        {
            if (value.StartsWith("s", StringComparison.InvariantCultureIgnoreCase))
                value = value.Substring(1);
            if (string.Equals(value, "X", StringComparison.InvariantCultureIgnoreCase))
                return -1;
            return Convert.ToInt32(value);
        }

        public static int DbInt(object obj)
        {
            if (obj == null || obj == DBNull.Value)
                return 0;
            return Convert.ToInt32(obj);
        }

        //public static T ConvertFromDbVal<T>(object obj)
        //{
        //    if (obj == null || obj == DBNull.Value)
        //        return default(T); // returns the default value for the type
        //    return (T)obj;
        //}

        public static void ImportCards(Dictionary<string, List<OctgnCard>> octgnSets, BackgroundWorker backgroundWorker)
        {
            var allCards = octgnSets.Values.SelectMany(c => c).ToList();
            var cursor = (object)0;

            ApplicationSettings.DatabaseManager.ResetAndImportCards(destinationRows =>
                {
                    if (backgroundWorker.CancellationPending)
                        return DatabaseManager.OperationResult.Abort;

                    var index = (int)cursor;
                    if (index >= allCards.Count)
                    {
                        backgroundWorker.ReportProgress(100, OctgnLoaderTask.FindCard);
                        backgroundWorker.ReportProgress(66, OctgnLoaderTask.UpdateDatabase); // arbitrary, we don't get progress notification when the dataset is updated
                        return DatabaseManager.OperationResult.Done;
                    }

                    var card = allCards[index];
                    cursor = index + 1;

                    var keywords = card.Keywords.Split('-').ToList();
                    bool isUnique = keywords.Remove("Unique");

                    var setInformations = card.Id.ToString().Substring(31);
                    var setId = Convert.ToInt32(setInformations.Substring(0, 2));
                    var cardId = Convert.ToInt32(setInformations.Substring(2));

                    destinationRows.Add(
                        index,
                        card.Name,
                        card.Subtitle,
                        card.Side,
                        card.Type,
                        card.Faction,
                        isUnique ? "Yes" : "No",
                        string.Join(". ", keywords),
                        card.Text,
                        card.Instructions,
                        card.Cost,
                        card.Stat == "-" ? string.Empty : card.Stat,
                        card.Influence == "-" ? string.Empty : card.Influence,
                        card.Requirement,
                        string.Format("CS({0})", cardId),
                        card.Name,
                        (setId * 10000 + cardId).ToString(),
                        "",//banned
                        "",//restricted
                        card.Id,
                        card.Flavor
                    );

                    backgroundWorker.ReportProgress((index * 100) / allCards.Count, OctgnLoaderTask.FindCard);
                    return DatabaseManager.OperationResult.Ok;
                });
            if (backgroundWorker.CancellationPending)
                return;
            backgroundWorker.ReportProgress(100, OctgnLoaderTask.UpdateDatabase);
        }

        public enum OctgnLoaderTask
        {
            Undefined,
            LoadSet,
            MatchSet,
            FindCard,
            UpdateDatabase
        }

        public enum OctgnLoaderResult
        {
            Undefined,
            Success,
            SetsNotFound
        }

        public static Dictionary<string, List<OctgnCard>> LoadAllSets(string path, BackgroundWorker backgroundWorker)
        {
            var result = new Dictionary<string, List<OctgnCard>>(); // key: set name

            var directoryInfo = new DirectoryInfo(path);
            var fileInfos = directoryInfo.GetFiles("*.o8s", SearchOption.TopDirectoryOnly);

            var progress = 0;
            foreach (FileInfo fileInfo in fileInfos)
            {
                if (backgroundWorker.CancellationPending)
                    return null;
                backgroundWorker.ReportProgress(progress * 100 / fileInfos.Length, OctgnLoaderTask.LoadSet);
                var tempFolderPath = Path.GetTempPath() + Path.GetRandomFileName();
                Directory.CreateDirectory(tempFolderPath);
                var files = ZipHelper.UnZipFile(fileInfo.FullName, tempFolderPath, filePattern: "*.xml");
                var setFile = files.FirstOrDefault(f => !f.StartsWith("[") && f.EndsWith(".xml"));
                if (setFile == null)
                    throw new Exception("Couldn't find set definition file in set file " + fileInfo);

                var doc = new XmlDocument();
                try
                {
                    doc.Load(setFile);
                }
                catch (Exception ex)
                {
                    throw new Exception("Couldn't read set definition file " + setFile, ex);
                }

                XmlNode setNode = doc.SelectNodes("//set")[0];
                var setName = setNode.Attributes["name"].Value;

                var cardNodes = setNode.SelectNodes("cards/card");

                var setCards = (from XmlNode cardNode in cardNodes select LoadOctgnCard(cardNode)).OrderBy(c => c.Name).ToList();

                if (setCards.Count > 0)
                    result.Add(setName, setCards);

                var tempDirectoryInfo = new DirectoryInfo(tempFolderPath);
                tempDirectoryInfo.Delete(true);
                ++progress;
            }
            backgroundWorker.ReportProgress(100, OctgnLoaderTask.LoadSet);
            return result;
        }

        private static OctgnCard LoadOctgnCard(XmlNode cardNode)
        {
            var card = new OctgnCard();
            foreach (XmlAttribute attribute in cardNode.Attributes)
            {
                var setter = GetSetter(attribute.Name);
                if (setter != null)
                    setter(card, attribute.Value);
            }

            var propertyNodes = cardNode.SelectNodes("property");
            foreach (XmlNode propertyNode in propertyNodes)
            {
                var propertyName = propertyNode.Attributes["name"].Value;
                var propertyValue = propertyNode.Attributes["value"].Value;

                var setter = GetSetter(propertyName);
                if (setter != null && propertyValue != null)
                    setter(card, propertyValue);
            }

            return card;
        }

        public delegate void Setter(OctgnCard card, string value);

        private static Action<OctgnCard, string> GetSetter(string propertyName)
        {
            var metadata = _metadata.FirstOrDefault(p => p.Item2.AttributeName == propertyName || p.Item2.PropertyName == propertyName);
            if (metadata == null)
                return null;

            PropertyInfo propertyInfo = metadata.Item1;
            var setter = propertyInfo.GetSetMethod();
            var setterDelegate = Delegate.CreateDelegate(typeof(Setter), setter);
            return (item, s) => ((Setter)setterDelegate)(item, s);
        }
    }
}
