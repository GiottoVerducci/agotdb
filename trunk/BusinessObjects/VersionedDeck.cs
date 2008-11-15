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
    public String Author; // name of the deck author
    public String Description; // description of the deck
    public String Name; // name of the deck
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
      for (int i = 0; i < original.fDecks.Count; ++i)
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
    public Deck GetVersion(int versionIndex)
    {
      return fDecks[versionIndex];
    }

    /// <summary>
    /// Gets the latest version of the deck.
    /// </summary>
    /// <returns>The lastest version of the deck.</returns>
    public Deck GetLastVersion()
    {
      return fDecks[fDecks.Count - 1];
    }

    /// <summary>
    /// Gets the count of versions of the deck.
    /// </summary>
    /// <returns>The count of versions of the deck.</returns>
    public int GetVersionsCount()
    {
      return fDecks.Count;
    }

    /// <summary>
    /// Adds a new version to the versioned deck.
    /// </summary>
    /// <param name="CommentsForLastVersion">The comment associated to the previously latest version.</param>
    public void AddNewVersion(String CommentsForLastVersion)
    {
      GetLastVersion().RevisionComments = CommentsForLastVersion;
      GetLastVersion().Editable = false;
      fDecks.Add(Deck.CreateRevision(GetLastVersion()));
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
    public DeckLoadResult LoadFromXMLFile(string filename)
    {
      XmlDocument doc = new XmlDocument();
      string author, description, name;
      List<Deck> decks = new List<Deck>();
      try
      {
        doc.Load(filename);
        XmlNode xmlDecl = doc.FirstChild;
        XmlNode root = xmlDecl.NextSibling;
        //if (root.GetAttributeNode("DeckBuilderVersion").Value
        author = XmlToolBox.GetElementValue(doc, root, "Author");
        description = XmlToolBox.GetElementValue(doc, root, "Description");
        name = XmlToolBox.GetElementValue(doc, root, "Name");
        int i = 0;
        XmlNode deckRoot;
        Deck deck = null;
        while ((deckRoot = XmlToolBox.FindNode(doc, root, "Deck" + i)) != null)
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
    private XmlDocument BuildXMLDocument()
    {
      XmlDocument doc = new XmlDocument();
      doc.AppendChild(doc.CreateXmlDeclaration("1.0", null, null));
      XmlElement root = doc.CreateElement(fXmlRootName);
      doc.AppendChild(root);

      root.SetAttributeNode("DeckBuilderVersion", "").Value = AboutForm.Version;
      XmlToolBox.AddElementValue(doc, root, "Author", Author);
      XmlToolBox.AddElementValue(doc, root, "Description", Description);
      XmlToolBox.AddElementValue(doc, root, "Name", Name);

      for (int i = 0; i < fDecks.Count; ++i)
      {
        XmlElement deckRoot = doc.CreateElement("Deck" + i);
        deckRoot.AppendChild(fDecks[i].ToXML(doc));
        root.AppendChild(deckRoot);
      }
      return doc;
    }

    /// <summary>
    /// Saves the deck using an xml representation to a file.
    /// </summary>
    /// <param name="filename">The full path and name of the file.</param>
    /// <returns>True if the save was successful, False otherwise.</returns>
    public bool SaveToXMLFile(string filename)
    {
      XmlDocument doc = BuildXMLDocument();
      try
      {
        doc.Save(filename);
      }
      catch
      {
        MessageBox.Show(String.Format(Resource1.ErrSaveXmlDeck, filename), Resource1.ErrDeckSaveTitle, MessageBoxButtons.OK, MessageBoxIcon.Error);
        return false;
      }
      return true;
    }

    /// <summary>
    /// Compares two versioned decks by comparing their content. If both references are null, the method
    /// returns true.
    /// </summary>
    /// <param name="vdeck1">The first versioned deck</param>
    /// <param name="vdeck2">The second versioned deck</param>
    /// <returns>True if the two versioned decks are identical by content or both null, false otherwise.</returns>
    public static bool AreEqual(VersionedDeck vdeck1, VersionedDeck vdeck2)
    {
      bool result = (vdeck1.Author == vdeck2.Author) &&
                    (vdeck1.Description == vdeck2.Description) &&
                    (vdeck1.Name == vdeck2.Name) &&
                    (vdeck1.fDecks.Count == vdeck2.fDecks.Count);
      int i = 0;
      while (result && (i < vdeck1.fDecks.Count))
      {
        result &= Deck.AreEqual(vdeck1.fDecks[i], vdeck2.fDecks[i]);
        ++i;
      }
      return result;
    }
  }
}