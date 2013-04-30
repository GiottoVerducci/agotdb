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
            public int SetId;
            public bool ByChapter;
            public string ShortName;
            public string[] ChaptersNames;

            public int GetChapterId(string chapterName)
            {
                for (int i = 0; i < ChaptersNames.Length; ++i)
                    if (string.Equals(ChaptersNames[i], chapterName, StringComparison.InvariantCultureIgnoreCase))
                        return i + 1;
                return 0;
            }
        }

        private static string Strip(string name)
        {
            char[] arr = name.ToCharArray();
            return new string(Array.FindAll<char>(arr, (c => (char.IsLetterOrDigit(c)))));
        }

        private static readonly List<string> _outputSummary = new List<string>();

        private static OctgnCard FindCard(DataRow row, Dictionary<int, SetInformation> setInformations, Dictionary<OctgnSetData, OctgnCard[]> octgnSets)
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
                            .Select(c => new Tuple<int, OctgnCard>(ComputeLevenshteinDistance(Strip(c.Name), originalName), c))
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
                    .Select(card => new Tuple<int, OctgnCard>(GetMatchingScore(row, card), card))
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

        public static int GetMatchingScore(DataRow row, OctgnCard card)
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

        public static void ExtractSetIdAndCardIdFromOctgnId(string octgnId, out int setId, out int cardId)
        {
            var setInformations = octgnId.Substring(31);
            setId = Convert.ToInt32(setInformations.Substring(0, 2));
            cardId = Convert.ToInt32(setInformations.Substring(2));
        }

        public struct ImportResult
        {
            public bool IsSuccessful;
            public OctgnSetData SetNotFound;
        }

        public static ImportResult UpdateCards(Dictionary<OctgnSetData, OctgnCard[]> octgnSets, BackgroundWorker backgroundWorker)
        {
            var setTable = ApplicationSettings.DatabaseManager.GetExpansionSets();
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
                    var octgnSetData = octgnSets.Keys.First(k => string.Equals(name, Strip(k.Name), StringComparison.InvariantCultureIgnoreCase));
                    si.OctgnName = octgnSetData.Name;
                    setInformations.Add(Convert.ToInt32(row["SetId"]), si);
                }
                ++progress;
            }
            backgroundWorker.ReportProgress(100, OctgnLoaderTask.MatchSet);

            object errorObject = null;

            ApplicationSettings.DatabaseManager.UpdateCards((row, cardProgress) =>
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

        private static int GetUniversalId(int setId, int cardId)
        {
            return setId * 10000 + cardId;
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
            NoSetsFounds,
            SetNotDefinedInDatabase
        }

        public class OctgnLoaderResultAndValue
        {
            public OctgnLoaderResult Result;
            public object Value;
        }

        public class OctgnSetData
        {
            public string Name { get; set; }
            public int Id { get; set; }
            public int MinCardId { get; set; }
            public int MaxCardId { get; set; }
            public int ChapterId { get; set; }
            public string Version { get; set; }
        }

        public static Dictionary<OctgnSetData, OctgnCard[]> LoadAllSets(string path, BackgroundWorker backgroundWorker)
        {
            var result = new Dictionary<OctgnSetData, OctgnCard[]>();

            var directoryInfo = new DirectoryInfo(path);
            var fileInfos = directoryInfo.GetFiles("*.o8s", SearchOption.TopDirectoryOnly);
            var patchFileInfos = directoryInfo.GetFiles("*.o8p", SearchOption.TopDirectoryOnly);

            var totalFileCount = fileInfos.Length + patchFileInfos.Length;

            var progress = 0;
            foreach (FileInfo fileInfo in fileInfos)
            {
                if (backgroundWorker.CancellationPending)
                    return null;
                backgroundWorker.ReportProgress(progress * 100 / totalFileCount, OctgnLoaderTask.LoadSet);
                var tempFolderPath = Path.GetTempPath() + Path.GetRandomFileName();
                Directory.CreateDirectory(tempFolderPath);
                var files = ZipHelper.UnZipFile(fileInfo.FullName, tempFolderPath, filePattern: "*.xml");
                var setFile = files.FirstOrDefault(f => 
                {
                    var name = Path.GetFileName(f);
                    return !name.StartsWith("[") && name.EndsWith(".xml"); 
                });
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

                var setData = new OctgnSetData();

                XmlNode setNode = doc.SelectNodes("//set")[0];
                setData.Name = setNode.Attributes["name"].Value;
                setData.Version = setNode.Attributes["version"].Value;

                var cardNodes = setNode.SelectNodes("cards/card");

                var setCards = (from XmlNode cardNode in cardNodes select LoadOctgnCard(cardNode)).OrderBy(c => c.Name).ToArray();

                if (setCards.Length > 0)
                {
                    setData.MinCardId = int.MaxValue;
                    setData.MaxCardId = int.MinValue;

                    foreach (var card in setCards)
                    {
                        int setId, cardId;
                        ExtractSetIdAndCardIdFromOctgnId(card.Id, out setId, out cardId);
                        setData.Id = setId;
                        if (cardId > setData.MaxCardId)
                            setData.MaxCardId = cardId;
                        if (cardId < setData.MinCardId)
                            setData.MinCardId = cardId;
                    }

                    result.Add(setData, setCards);
                }

                var tempDirectoryInfo = new DirectoryInfo(tempFolderPath);
                tempDirectoryInfo.Delete(true);
                ++progress;
            }
            // load patchs
            foreach (FileInfo fileInfo in patchFileInfos)
            {
                if (backgroundWorker.CancellationPending)
                    return null;
                backgroundWorker.ReportProgress(progress * 100 / totalFileCount, OctgnLoaderTask.LoadSet);
                var tempFolderPath = Path.GetTempPath() + Path.GetRandomFileName();
                Directory.CreateDirectory(tempFolderPath);
                var files = ZipHelper.UnZipFile(fileInfo.FullName, tempFolderPath, filePattern: "*.xml");
                var patchSetFiles = files.Where(f => 
                {
                    var pathItems = f.Split('\\', '/');
                    return pathItems.Length > 1 && pathItems[pathItems.Length-2] == "xmls"
                        && !pathItems[pathItems.Length - 1].StartsWith("[") && pathItems[pathItems.Length - 1].EndsWith(".xml"); 
                }).ToArray();
                if (patchSetFiles.Length == 0)
                    continue;

                foreach (var patchSetFile in patchSetFiles)
                {
                    var doc = new XmlDocument();
                    try
                    {
                        doc.Load(patchSetFile);
                    }
                    catch (Exception ex)
                    {
                        throw new Exception("Couldn't read patch set definition file " + patchSetFile, ex);
                    }

                    XmlNode setNode = doc.SelectNodes("//set")[0];
                    var name = setNode.Attributes["name"].Value;

                    OctgnSetData setData = result.Keys.FirstOrDefault(k => k.Name == name);
                    if (setData == null) // set to patch not found
                        continue;

                    var version = setNode.Attributes["version"].Value;
                    if (setData.Version != version) // the patch applies to another version
                        continue;

                    var setCards = result[setData];

                    var cardNodes = setNode.SelectNodes("cards/card");

                    var patchSetCards = (from XmlNode cardNode in cardNodes select LoadOctgnCard(cardNode)).OrderBy(c => c.Name).ToArray();

                    // replace the cards by their patched value
                    foreach (var card in patchSetCards)
                    {
                        int i = 0;
                        while (i < setCards.Length && setCards[i].Id != card.Id)
                            ++i;
                        if (i < setCards.Length)
                            setCards[i] = card;
                    }

                    // recompute the data
                    if (setCards.Length > 0)
                    {
                        setData.MinCardId = int.MaxValue;
                        setData.MaxCardId = int.MinValue;

                        foreach (var card in setCards)
                        {
                            int setId, cardId;
                            ExtractSetIdAndCardIdFromOctgnId(card.Id, out setId, out cardId);
                            setData.Id = setId;
                            if (cardId > setData.MaxCardId)
                                setData.MaxCardId = cardId;
                            if (cardId < setData.MinCardId)
                                setData.MinCardId = cardId;
                        }
                    }
                }

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
