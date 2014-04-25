using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Xml;

using GenericDB.Helper;

namespace GenericDB.OCTGN
{
    public interface IOctgnLoader
    {
        void ImportAllImages(string imagePath, string o8cPath, BackgroundWorker backgroundWorker);
    }
    
    public class OctgnLoader<TOctgnCard>: IOctgnLoader
        where TOctgnCard : IOctgnCard, new()
    {
        private static readonly List<Tuple<PropertyInfo, OctgnCardDataAttribute>> _metadata =
            new List<Tuple<PropertyInfo, OctgnCardDataAttribute>>();

        static OctgnLoader()
        {
            PropertyInfo[] propertyInfos = typeof(TOctgnCard).GetProperties();
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

        public virtual void ExtractSetIdAndCardIdFromOctgnId(string octgnId, out int setId, out int cardId)
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

        protected static int GetUniversalId(int setId, int cardId)
        {
            return setId * 10000 + cardId;
        }

        public enum OctgnLoaderTask
        {
            Undefined,
            LoadSet,
            ImportSet,
            ImportCard,
            UpdateDatabase,
            MatchSet,
            FindCard
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

        public void ImportAllImages(string imagePath, string o8cPath, BackgroundWorker backgroundWorker)
        {
            Directory.CreateDirectory(imagePath);
            var tempFolderPath = Path.GetTempPath() + Path.GetRandomFileName();
            Directory.CreateDirectory(tempFolderPath);
            var filePaths = ZipHelper.UnZipFile(o8cPath, tempFolderPath, filePattern: "*.jpg");

            foreach (var filePath in filePaths)
            {
                var newPath = imagePath + Path.DirectorySeparatorChar + Path.GetFileName(filePath);
                if (File.Exists(newPath))
                    File.Delete(newPath);
                File.Move(filePath, newPath);
            }
            var tempDirectoryInfo = new DirectoryInfo(tempFolderPath);
            tempDirectoryInfo.Delete(true);
        }

        public virtual Dictionary<OctgnSetData, TOctgnCard[]> LoadAllSets(string path, BackgroundWorker backgroundWorker)
        {
            var result = new Dictionary<OctgnSetData, TOctgnCard[]>();

            var progress = 0;
            var tempFolderPath = Path.GetTempPath() + Path.GetRandomFileName();
            Directory.CreateDirectory(tempFolderPath);
            var filePaths = ZipHelper.UnZipFile(path, tempFolderPath, filePattern: "*set.xml");

            var totalFileCount = filePaths.Count;

            foreach (var setFilePath in filePaths) //FileInfo fileInfo in fileInfos)
            {
                if (backgroundWorker.CancellationPending)
                    return null;
                backgroundWorker.ReportProgress(progress * 100 / totalFileCount, OctgnLoaderTask.LoadSet);

                var doc = new XmlDocument();
                try
                {
                    doc.Load(setFilePath);
                }
                catch (Exception ex)
                {
                    throw new Exception("Couldn't read set definition file " + setFilePath, ex);
                }

                var setData = new OctgnSetData();

                XmlNode setNode = doc.SelectNodes("//set")[0];
                setData.Name = setNode.Attributes["name"].Value;

                if (setData.Name == "Markers")
                    continue;

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

                ++progress;
            }
            var tempDirectoryInfo = new DirectoryInfo(tempFolderPath);
            tempDirectoryInfo.Delete(true);

            backgroundWorker.ReportProgress(100, OctgnLoaderTask.LoadSet);
            return result;
        }

        private static TOctgnCard LoadOctgnCard(XmlNode cardNode)
        {
            var card = new TOctgnCard();
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

        public delegate void Setter(TOctgnCard card, string value);

        private static Action<TOctgnCard, string> GetSetter(string propertyName)
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
