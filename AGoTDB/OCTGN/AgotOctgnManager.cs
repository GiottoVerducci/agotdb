using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Linq;

using GenericDB.BusinessObjects;
using GenericDB.OCTGN;

using AGoTDB.BusinessObjects;

namespace AGoTDB.OCTGN
{
    public class AgotOctgnManager : OctgnManager
    {
        public AgotOctgnManager()
            : base(new AgotOctgnLoader())
        {
        }

        public static void PromptForInitialization(Action callback = null)
        {
            var dialog = new OctgnSetSelector { IsUrl = true, Url = UserSettings.OctgnSetsDownloadUrl, Items = "sets" };

            if (dialog.ShowDialog() == DialogResult.OK)
            {
                var path = dialog.Path;
                var octgnLoaderWorker = new BackgroundWorker
                {
                    WorkerReportsProgress = true,
                    WorkerSupportsCancellation = true
                };

                octgnLoaderWorker.DoWork += OctgnLoaderWorkerOnDoWork;

                var loaderForm = new OctgnLoaderForm { BackgroundWorker = octgnLoaderWorker, Path = path, Callback = callback };
                loaderForm.ShowDialog();
            }
        }

        private static void OctgnLoaderWorkerOnDoWork(object sender, DoWorkEventArgs doWorkEventArgs)
        {
            var backgroundWorker = (BackgroundWorker)sender;
            var path = (string)doWorkEventArgs.Argument;
            var loader = new AgotOctgnLoader();
            var sets = loader.LoadAllSets(path, backgroundWorker);
            if (sets.Count == 0 || sets.All(s => s.Value.Length == 0))
            {
                doWorkEventArgs.Result = new OctgnLoaderResultAndValue { Result = OctgnLoaderResult.NoSetsFounds };
                return;
            }
            AgotOctgnLoader.UpdateCards(sets, backgroundWorker);
            doWorkEventArgs.Result = new OctgnLoaderResultAndValue { Result = OctgnLoaderResult.Success };
        }

        private static readonly Dictionary<int, AgotCard> _dummyHouseCards = new Dictionary<int, AgotCard>
            {
                { (int)AgotCard.CardHouse.Baratheon, new AgotCard { OriginalName = new FormattedValue<string>("House Baratheon", null), OctgnId = UserSettings.OctgnHouseBaratheonId } },
                { (int)AgotCard.CardHouse.Greyjoy, new AgotCard { OriginalName = new FormattedValue<string>("House Greyjoy", null), OctgnId = UserSettings.OctgnHouseGreyjoyId} },
                { (int)AgotCard.CardHouse.Lannister, new AgotCard { OriginalName = new FormattedValue<string>("House Lannister", null), OctgnId = UserSettings.OctgnHouseLannisterId } },
                { (int)AgotCard.CardHouse.Martell, new AgotCard { OriginalName = new FormattedValue<string>("House Martell", null), OctgnId = UserSettings.OctgnHouseMartellId } },
                { (int)AgotCard.CardHouse.Stark, new AgotCard { OriginalName = new FormattedValue<string>("House Stark", null), OctgnId = UserSettings.OctgnHouseStarkId } },
                { (int)AgotCard.CardHouse.Targaryen, new AgotCard { OriginalName = new FormattedValue<string>("House Targaryen", null), OctgnId = UserSettings.OctgnHouseTargaryenId } },
            };

        public static void SaveOctgnDeck(AgotVersionedDeck versionedDeck, AgotDeck currentDeck)
        {
            string filename;
            using (var dialog = new SaveFileDialog())
            {
                dialog.Filter = Resource1.OctgnDeckFileFilter;
                dialog.ValidateNames = true;
                dialog.FileName = versionedDeck.Name + ".o8d"; // use deck name as default name. Todo: remove special characters
                if (dialog.ShowDialog() != DialogResult.OK)
                    return;
                filename = dialog.FileName;
            }
            var xDoc = new XDocument { Declaration = new XDeclaration("1.0", "utf-8", "yes") };
            var xDeck = new XElement("deck");
            xDeck.SetAttributeValue("game", UserSettings.OctgnGameId);
            xDoc.Add(xDeck);


            if (currentDeck.Houses != (int)AgotCard.CardHouse.Neutral)
                AddSection(xDeck, "House", _dummyHouseCards.Where(hc => (hc.Key & currentDeck.Houses) == hc.Key).Select(hc => hc.Value));

            AddSection(xDeck, "Agenda", currentDeck.Agenda);
            AddSection(xDeck, "Characters", currentDeck.CardLists[1].Where(card => card.Type.Value == (int)AgotCard.CardType.Character));
            AddSection(xDeck, "Locations", currentDeck.CardLists[1].Where(card => card.Type.Value == (int)AgotCard.CardType.Location));
            AddSection(xDeck, "Events", currentDeck.CardLists[1].Where(card => card.Type.Value == (int)AgotCard.CardType.Event));
            AddSection(xDeck, "Attachments", currentDeck.CardLists[1].Where(card => card.Type.Value == (int)AgotCard.CardType.Attachment));
            AddSection(xDeck, "Plots", currentDeck.CardLists[1].Where(card => card.Type.Value == (int)AgotCard.CardType.Plot));

            xDoc.Save(filename);
        }

        private static void AddSection(XElement xRoot, string sectionName, IEnumerable<AgotCard> agotCards)
        {
            var xSection = new XElement("section");
            xSection.SetAttributeValue("name", sectionName);
            foreach (var card in agotCards)
            {
                var xCard = new XElement("card");
                xCard.SetAttributeValue("qty", card.Quantity);
                xCard.SetAttributeValue("id", card.OctgnId);
                xCard.SetValue(card.OriginalName.Value);
                xSection.Add(xCard);
            }
            xRoot.Add(xSection);
        }

        public static AgotVersionedDeck LoadOctgnDeck()
        {
            string filename;
            using (var dialog = new OpenFileDialog())
            {
                dialog.Filter = Resource1.OctgnDeckFileFilter;
                if (dialog.ShowDialog() != DialogResult.OK)
                    return null;
                filename = dialog.FileName;
                //_currentFilename = Path.ChangeExtension(filename, "xml");
            }

            var doc = new XmlDocument();
            try
            {
                doc.Load(filename);
            }
            catch (FileNotFoundException)
            {
                MessageBox.Show(String.Format(CultureInfo.CurrentCulture, Resource1.ErrXmlDeckFileNotFound, filename),
                    Resource1.ErrDeckLoadTitle, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            XmlNode xmlDecl = doc.FirstChild;
            XmlNode root = xmlDecl.NextSibling;
            var versionedDeck = new AgotVersionedDeck { Name = Path.GetFileNameWithoutExtension(filename) };

            var cardErrorList = new List<string>();

            versionedDeck.LastVersion.Houses = ComputeOctgnHouseSection(GetSectionNode(root, "House"), cardErrorList);

            AddOctgnCardToDeck(GetSectionNode(root, "Agenda"), versionedDeck.LastVersion.Agenda, (int)AgotCard.CardType.Agenda, cardErrorList);
            var mainDeckList = versionedDeck.LastVersion.CardLists[1];
            AddOctgnCardToDeck(GetSectionNode(root, "Characters"), mainDeckList, (int)AgotCard.CardType.Character, cardErrorList);
            AddOctgnCardToDeck(GetSectionNode(root, "Locations"), mainDeckList, (int)AgotCard.CardType.Location, cardErrorList);
            AddOctgnCardToDeck(GetSectionNode(root, "Events"), mainDeckList, (int)AgotCard.CardType.Event, cardErrorList);
            AddOctgnCardToDeck(GetSectionNode(root, "Attachments"), mainDeckList, (int)AgotCard.CardType.Attachment, cardErrorList);
            AddOctgnCardToDeck(GetSectionNode(root, "Plots"), mainDeckList, (int)AgotCard.CardType.Plot, cardErrorList);

            if (cardErrorList.Count > 0)
            {
                var message = Resource1.ErrCardsNotFoundInOctgnDeckFile + Environment.NewLine
                    + string.Join(Environment.NewLine, cardErrorList);
                MessageBox.Show(message, Resource1.ErrDeckLoadTitle, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            return versionedDeck;
        }

        private static XmlNode GetSectionNode(XmlNode root, string sectionName)
        {
            return root.ChildNodes.Cast<XmlNode>().FirstOrDefault(cn => cn.Name == "section" && cn.Attributes != null && cn.Attributes["name"].Value == sectionName);
        }

        private static void ProcessOctgnSectionNode(XmlNode sectionNode, Action<Guid, int, string> processAction)
        {
            if (sectionNode == null)
                return;
            foreach (XmlElement node in sectionNode.ChildNodes)
            {
                var octgnId = new Guid(node.Attributes["id"].Value);
                int quantity;
                quantity = Int32.TryParse(node.Attributes["qty"].Value, out quantity) ? quantity : 1;
                var name = node.HasChildNodes ? node.FirstChild.Value : node.InnerText;
                processAction(octgnId, quantity, name);
            }
        }

        private static int ComputeOctgnHouseSection(XmlNode sectionNode, List<string> cardErrorList)
        {
            var result = 0;
            ProcessOctgnSectionNode(sectionNode, (octgnId, quantity, name) =>
            {
                if (_dummyHouseCards.Any(kvp => kvp.Value.OctgnId == octgnId))
                {
                    var houseCard = _dummyHouseCards.First(kvp => kvp.Value.OctgnId == octgnId);
                    result |= houseCard.Key;
                }
                else
                {
                    cardErrorList.Add(string.Format("'{0}' (id: '{1}')", name, octgnId));
                }
            });
            return result;
        }

        private static void AddOctgnCardToDeck(XmlNode sectionNode, CardList<AgotCard> cardlist, int expectedCardType, List<string> cardErrorList)
        {
            ProcessOctgnSectionNode(sectionNode, (octgnId, quantity, name) =>
            {
                var cardTable = ApplicationSettings.Instance.DatabaseManager.GetCardFromOctgnId(octgnId);
                if (cardTable.Rows.Count == 0)
                    cardErrorList.Add(string.Format("'{0}' (id: '{1}')", name, octgnId));
                else
                {
                    DataRow row = null;
                    if (cardTable.Rows.Count > 0)
                    {
                        // try to match the expected cart type
                        row = cardTable.Rows.Cast<DataRow>().LastOrDefault(r => Int32.Parse(r["Type"].ToString()) == expectedCardType);
                    }
                    if (row == null)
                        row = cardTable.Rows[cardTable.Rows.Count - 1]; // take the most recent card
                    cardlist.Add(new AgotCard(row) { Quantity = quantity });
                }
            });
        }
    }
}