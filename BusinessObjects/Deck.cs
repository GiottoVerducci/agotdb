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

namespace AGoT.AGoTDB.BusinessObjects
{
  public class Deck
  {
    public List<CardList> CardLists { get; private set; }
    public Int32 Houses { get; set; }
    public Card Agenda { get; set; }
    public String RevisionComments { get; set; }
    public DateTime CreationDate { get; private set; }
    public DateTime LastModifiedDate { get; private set; }
    public bool Editable { get; set; }

    public Deck()
    {
      CardLists = new List<CardList> { new CardList(), new CardList() }; // sideboard (0) and main deck (1)
      Agenda = null;
      RevisionComments = "";
      CreationDate = DateTime.Now;
      LastModifiedDate = DateTime.Now;
      Editable = true;
    }

    /// <summary>
    /// This constructor is used to create a deep copy (clone)
    /// </summary>
    /// <param name="originalDeck">The deck to clone.</param>
    public Deck(Deck originalDeck)
      : this()
    {
      CardLists = new List<CardList>();
      for (var i = 0; i < originalDeck.CardLists.Count; ++i)
        CardLists.Add(new CardList(originalDeck.CardLists[i]));
      Houses = originalDeck.Houses;
      Agenda = originalDeck.Agenda;
      RevisionComments = originalDeck.RevisionComments;
      CreationDate = originalDeck.CreationDate;
      LastModifiedDate = originalDeck.LastModifiedDate;
      Editable = originalDeck.Editable;
    }

    /// <summary>
    /// Creates a new revision of the deck.
    /// </summary>
    /// <param name="previousRevisionDeck">The previous revision of the deck.</param>
    public static Deck CreateRevision(Deck previousRevisionDeck)
    {
      var result = new Deck();
      result.CardLists.Clear();
      for (var i = 0; i < previousRevisionDeck.CardLists.Count; ++i)
        result.CardLists.Add(new CardList(previousRevisionDeck.CardLists[i]));
      result.Houses = previousRevisionDeck.Houses;
      result.Agenda = previousRevisionDeck.Agenda;
      return result;
    }

    /// <summary>
    /// Serializes the deck under an xml data format.
    /// </summary>
    /// <param name="doc">The xml document where the data will be stored.</param>
    public XmlElement ToXml(XmlDocument doc)
    {
      XmlElement deckRoot = doc.CreateElement("Deck");

      XmlToolbox.AddElementValue(doc, deckRoot, "RevisionComments", RevisionComments);
      XmlToolbox.AddElementValue(doc, deckRoot, "CreationDate", CreationDate.ToBinary().ToString(CultureInfo.InvariantCulture));
      XmlToolbox.AddElementValue(doc, deckRoot, "LastModifiedDate", LastModifiedDate.ToBinary().ToString(CultureInfo.InvariantCulture));
      XmlToolbox.AddElementValue(doc, deckRoot, "Houses", Houses.ToString());
      if (Agenda != null)
      {
        XmlElement agendaElement = doc.CreateElement("Agenda");
        agendaElement.AppendChild(Agenda.ToXml(doc));
        deckRoot.AppendChild(agendaElement);
      }
      for (var j = 0; j < CardLists.Count; ++j)
      {
        string nodeName = GetNodeName(j);
        XmlElement cardsElement = CardLists[j].ToXml(doc, nodeName);
        deckRoot.AppendChild(cardsElement);
      }
      return deckRoot;
    }

    /// <summary>
    /// Returns the node name for xml storage associated to the index in the list of CardList elements.
    /// </summary>
    /// <param name="cardListIndex">The index.</param>
    /// <returns>The node name.</returns>
    private static String GetNodeName(int cardListIndex)
    {
      return cardListIndex == 0
        ? "Sideboard"
        : (cardListIndex == 1 ? "Cards" : String.Format(CultureInfo.CurrentCulture, "Cards{0}", cardListIndex));
    }

    /// <summary>
    /// Initializes the deck from XML data. The data must have been generated
    /// using the ToXML method.
    /// </summary>
    /// <param name="doc">The xml document where the data is stored</param>
    /// <param name="deckRoot">The xml root node that was returned by a call to ToXML</param>
    public Deck(XmlDocument doc, XmlNode deckRoot)
      : this()
    {
      if (deckRoot.Name != "Deck")
        throw new XmlException("Invalid deck root node");
      RevisionComments = XmlToolbox.GetElementValue(doc, deckRoot, "RevisionComments");

      String value;
      if ((value = XmlToolbox.GetElementValue(doc, deckRoot, "CreationDate")) != null)
        CreationDate = DateTime.FromBinary(Int64.Parse(value, CultureInfo.InvariantCulture));
      if ((value = XmlToolbox.GetElementValue(doc, deckRoot, "LastModifiedDate")) != null)
        LastModifiedDate = DateTime.FromBinary(Int64.Parse(value, CultureInfo.InvariantCulture));
      if (!string.IsNullOrEmpty(value = XmlToolbox.GetElementValue(doc, deckRoot, "Houses")))
        Houses = Int32.Parse(value, CultureInfo.InvariantCulture);
      XmlNode agendaNode = XmlToolbox.FindNode(deckRoot, "Agenda");
      Agenda = (agendaNode != null) ? new Card(doc, agendaNode.FirstChild) : null;
      // we read the cardlists
      CardLists.Clear();
      XmlNode cardsRoot;
      var j = 0;
      while (null != (cardsRoot = XmlToolbox.FindNode(deckRoot, GetNodeName(j))))
      {
        CardLists.Add(new CardList(doc, cardsRoot));
        ++j;
      }
    }

    /// <summary>
    /// Compares two decks by comparing their content. If both references are null, the method
    /// returns true. If one reference is null and the other isn't, the methode returns false.
    /// </summary>
    /// <param name="deck1">The first deck</param>
    /// <param name="deck2">The second deck</param>
    /// <returns>True if the two decks are identical by content or both null, false otherwise.</returns>
    public static bool AreEqual(Deck deck1, Deck deck2)
    {
      if ((deck1 == null) && (deck2 == null))
        return true;
      if ((deck1 == null) || (deck2 == null))
        return false;
      if ((deck1.Houses != deck2.Houses) || (!Card.AreEqual(deck1.Agenda, deck2.Agenda)) ||
          (deck1.RevisionComments != deck2.RevisionComments) || (deck1.CreationDate.CompareTo(deck2.CreationDate) != 0) ||
          (deck1.LastModifiedDate.CompareTo(deck2.LastModifiedDate) != 0) ||
          (deck1.CardLists.Count != deck2.CardLists.Count))
        return false;

      for (var i = 0; i < deck1.CardLists.Count; ++i)
        if (!CardList.AreEqual(deck1.CardLists[i], deck2.CardLists[i]))
          return false;
      return true;
    }
  }

}
