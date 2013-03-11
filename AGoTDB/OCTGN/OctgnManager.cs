using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Linq;

using GenericDB.BusinessObjects;

using AGoTDB.BusinessObjects;

namespace AGoTDB.OCTGN
{
    public static class OctgnManager
    {
        public static void PromptForInitialization(Action callback = null)
        {
            var dialog = new FolderBrowserDialog
            {
                Description = "Select path for OCTGN set files",
                ShowNewFolderButton = false
            };

            if (dialog.ShowDialog() == DialogResult.OK)
            {
                var octgnLoaderWorker = new BackgroundWorker
                {
                    WorkerReportsProgress = true,
                    WorkerSupportsCancellation = true
                };

                octgnLoaderWorker.DoWork += OctgnLoaderWorkerOnDoWork;

                var loaderForm = new OctgnLoaderForm { BackgroundWorker = octgnLoaderWorker, Path = dialog.SelectedPath, Callback = callback };
                loaderForm.ShowDialog();
            }
        }

        private static void OctgnLoaderWorkerOnDoWork(object sender, DoWorkEventArgs doWorkEventArgs)
        {
            var backgroundWorker = (BackgroundWorker)sender;
            var path = (string)doWorkEventArgs.Argument;
            var sets = OctgnLoader.LoadAllSets(path, backgroundWorker);
            if (sets.Count == 0 || sets.All(s => s.Value.Count == 0))
            {
                doWorkEventArgs.Result = OctgnLoader.OctgnLoaderResult.SetsNotFound;
                return;
            }
            OctgnLoader.UpdateCards(sets, backgroundWorker);
            doWorkEventArgs.Result = OctgnLoader.OctgnLoaderResult.Success;
        }

        private static readonly Dictionary<int, AgotCard> _dummyHouseCards = new Dictionary<int, AgotCard>
            {
                { (int)AgotCard.CardHouse.Baratheon, new AgotCard { OriginalName = new FormattedValue<string>("House Baratheon", null), OctgnId = new Guid("a12af4e8-be4b-4cda-a6b6-534f97001001") } },
                { (int)AgotCard.CardHouse.Greyjoy, new AgotCard { OriginalName = new FormattedValue<string>("House Greyjoy", null), OctgnId = new Guid("a12af4e8-be4b-4cda-a6b6-534f97001002") } },
                { (int)AgotCard.CardHouse.Lannister, new AgotCard { OriginalName = new FormattedValue<string>("House Lannister", null), OctgnId = new Guid("a12af4e8-be4b-4cda-a6b6-534f97001003") } },
                { (int)AgotCard.CardHouse.Martell, new AgotCard { OriginalName = new FormattedValue<string>("House Martell", null), OctgnId = new Guid("a12af4e8-be4b-4cda-a6b6-534f97001004") } },
                { (int)AgotCard.CardHouse.Stark, new AgotCard { OriginalName = new FormattedValue<string>("House Stark", null), OctgnId = new Guid("a12af4e8-be4b-4cda-a6b6-534f97001005") } },
                { (int)AgotCard.CardHouse.Targaryen, new AgotCard { OriginalName = new FormattedValue<string>("House Targaryen", null), OctgnId = new Guid("a12af4e8-be4b-4cda-a6b6-534f97001006") } },
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
            xDeck.SetAttributeValue("game", Guid.NewGuid());
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
            var xSection = new XElement(sectionName);
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

            var houseNode = XmlToolbox.FindNode(root, "House");
            versionedDeck.LastVersion.Houses = ComputeOctgnHouseSection(houseNode, cardErrorList);

            AddOctgnCardToDeck(XmlToolbox.FindNode(root, "Agenda"), versionedDeck.LastVersion.Agenda, cardErrorList);
            var mainDeckList = versionedDeck.LastVersion.CardLists[1];
            AddOctgnCardToDeck(XmlToolbox.FindNode(root, "Characters"), mainDeckList, cardErrorList);
            AddOctgnCardToDeck(XmlToolbox.FindNode(root, "Locations"), mainDeckList, cardErrorList);
            AddOctgnCardToDeck(XmlToolbox.FindNode(root, "Events"), mainDeckList, cardErrorList);
            AddOctgnCardToDeck(XmlToolbox.FindNode(root, "Attachments"), mainDeckList, cardErrorList);
            AddOctgnCardToDeck(XmlToolbox.FindNode(root, "Plots"), mainDeckList, cardErrorList);

            if (cardErrorList.Count > 0)
            {
                var message = Resource1.ErrCardsNotFoundInOctgnDeckFile + Environment.NewLine
                    + string.Join(Environment.NewLine, cardErrorList);
                MessageBox.Show(message, Resource1.ErrDeckLoadTitle, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            return versionedDeck;
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

        private static void AddOctgnCardToDeck(XmlNode sectionNode, CardList<AgotCard> cardlist, List<string> cardErrorList)
        {
            ProcessOctgnSectionNode(sectionNode, (octgnId, quantity, name) =>
            {
                var cardTable = ApplicationSettings.DatabaseManager.GetCardFromOctgnId(octgnId);
                if (cardTable.Rows.Count == 0)
                    cardErrorList.Add(string.Format("'{0}' (id: '{1}')", name, octgnId));
                else
                    cardlist.Add(new AgotCard(cardTable.Rows[0]) { Quantity = quantity });
            });
        }
    }
}