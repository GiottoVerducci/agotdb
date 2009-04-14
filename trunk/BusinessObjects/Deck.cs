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

namespace AGoT.AGoTDB.BusinessObjects
{
  public class Deck
  {
    public List<Card> Cards { get; private set; }
    public List<Card> Sideboard { get; private set; }
    public Int32 Houses { get; set; }
    public Card Agenda { get; set; }
    public String RevisionComments { get; set; }
    public DateTime CreationDate { get; private set; }
    public DateTime LastModifiedDate { get; private set; }
    public bool Editable { get; set; }

    public Deck()
    {
      Cards = new List<Card>();
      Sideboard = new List<Card>();
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
      CopyCardList(originalDeck.Cards, Cards);
      CopyCardList(originalDeck.Sideboard, Sideboard);
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
      CopyCardList(previousRevisionDeck.Cards, result.Cards);
      CopyCardList(previousRevisionDeck.Sideboard, result.Sideboard);
      result.Houses = previousRevisionDeck.Houses;
      result.Agenda = previousRevisionDeck.Agenda;
      return result;
    }

    private static void CopyCardList(IList<Card> source, ICollection<Card> dest)
    {
      for (var i = 0; i < source.Count; ++i)
        dest.Add(new Card(source[i]));
    }

    /// <summary>
    /// Serializes the deck under an xml data format.
    /// </summary>
    /// <param name="doc">The xml document where the data will be stored.</param>
    public XmlElement ToXML(XmlDocument doc)
    {
      XmlElement deckRoot = doc.CreateElement("Deck");

      XmlToolBox.AddElementValue(doc, deckRoot, "RevisionComments", RevisionComments);
      XmlToolBox.AddElementValue(doc, deckRoot, "CreationDate", CreationDate.ToBinary().ToString());
      XmlToolBox.AddElementValue(doc, deckRoot, "LastModifiedDate", LastModifiedDate.ToBinary().ToString());
      XmlToolBox.AddElementValue(doc, deckRoot, "Houses", Houses.ToString());
      if (Agenda != null)
      {
        XmlElement agendaElement = doc.CreateElement("Agenda");
        agendaElement.AppendChild(Agenda.ToXml(doc));
        deckRoot.AppendChild(agendaElement);
      }
      XmlElement cardsElement = doc.CreateElement("Cards");
      for (var i = 0; i < Cards.Count; ++i)
        cardsElement.AppendChild(Cards[i].ToXml(doc));
      deckRoot.AppendChild(cardsElement);
      XmlElement sideboardElement = doc.CreateElement("Sideboard");
      for (var i = 0; i < Sideboard.Count; ++i)
        cardsElement.AppendChild(Sideboard[i].ToXml(doc));
      deckRoot.AppendChild(sideboardElement);
      return deckRoot;
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
        throw new Exception("Invalid deck root node");
      RevisionComments = XmlToolBox.GetElementValue(doc, deckRoot, "RevisionComments");

      String value;
      if ((value = XmlToolBox.GetElementValue(doc, deckRoot, "CreationDate")) != null)
        CreationDate = DateTime.FromBinary(Int64.Parse(value));
      if ((value = XmlToolBox.GetElementValue(doc, deckRoot, "LastModifiedDate")) != null)
        LastModifiedDate = DateTime.FromBinary(Int64.Parse(value));
      if (((value = XmlToolBox.GetElementValue(doc, deckRoot, "Houses")) != null) && (value != ""))
        Houses = Int32.Parse(value);
      XmlNode agendaNode = XmlToolBox.FindNode(doc, deckRoot, "Agenda");
      Agenda = (agendaNode != null) ? new Card(doc, agendaNode.FirstChild) : null;
      XmlNode cardsRoot = XmlToolBox.FindNode(doc, deckRoot, "Cards");
      if (cardsRoot != null)
      {
        foreach (XmlNode cardNode in cardsRoot.ChildNodes)
          Cards.Add(new Card(doc, cardNode));
      }
      XmlNode sideboardRoot = XmlToolBox.FindNode(doc, deckRoot, "Sideboard");
      if (sideboardRoot != null)
      {
        foreach (XmlNode sidecardNode in sideboardRoot.ChildNodes)
          Sideboard.Add(new Card(doc, sidecardNode));
      }
    }

    /// <summary>
    /// Adds a card to the deck. If the card is already present, increment
    /// the quantity by 1.
    /// </summary>
    /// <param name="card">The card to add to the deck</param>
    /// <returns>The card added or modified</returns>
    public Card AddCard(Card card)
    {
      return AddCardToList(card, Cards);
    }

    /// <summary>
    /// Substracts a card from the deck by decreasing the quantity by 1.
    /// If the quantity reaches 0, removes the card from the list.
    /// </summary>
    /// <param name="card">The card to substract from the deck</param>
    /// <returns>The card substracted, or null if the card was not found or the last copy was removed.</returns>
    public Card SubstractCard(Card card)
    {
      return SubstractCardFromList(card, Cards);
    }

    /// <summary>
    /// Adds a card to the sideboard. If the card is already present, increment
    /// the quantity by 1.
    /// </summary>
    /// <param name="card">The card to add to the sideboard</param>
    /// <returns>The card added or modified</returns>
    public Card AddCardToSideboard(Card card)
    {
      return AddCardToList(card, Sideboard);
    }

    /// <summary>
    /// Substracts a card from the sideboard by decreasing the quantity by 1.
    /// If the quantity reaches 0, removes the card from the list.
    /// </summary>
    /// <param name="card">The card to substract from the sideboard</param>
    /// <returns>The card substracted, or null if the card was not found or the last copy was removed.</returns>
    public Card SubstractCardFromSideboard(Card card)
    {
      return SubstractCardFromList(card, Sideboard);
    }

    /// <summary>
    /// Adds a card to a list of cards. If the card is already present, increment
    /// the quantity by 1.
    /// </summary>
    /// <param name="card">The card to add to the list.</param>
    /// <param name="list">The list to add the card to.</param>
    /// <returns>The card added or modified</returns>
    private static Card AddCardToList(Card card, List<Card> list)
    {
      Card result = list.Find(c => (c.UniversalId == card.UniversalId));
      if (result != null)
        result.Quantity++;
      else
      {
        result = new Card(card) { Quantity = 1 };
        list.Add(result);
      }
      return result;
    }

    /// <summary>
    /// Substracts a card from a list of cards by decreasing the quantity by 1.
    /// If the quantity reaches 0, removes the card from the list.
    /// </summary>
    /// <param name="card">The card to substract from the list</param>
    /// <param name="list">The list from which the card must be substracted.</param>
    /// <returns>The card substracted, or null if the card was not found or the last copy was removed.</returns>
    public Card SubstractCardFromList(Card card, List<Card> list)
    {
      Card result = list.Find(c => (c.UniversalId == card.UniversalId));
      if (result != null)
      {
        result.Quantity--;
        if (result.Quantity == 0)
        {
          list.Remove(result);
          result = null;
        }
      }
      return result;
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
          (deck1.LastModifiedDate.CompareTo(deck2.LastModifiedDate) != 0))
        return false;

      return AreEqual(deck1.Sideboard, deck2.Sideboard) && AreEqual(deck1.Cards, deck2.Cards);
    }

    private static bool AreEqual(List<Card> cardList1, List<Card> cardList2)
    {
      if (cardList1.Count != cardList2.Count)
        return false;
      var i = 0;
      while(i < cardList1.Count)
      {
        if (!Card.AreEqual(cardList1[i], cardList2[i]) && // quick test if both card lists are in the same order
          (cardList2.Find(c => Card.AreEqual(c, cardList1[i])) == null))
          return false;
      }
      return true;
    }
  }

}
