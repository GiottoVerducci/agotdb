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
    private readonly List<Card> fCards;
    private readonly List<Card> fSideboard;
    private Int32 fHouses;
    private Card fAgenda;
    private String fRevisionComments;
    private DateTime fCreationDate, fLastModifiedDate;
    private bool fEditable;

    #region Getter and setter

    public List<Card> Cards
    {
      get { return fCards; }
    }

    public List<Card> Sideboard
    {
      get { return fSideboard; }
    }

    public Int32 Houses
    {
      get { return fHouses; }
      set { fHouses = value; }
    }

    public Card Agenda
    {
      get { return fAgenda; }
      set { fAgenda = value; }
    }

    public String RevisionComments
    {
      get { return fRevisionComments; }
      set { fRevisionComments = value; }
    }

    public DateTime CreationDate
    {
      get { return fCreationDate; }
    }

    public DateTime LastModifiedDate
    {
      get { return fLastModifiedDate; }
    }

    public bool Editable
    {
      get { return fEditable; }
      set { fEditable = value; }
    }

    #endregion

    public Deck()
    {
      fCards = new List<Card>();
      fSideboard = new List<Card>();
      fAgenda = null;
      fRevisionComments = "";
      fCreationDate = DateTime.Now;
      fLastModifiedDate = DateTime.Now;
      fEditable = true;
    }

    /// <summary>
    /// This constructor is used to create a deep copy (clone)
    /// </summary>
    /// <param name="aDeck">the cloned deck </param>
    public Deck(Deck aDeck) : this()
    {
      CopyCardList(aDeck.Cards, fCards);
      CopyCardList(aDeck.Sideboard, fSideboard);
      fHouses = aDeck.Houses;
      fAgenda = aDeck.Agenda;
      fRevisionComments = aDeck.fRevisionComments;
      fCreationDate = aDeck.fCreationDate;
      fLastModifiedDate = aDeck.fLastModifiedDate;
      fEditable = aDeck.fEditable;
    }

    /// <summary>
    /// Creates a new revision of the deck.
    /// </summary>
    /// <param name="aDeck">the previous revision of the deck </param>
    public static Deck CreateRevision(Deck aDeck)
    {
      Deck result = new Deck();
      CopyCardList(aDeck.Cards, result.fCards);
      CopyCardList(aDeck.Sideboard, result.fSideboard);
      result.fHouses = aDeck.Houses;
      result.fAgenda = aDeck.Agenda;
      return result;
    }

    private static void CopyCardList(List<Card> source, List<Card> dest)
    {
      for (int i = 0; i < source.Count; ++i)
        dest.Add(new Card(source[i]));
    }

    /// <summary>
    /// Serializes the deck under an xml data format.
    /// </summary>
    /// <param name="doc">The xml document where the data will be stored</param>
    public XmlElement ToXML(XmlDocument doc)
    {
      XmlElement deckRoot = doc.CreateElement("Deck");

      XmlToolBox.AddElementValue(doc, deckRoot, "RevisionComments", fRevisionComments);
      XmlToolBox.AddElementValue(doc, deckRoot, "CreationDate", fCreationDate.ToBinary().ToString());
      XmlToolBox.AddElementValue(doc, deckRoot, "LastModifiedDate", fLastModifiedDate.ToBinary().ToString());
      XmlToolBox.AddElementValue(doc, deckRoot, "Houses", Houses.ToString());
      if (Agenda != null)
      {
        XmlElement agendaElement = doc.CreateElement("Agenda");
        agendaElement.AppendChild(Agenda.ToXml(doc));
        deckRoot.AppendChild(agendaElement);
      }
      XmlElement cardsElement = doc.CreateElement("Cards");
      for (int i = 0; i < fCards.Count; ++i)
        cardsElement.AppendChild(fCards[i].ToXml(doc));
      deckRoot.AppendChild(cardsElement);
      XmlElement sideboardElement = doc.CreateElement("Sideboard");
      for (int i = 0; i < fSideboard.Count; ++i)
        cardsElement.AppendChild(fSideboard[i].ToXml(doc));
      deckRoot.AppendChild(sideboardElement);
      return deckRoot;
    }

    /// <summary>
    /// Initializes the deck from XML data. The data must have been generated
    /// using the ToXML method.
    /// </summary>
    /// <param name="doc">The xml document where the data is stored</param>
    /// <param name="deckRoot">The xml root node that was returned by a call to ToXML</param>
    public Deck(XmlDocument doc, XmlNode deckRoot) : this()
    {
      if (deckRoot.Name != "Deck")
        throw new Exception("Invalid deck root node");
      fRevisionComments = XmlToolBox.GetElementValue(doc, deckRoot, "RevisionComments");

      String value;
      if ((value = XmlToolBox.GetElementValue(doc, deckRoot, "CreationDate")) != null)
        fCreationDate = DateTime.FromBinary(Int64.Parse(value));
      if ((value = XmlToolBox.GetElementValue(doc, deckRoot, "LastModifiedDate")) != null)
        fLastModifiedDate = DateTime.FromBinary(Int64.Parse(value));
      if (((value = XmlToolBox.GetElementValue(doc, deckRoot, "Houses")) != null) && (value != ""))
        fHouses = Int32.Parse(value);
      XmlNode agendaNode = XmlToolBox.FindNode(doc, deckRoot, "Agenda");
      fAgenda = (agendaNode != null) ? new Card(doc, agendaNode.FirstChild) : null;
      XmlNode cardsRoot = XmlToolBox.FindNode(doc, deckRoot, "Cards");
      if (cardsRoot != null)
      {
        foreach(XmlNode cardNode in cardsRoot.ChildNodes)
          fCards.Add(new Card(doc, cardNode));
      }
      XmlNode sideboardRoot = XmlToolBox.FindNode(doc, deckRoot, "Sideboard");
      if (sideboardRoot != null)
      {
        foreach (XmlNode sidecardNode in sideboardRoot.ChildNodes)
          fSideboard.Add(new Card(doc, sidecardNode));
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
      return AddCardToList(card, fCards);
    }

    /// <summary>
    /// Substracts a card from the deck by decreasing the quantity by 1.
    /// If the quantity reaches 0, removes the card from the list.
    /// </summary>
    /// <param name="card">The card to substract from the deck</param>
    /// <returns>The card substracted, or null if the card was not found or the last copy was removed.</returns>
    public Card SubstractCard(Card card)
    {
      return SubstractCardFromList(card, fCards);
    }

    /// <summary>
    /// Adds a card to the sideboard. If the card is already present, increment
    /// the quantity by 1.
    /// </summary>
    /// <param name="card">The card to add to the sideboard</param>
    /// <returns>The card added or modified</returns>
    public Card AddCardToSideboard(Card card)
    {
      return AddCardToList(card, fSideboard);
    }

    /// <summary>
    /// Substracts a card from the sideboard by decreasing the quantity by 1.
    /// If the quantity reaches 0, removes the card from the list.
    /// </summary>
    /// <param name="card">The card to substract from the sideboard</param>
    /// <returns>The card substracted, or null if the card was not found or the last copy was removed.</returns>
    public Card SubstractCardFromSideboard(Card card)
    {
      return SubstractCardFromList(card, fSideboard);
    }

    /// <summary>
    /// Adds a card to a list of cards. If the card is already present, increment
    /// the quantity by 1.
    /// </summary>
    /// <param name="card">The card to add to the list</param>
    /// <returns>The card added or modified</returns>
    private Card AddCardToList(Card card, List<Card> list)
    {
      Card c = list.Find(delegate(Card aCard) { return (aCard.UniversalId == card.UniversalId); });
      if (c != null)
        c.Quantity++;
      else
      {
        c = new Card(card);
        c.Quantity = 1;
        list.Add(c);
      }
      return c;
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
      Card c = list.Find(delegate(Card aCard) { return (aCard.UniversalId == card.UniversalId); });
      if (c != null)
      {
        c.Quantity--;
        if (c.Quantity == 0)
        {
          list.Remove(c);
          c = null;
        }
      }
      return c;
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
      bool result = (deck1.fHouses == deck2.fHouses) &&
                    (Card.AreEqual(deck1.fAgenda, deck2.fAgenda)) &&
                    (deck1.fRevisionComments == deck2.fRevisionComments) &&
                    (deck1.fCreationDate.CompareTo(deck2.fCreationDate) == 0) &&
                    (deck1.fLastModifiedDate.CompareTo(deck2.fLastModifiedDate) == 0) &&
                    (deck1.fCards.Count == deck2.fCards.Count) &&
                    (deck1.fSideboard.Count == deck2.fSideboard.Count);
      int i = 0;
      while (result && (i < deck1.fCards.Count))
      {
        result &= Card.AreEqual(deck1.fCards[i], deck2.fCards[i]);
        ++i;
      }

      i = 0;
      while (result && (i < deck1.fSideboard.Count))
      {
        result &= Card.AreEqual(deck1.fSideboard[i], deck2.fSideboard[i]);
        ++i;
      }

      return result;
    }
  }
}