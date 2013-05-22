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

using AndroidDB;
using NRADB.BusinessObjects;

namespace NRADB.OCTGN
{
    public static class OctgnManager
    {
        public static void PromptForInitialization(Action callback = null)
        {
            var dialog = new OctgnSetSelector { IsUrl = true, Url = UserSettings.OctgnSetsDownloadUrl };

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
            var sets = OctgnLoader.LoadAllSets(path, backgroundWorker);
            if (sets.Count == 0 || sets.All(s => s.Value.Length == 0))
            {
                doWorkEventArgs.Result = new OctgnLoader.OctgnLoaderResultAndValue { Result = OctgnLoader.OctgnLoaderResult.NoSetsFounds };
                return;
            }

            //OctgnLoader.ImportSets(sets, backgroundWorker);

            var importCardResult = OctgnLoader.ImportCards(sets, backgroundWorker);
            if (importCardResult.IsSuccessful)
                doWorkEventArgs.Result = new OctgnLoader.OctgnLoaderResultAndValue { Result = OctgnLoader.OctgnLoaderResult.Success };
            else
            {
                if (importCardResult.SetNotFound != null)
                    doWorkEventArgs.Result = new OctgnLoader.OctgnLoaderResultAndValue
                    {
                        Result = OctgnLoader.OctgnLoaderResult.SetNotDefinedInDatabase,
                        Value = importCardResult.SetNotFound
                    };
            }
        }

        public static void SaveOctgnDeck(NraVersionedDeck versionedDeck, NraDeck currentDeck)
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

            AddSection(xDeck, "Identity", currentDeck.CardLists[1].Where(card => card.Type.Value == (int)NraCard.CardType.Identity));
            AddSection(xDeck, "R&D / Stack", currentDeck.CardLists[1].Where(card => card.Type.Value != (int)NraCard.CardType.Identity));

            xDoc.Save(filename);
        }

        private static void AddSection(XElement xRoot, string sectionName, IEnumerable<NraCard> NraCards)
        {
            var xSection = new XElement("section");
            xSection.SetAttributeValue("name", sectionName);
            foreach (var card in NraCards)
            {
                var xCard = new XElement("card");
                xCard.SetAttributeValue("qty", card.Quantity);
                xCard.SetAttributeValue("id", card.OctgnId);
                xCard.SetValue(card.OriginalName.Value);
                xSection.Add(xCard);
            }
            xRoot.Add(xSection);
        }

        public static NraVersionedDeck LoadOctgnDeck()
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
            var versionedDeck = new NraVersionedDeck { Name = Path.GetFileNameWithoutExtension(filename) };

            var cardErrorList = new List<string>();

            //versionedDeck.LastVersion.Houses = ComputeOctgnHouseSection(GetSectionNode(root, "House"), cardErrorList);

            //AddOctgnCardToDeck(GetSectionNode(root, "Agenda"), versionedDeck.LastVersion.Agenda, (int)NraCard.CardType.Agenda, cardErrorList);
            var mainDeckList = versionedDeck.LastVersion.CardLists[1];
            AddOctgnCardToDeck(GetSectionNode(root, "Identity"), mainDeckList, (int)NraCard.CardType.Identity, cardErrorList);
            AddOctgnCardToDeck(GetSectionNode(root, "R&D / Stack"), mainDeckList, (int)NraCard.CardType.None, cardErrorList);

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

        private static void AddOctgnCardToDeck(XmlNode sectionNode, CardList<NraCard> cardlist, int expectedCardType, List<string> cardErrorList)
        {
            ProcessOctgnSectionNode(sectionNode, (octgnId, quantity, name) =>
            {
                var cardTable = ApplicationSettings.DatabaseManager.GetCardFromOctgnId(octgnId);
                if (cardTable.Rows.Count == 0)
                    cardErrorList.Add(string.Format("'{0}' (id: '{1}')", name, octgnId));
                else
                {
                    DataRow row = null;
                    if (cardTable.Rows.Count > 1 && expectedCardType > 0)
                    {
                        // try to match the expected card type
                        row = cardTable.Rows.Cast<DataRow>().LastOrDefault(r => Int32.Parse(r["Type"].ToString()) == expectedCardType);
                    }
                    if (row == null)
                        row = cardTable.Rows[cardTable.Rows.Count - 1]; // take the most recent card
                    cardlist.Add(new NraCard(row) { Quantity = quantity });
                }
            });
        }
    }
}
