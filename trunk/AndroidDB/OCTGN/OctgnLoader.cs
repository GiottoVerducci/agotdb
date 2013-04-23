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

        //public static void ImportSets(Dictionary<OctgnSetData, OctgnCard[]> sets, BackgroundWorker backgroundWorker)
        //{
        //    var setDataById = sets.Keys.GroupBy(sd => sd.Id).OrderBy(g => g.Key).ToArray();
        //    var cursor = (object)0;
        //    ApplicationSettings.DatabaseManager.ResetAndImportSets(destinationRows =>
        //        {
        //            if (backgroundWorker.CancellationPending)
        //                return DatabaseManager.OperationResult.Abort;
        //            var index = (int)cursor;
        //            if (index >= setDataById.Length)
        //            {
        //                backgroundWorker.ReportProgress(100, OctgnLoaderTask.ImportSet);
        //                backgroundWorker.ReportProgress(66, OctgnLoaderTask.UpdateDatabase); // arbitrary, we don't get progress notification when the dataset is updated
        //                return DatabaseManager.OperationResult.Done;
        //            }
        //            var setDataGroup = setDataById[index];
        //            cursor = index + 1;
        //            var chapters = setDataGroup.OrderBy(sd => sd.MinCardId).ToArray();
        //            var chapterId = 0;
        //            foreach (var chapter in chapters)
        //                chapter.ChapterId = ++chapterId;

        //            var chapterNames = string.Join(", ", chapters.Select(sd => sd.Name));
        //            destinationRows.Add(
        //                index,
        //                chapters.Length > 1 ? "Set " + (index + 1) : chapterNames,
        //                chapters.Length > 1 ? "Set " + (index + 1) : chapterNames,
        //                setDataGroup.Key,
        //                chapters.Length > 1,
        //                true,
        //                chapters.Length > 1 ? chapterNames : null,
        //                chapters.Length > 1 ? "Set " + (index + 1) : chapterNames);

        //            backgroundWorker.ReportProgress((index * 100) / setDataById.Length, OctgnLoaderTask.ImportSet);
        //            return DatabaseManager.OperationResult.Ok;
        //        });
        //}

        public static void ImportCards(Dictionary<OctgnSetData, OctgnCard[]> octgnSets, BackgroundWorker backgroundWorker)
        {
            var setTable = ApplicationSettings.DatabaseManager.GetExpansionSets();
            var setInformations = new Dictionary<int, SetInformation>();

            var progress = 0;
            foreach (DataRow row in setTable.Rows)
            {
                if (backgroundWorker.CancellationPending)
                    return;
                backgroundWorker.ReportProgress(progress * 100 / setTable.Rows.Count, OctgnLoaderTask.ImportSet);
                var setId = (int)row["Id"];
                if (setId >= 0)
                {
                    var si = new SetInformation
                    {
                        OriginalName = row["OriginalName"].ToString(),
                        ShortName = row["ShortName"].ToString(),
                        ByChapter = (bool)row["ByChapter"],
                        SetId = (byte) row["SetId"],
                        ChaptersNames = row["ChaptersNames"].ToString().Split(',').Select(s => s.Trim()).ToArray()
                    };
                    //var name = Strip(si.OriginalName);
                    //var octgnName = octgnSets.Keys.FirstOrDefault(k => string.Equals(name, Strip(k.Name), StringComparison.InvariantCultureIgnoreCase));
                    //si.OctgnName = octgnName.Name;
                    setInformations.Add(Convert.ToInt32(row["SetId"]), si);
                }
                ++progress;
            }
            backgroundWorker.ReportProgress(100, OctgnLoaderTask.ImportSet); // match set

            var allCards = new List<KeyValuePair<OctgnSetData, OctgnCard>>();
            foreach (var kvp in octgnSets)
                allCards.AddRange(kvp.Value.Select((card => new KeyValuePair<OctgnSetData, OctgnCard>(kvp.Key, card))));
            allCards = allCards.OrderBy(kvp => 
            {
                int setId, cardId;
                ExtractSetIdAndCardIdFromOctgnId(kvp.Value.Id, out setId, out cardId);
                return GetUniversalId(setId, cardId);
            }).ToList();

            var cursor = (object)0;

            ApplicationSettings.DatabaseManager.ResetAndImportCards(destinationRows =>
                {
                    if (backgroundWorker.CancellationPending)
                        return DatabaseManager.OperationResult.Abort;

                    var index = (int)cursor;
                    if (index >= allCards.Count)
                    {
                        backgroundWorker.ReportProgress(100, OctgnLoaderTask.ImportCard);
                        backgroundWorker.ReportProgress(66, OctgnLoaderTask.UpdateDatabase); // arbitrary, we don't get progress notification when the dataset is updated
                        return DatabaseManager.OperationResult.Done;
                    }

                    var card = allCards[index].Value;
                    cursor = index + 1;

                    var keywords = card.Keywords.Split('-').ToList();
                    bool isUnique = keywords.Remove("Unique");

                    int setId, cardId;
                    ExtractSetIdAndCardIdFromOctgnId(card.Id, out setId, out cardId);

                    var stat = card.Stat == "-" ? string.Empty : card.Stat;
                    var strength = string.Equals(card.Type, "ICE", StringComparison.InvariantCultureIgnoreCase)
                        || string.Equals(card.Type, "Program", StringComparison.InvariantCultureIgnoreCase)
                        ? (card.Stat == "-" ? string.Empty : (string.IsNullOrEmpty(card.Stat) ? "0" : card.Stat))
                        : string.Empty;
                    var agendaPoints = string.Equals(card.Type, "Agenda", StringComparison.InvariantCultureIgnoreCase)
                        ? stat
                        : string.Empty;
                    var link = string.Equals(card.Type, "Identity", StringComparison.InvariantCultureIgnoreCase)
                        ? card.Cost
                        : string.Empty;
                    var trashCost = string.Equals(card.Type, "Asset", StringComparison.InvariantCultureIgnoreCase)
                         ? stat
                        : string.Empty;

                    var influence = card.Influence == "-" ? string.Empty : card.Influence;
                    if (string.Equals(card.Type, "Identity", StringComparison.InvariantCultureIgnoreCase))
                        influence = stat;

                    var mu = string.Equals(card.Type, "Program", StringComparison.InvariantCultureIgnoreCase)
                        ? card.Requirement
                        : string.Empty;
                    var deckSize = string.Equals(card.Type, "Identity", StringComparison.InvariantCultureIgnoreCase)
                        ? card.Requirement
                        : string.Empty;

                    var setInformation = setInformations[allCards[index].Key.Id];
                    var set = setInformation.ByChapter
                        ? string.Format("{0}-{1}({2})", setInformation.ShortName, setInformation.GetChapterId(allCards[index].Key.Name), cardId)
                        : string.Format("{0}({1})", setInformation.ShortName, cardId);

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
                        stat,
                        strength,
                        agendaPoints,
                        link,
                        trashCost,
                        influence,
                        card.Requirement,
                        mu,
                        deckSize,
                        set,
                        card.Name,
                        (GetUniversalId(setId, cardId)).ToString(),
                        "",//banned
                        "",//restricted
                        card.Id,
                        card.Flavor
                    );

                    backgroundWorker.ReportProgress((index * 100) / allCards.Count, OctgnLoaderTask.ImportCard);
                    return DatabaseManager.OperationResult.Ok;
                });
            if (backgroundWorker.CancellationPending)
                return;
            backgroundWorker.ReportProgress(100, OctgnLoaderTask.UpdateDatabase);
        }

        private static int GetUniversalId(int setId, int cardId)
        {
            return setId * 10000 + cardId;
        }

        public enum OctgnLoaderTask
        {
            Undefined,
            LoadSet,
            ImportSet,
            ImportCard,
            UpdateDatabase
        }

        public enum OctgnLoaderResult
        {
            Undefined,
            Success,
            SetsNotFound
        }

        public class OctgnSetData
        {
            public string Name { get; set; }
            public int Id { get; set; }
            public int MinCardId { get; set; }
            public int MaxCardId { get; set; }
            public int ChapterId { get; set; }
        }

        public static Dictionary<OctgnSetData, OctgnCard[]> LoadAllSets(string path, BackgroundWorker backgroundWorker)
        {
            var result = new Dictionary<OctgnSetData, OctgnCard[]>(); // key: set name

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

                var setData = new OctgnSetData();

                XmlNode setNode = doc.SelectNodes("//set")[0];
                setData.Name = setNode.Attributes["name"].Value;

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
