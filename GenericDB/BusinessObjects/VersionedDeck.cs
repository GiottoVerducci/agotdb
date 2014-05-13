// GenericDB - A generic card searcher and deck builder library for CCGs
// Copyright © 2007, 2008, 2009, 2010, 2011, 2012, 2013, 2014 Vincent Ripoll
// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.

// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.

// You should have received a copy of the GNU General Public License
// along with this program. If not, see <http://www.gnu.org/licenses/>.
// You can contact me at v.ripoll@gmail.com

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Xml;

namespace GenericDB.BusinessObjects
{
    /// <summary>
    /// Represents a deck through time, from its first to its last version.
    /// </summary>
    public abstract class VersionedDeck<TD, TCL, TC> : IVersionedDeck<TD, TCL, TC>
        where TC : class, ICard, new()
        where TCL : class, ICardList<TC>, new()
        where TD : class, IDeck<TCL, TC>, new()
    {
        protected readonly List<TD> _decks;

        /// <summary>
        /// Name of the deck author.
        /// </summary>
        public string Author { get; set; }
        /// <summary>
        /// Description of the deck, by its author.
        /// </summary>
        public string Description { get; set; }
        /// <summary>
        /// Name of the deck, by its author.
        /// </summary>
        public string Name { get; set; }

        #region Constructors and clone
        /// <summary>
        /// Initializes a new instance of VersionedDeck with default values. It contains only an empty current deck.
        /// </summary>
        protected VersionedDeck()
        {
            _decks = new List<TD> { new TD() };
            Author = "";
            Description = "";
            Name = "";
        }

        /// <summary>
        /// Initializes a new instance of VersionedDeck by cloning the given versioned deck 
        /// by performing a deep copy.
        /// </summary>
        /// <param name="original">The deck to clone.</param>
        protected VersionedDeck(VersionedDeck<TD, TCL, TC> original)
        {
            _decks = new List<TD>();
            for (var i = 0; i < original._decks.Count; ++i)
                //fDecks.Add(new Deck(original.fDecks[i]));
                _decks.Add((TD)original._decks[i].Clone());
            Author = original.Author;
            Description = original.Description;
            Name = original.Name;
        }

        public abstract IVersionedDeck<TD, TCL, TC> Clone();
        #endregion

        #region Implementation of IXmlizable

        /// <summary>
        /// Gets the XML representation of this versioned deck.
        /// </summary>
        /// <param name="doc">The XML document for which the XML representation is generated.</param>
        /// <returns>A XML node representing this card.</returns>
        public XmlNode ToXml(XmlDocument doc)
        {
            XmlElement root = doc.CreateElement("VersionedDeck");
            XmlToolbox.AddElementValue(doc, root, "Author", Author);
            XmlToolbox.AddElementValue(doc, root, "Description", Description);
            XmlToolbox.AddElementValue(doc, root, "Name", Name);

            for (var i = 0; i < _decks.Count; ++i)
            {
                XmlElement deckRoot = doc.CreateElement("Deck" + i);
                deckRoot.AppendChild(_decks[i].ToXml(doc));
                root.AppendChild(deckRoot);
            }
            return root;
        }

        /// <summary>
        /// Initializes the properties of this versioned deck from an XML node that was generated 
        /// using the ToXml method.
        /// </summary>
        /// <param name="doc">The XML document containing the XML node.</param>
        /// <param name="root">The XML node containing the XML data representing the object.</param>
        public void InitializeFromXml(XmlDocument doc, XmlNode root)
        {
            //if (root.GetAttributeNode("DeckBuilderVersion").Value
            Author = XmlToolbox.GetElementValue(doc, root, "Author");
            Description = XmlToolbox.GetElementValue(doc, root, "Description");
            Name = XmlToolbox.GetElementValue(doc, root, "Name");

            _decks.Clear();
            int i = 0;
            XmlNode deckRoot;
            TD deck = null;
            while ((deckRoot = XmlToolbox.FindNode(root, "Deck" + i)) != null)
            {
                if (deck != null) // allows us to affect all decks but the last
                    deck.Editable = false;
                //deck = new Deck(doc, deckRoot.FirstChild);
                deck = new TD();
                deck.InitializeFromXml(doc, deckRoot.FirstChild);
                _decks.Add(deck);
                ++i;
            }
        }
        #endregion

        #region Equality
        public override bool Equals(object obj)
        {
            // if parameter cannot be cast to this, return false
            var versionedDeck = obj as VersionedDeck<TD, TCL, TC>;
            if (versionedDeck == null)
            {
                return false;
            }

            if ((Author != versionedDeck.Author) ||
                (Description != versionedDeck.Description) ||
                (Name != versionedDeck.Name) ||
                (_decks.Count != versionedDeck._decks.Count))
                return false;

            for (var i = 0; i < _decks.Count; ++i)
                if (!StaticComparer.AreEqual(_decks[i], versionedDeck._decks[i]))
                    return false;
            return true;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
        #endregion

        /// <summary>
        /// Gets the version of the deck of given index.
        /// </summary>
        /// <remarks>The deck of index 0 is the initial deck version. 
        /// The deck of index equal to GetVersionsCount - 1 is the latest deck version.</remarks>
        /// <param name="versionIndex">The index of the version of the deck to get.</param>
        /// <returns>The <paramref name="versionIndex"/> version of the deck.</returns>
        public TD this[int versionIndex]
        {
            get { return _decks[versionIndex]; }
        }

        /// <summary>
        /// Gets the latest version of the deck.
        /// </summary>
        /// <returns>The lastest version of the deck.</returns>
        public TD LastVersion
        {
            get { return _decks[_decks.Count - 1]; }
        }

        /// <summary>
        /// Gets the count of versions of the deck.
        /// </summary>
        /// <returns>The count of versions of the deck.</returns>
        public int Count
        {
            get { return _decks.Count; }
        }

        /// <summary>
        /// Adds a new version to the versioned deck.
        /// </summary>
        /// <param name="commentsForLastVersion">The comment associated to the former latest version.</param>
        public virtual void AddNewVersion(String commentsForLastVersion)
        {
            LastVersion.RevisionComments = commentsForLastVersion;
            LastVersion.Editable = false;
            _decks.Add((TD)LastVersion.CreateRevision());
        }

        public DeckLoadResult LoadFromXmlFile(string filename)
        {
            var doc = new XmlDocument();
            try
            {
                doc.Load(filename);
                XmlNode xmlDecl = doc.FirstChild;
                XmlNode root = xmlDecl.NextSibling;

                // tweak for legacy format
                if ((root.FirstChild != null) && (root.FirstChild.Name == "VersionedDeck"))
                    root = root.FirstChild;
                // end of tweak

                InitializeFromXml(doc, root);

                if (_decks.Count == 0)
                    return DeckLoadResult.InvalidContent;
            }
            catch (System.IO.FileNotFoundException)
            {
                return DeckLoadResult.FileNotFound;
            }
            // the deck file was successfully loaded
            return DeckLoadResult.Success;
        }

        public DeckSaveResult SaveToXmlFile(string filename, string applicationName, SoftwareVersion applicationVersion, int? databaseVersion)
        {
            XmlDocument doc = BuildXmlDocument(applicationName, applicationVersion, databaseVersion);
            try
            {
                doc.Save(filename);
            }
            catch
            {
                return DeckSaveResult.Failure;
            }
            return DeckSaveResult.Success;
        }

        /// <summary>
        /// Builds an xml document representing this versioned deck.
        /// </summary>
        /// <returns>The xml document.</returns>
        private XmlDocument BuildXmlDocument(string softwareName, SoftwareVersion softwareVersion, int? databaseVersion)
        {
            var doc = new XmlDocument();
            doc.AppendChild(doc.CreateXmlDeclaration("1.0", null, null));
            XmlElement root = doc.CreateElement(softwareName);
            root.SetAttributeNode("DeckBuilderVersion", "").Value = softwareVersion.ToString();
            if (databaseVersion != null)
                root.SetAttributeNode("DatabaseVersion", "").Value = databaseVersion.Value.ToString(CultureInfo.InvariantCulture);
            var vdRoot = ToXml(doc);
            root.AppendChild(vdRoot);
            doc.AppendChild(root);
            return doc;
        }
    }

}