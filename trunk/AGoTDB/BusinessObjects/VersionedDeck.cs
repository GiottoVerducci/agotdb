// AGoTDB - A card searcher and deck builder tool for the CCG "A Game of Thrones"
// Copyright © 2007, 2008, 2009 Vincent Ripoll
// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.

// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.

// You should have received a copy of the GNU General Public License
// along with this program.  If not, see <http://www.gnu.org/licenses/>.
// You can contact me at v.ripoll@gmail.com
// © A Game of Thrones 2005 George R. R. Martin
// © A Game of Thrones CCG 2005 Fantasy Flight Games Inc.
// © Le Trône de Fer JCC 2005-2007 Stratagèmes éditions / Xénomorphe Sàrl

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Xml;
using System.Windows.Forms;
using AGoT.AGoTDB.Forms;

namespace AGoT.AGoTDB.BusinessObjects
{
  /// <summary>
  /// Represents a deck through time, from its first to its last version.
  /// </summary>
  public class VersionedDeck
  {
    private readonly List<Deck> fDecks;
    /// <summary>
    /// Name of the deck author.
    /// </summary>
    public String Author { get; set; }
    /// <summary>
    /// Description of the deck, by its author.
    /// </summary>
    public String Description { get; set; }
    /// <summary>
    /// Name of the deck, by its author.
    /// </summary>
    public String Name { get; set; }
    private const String fXmlRootName = "VersionedDeck";

    /// <summary>
    /// Initializes a new instance of VersionedDeck with default values. It contains only an empty current deck.
    /// </summary>
    public VersionedDeck()
    {
      fDecks = new List<Deck>();
      fDecks.Add(new Deck());
      Author = "";
      Description = "";
      Name = "";
    }

    /// <summary>
    /// Initializes a new instance of VersionedDeck by cloning the given versioned deck 
    /// by performing a deep copy.
    /// </summary>
    /// <param name="original">The deck to clone.</param>
    public VersionedDeck(VersionedDeck original)
    {
      fDecks = new List<Deck>();
      for (var i = 0; i < original.fDecks.Count; ++i)
        fDecks.Add(new Deck(original.fDecks[i]));
      Author = original.Author;
      Description = original.Description;
      Name = original.Name;
    }

    /// <summary>
    /// Gets the version of the deck of given index.
    /// </summary>
    /// <remarks>The deck of index 0 is the initial deck version. 
    /// The deck of index equal to GetVersionsCount - 1 is the latest deck version.</remarks>
    /// <param name="versionIndex">The index of the version of the deck to get.</param>
    /// <returns>The <paramref name="versionIndex"/> version of the deck.</returns>
    public Deck this[int versionIndex]
    {
      get { return fDecks[versionIndex]; }
    }

    /// <summary>
    /// Gets the latest version of the deck.
    /// </summary>
    /// <returns>The lastest version of the deck.</returns>
    public Deck LastVersion
    {
      get { return fDecks[fDecks.Count - 1]; }
    }

    /// <summary>
    /// Gets the count of versions of the deck.
    /// </summary>
    /// <returns>The count of versions of the deck.</returns>
    public int Count
    {
      get { return fDecks.Count; }
    }

    /// <summary>
    /// Adds a new version to the versioned deck.
    /// </summary>
    /// <param name="commentsForLastVersion">The comment associated to the previously latest version.</param>
    public void AddNewVersion(String commentsForLastVersion)
    {
      LastVersion.RevisionComments = commentsForLastVersion;
      LastVersion.Editable = false;
      fDecks.Add(Deck.CreateRevision(LastVersion));
    }

    /// <summary>
    /// Indicates the result of the LoadFromXMLFile method.
    /// </summary>
    public enum DeckLoadResult
    {
      Undefined = 0,
      Success,
      FileNotFound,
      InvalidContent
    }

    /// <summary>
    /// Initializes this instance with data from an xml file.
    /// </summary>
    /// <param name="filename">The full path and name of the xml file.</param>
    /// <returns>A value indicating if the load was successful or if not, why it wasn't.</returns>
    public DeckLoadResult LoadFromXmlFile(string filename)
    {
      var doc = new XmlDocument();
      string author, description, name;
      var decks = new List<Deck>();
      try
      {
        doc.Load(filename);
        XmlNode xmlDecl = doc.FirstChild;
        XmlNode root = xmlDecl.NextSibling;
        //if (root.GetAttributeNode("DeckBuilderVersion").Value
        author = XmlToolbox.GetElementValue(doc, root, "Author");
        description = XmlToolbox.GetElementValue(doc, root, "Description");
        name = XmlToolbox.GetElementValue(doc, root, "Name");
        int i = 0;
        XmlNode deckRoot;
        Deck deck = null;
        while ((deckRoot = XmlToolbox.FindNode(root, "Deck" + i)) != null)
        {
          if (deck != null) // allows us to affect all decks but the last
            deck.Editable = false;
          deck = new Deck(doc, deckRoot.FirstChild);
          decks.Add(deck);
          ++i;
        }
        if (decks.Count == 0)
          return DeckLoadResult.InvalidContent;
      }
      catch (System.IO.FileNotFoundException)
      {
        return DeckLoadResult.FileNotFound;
      }
      // the deck file was successfully loaded
      Author = author;
      Description = description;
      Name = name;
      fDecks.Clear();
      fDecks.AddRange(decks);
      return DeckLoadResult.Success;
    }

    /// <summary>
    /// Builds an xml document representing this versioned deck.
    /// </summary>
    /// <returns>The xml document.</returns>
    private XmlDocument BuildXmlDocument()
    {
      XmlDocument doc = new XmlDocument();
      doc.AppendChild(doc.CreateXmlDeclaration("1.0", null, null));
      XmlElement root = doc.CreateElement(fXmlRootName);
      doc.AppendChild(root);

      root.SetAttributeNode("DeckBuilderVersion", "").Value = ApplicationSettings.ApplicationVersion.ToString();
      if (DatabaseInterface.Singleton.DatabaseInfos.Count > 0)
        root.SetAttributeNode("DatabaseVersion", "").Value = DatabaseInterface.Singleton.DatabaseInfos[0].VersionId.ToString(CultureInfo.InvariantCulture);
      XmlToolbox.AddElementValue(doc, root, "Author", Author);
      XmlToolbox.AddElementValue(doc, root, "Description", Description);
      XmlToolbox.AddElementValue(doc, root, "Name", Name);

      for (var i = 0; i < fDecks.Count; ++i)
      {
        XmlElement deckRoot = doc.CreateElement("Deck" + i);
        deckRoot.AppendChild(fDecks[i].ToXml(doc));
        root.AppendChild(deckRoot);
      }
      return doc;
    }

    /// <summary>
    /// Saves the deck using an xml representation to a file.
    /// </summary>
    /// <param name="filename">The full path and name of the file.</param>
    /// <returns>True if the save was successful, False otherwise.</returns>
    public bool SaveToXmlFile(string filename)
    {
      XmlDocument doc = BuildXmlDocument();
      try
      {
        doc.Save(filename);
      }
      catch
      {
        MessageBox.Show(String.Format(CultureInfo.CurrentCulture, Resource1.ErrSaveXmlDeck, filename), Resource1.ErrDeckSaveTitle, MessageBoxButtons.OK, MessageBoxIcon.Error);
        return false;
      }
      return true;
    }

    /// <summary>
    /// Compares two versioned decks by comparing their content. If both references are null, the method
    /// returns true.
    /// </summary>
    /// <param name="first">The first versioned deck.</param>
    /// <param name="second">The second versioned deck.</param>
    /// <returns>True if the two versioned decks are identical by content or both null, false otherwise.</returns>
    public static bool AreEqual(VersionedDeck first, VersionedDeck second)
    {
      if ((first.Author != second.Author) ||
        (first.Description != second.Description) ||
        (first.Name != second.Name) ||
        (first.fDecks.Count != second.fDecks.Count))
        return false;

      for (var i = 0; i < first.fDecks.Count; ++i)
        if (!Deck.AreEqual(first.fDecks[i], second.fDecks[i]))
          return false;
      return true;
    }
  }
}