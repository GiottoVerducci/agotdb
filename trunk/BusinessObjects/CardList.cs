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

using System.Collections.Generic;
using System.Xml;

namespace AGoT.AGoTDB.BusinessObjects
{
  public class CardList : List<Card>
  {
    public CardList()
    {
    }
    
    public CardList(CardList other)
    {
      CopyCardList(other);
    }

    public CardList(XmlDocument doc, XmlNode cardListRoot)
    {
      if (cardListRoot == null) 
        return;
      foreach (XmlNode cardNode in cardListRoot.ChildNodes)
        Add(new Card(doc, cardNode));
    }

    private void CopyCardList(IList<Card> source)
    {
      Clear();
      for (var i = 0; i < source.Count; ++i)
        Add(new Card(source[i]));
    }

    /// <summary>
    /// Serializes the card list under an xml data format.
    /// </summary>
    /// <param name="doc">The xml document where the data will be stored.</param>
    /// <param name="nodeName">The name of the xml element.</param>
    public XmlElement ToXml(XmlDocument doc, string nodeName)
    {
      XmlElement result = doc.CreateElement(nodeName);
      for (var i = 0; i < Count; ++i)
        result.AppendChild(this[i].ToXml(doc));
      return result;
    }

    public static bool AreEqual(CardList cardList1, CardList cardList2)
    {
      if (cardList1.Count != cardList2.Count)
        return false;
      var i = 0;
      while (i < cardList1.Count)
      {
        int index = i;
        if (!Card.AreEqual(cardList1[i], cardList2[i]) && // quick test if both card lists are in the same order
          (cardList2.Find(c => Card.AreEqual(c, cardList1[index])) == null))
          return false;
        ++i;
      }
      return true;
    }

    /// <summary>
    /// Adds a card to this list of cards. If the card is already present, increments
    /// the quantity by 1.
    /// </summary>
    /// <param name="card">The card to add to the list.</param>
    /// <returns>The card added or modified</returns>
    public Card AddCard(Card card)
    {
      Card result = Find(c => (c.UniversalId == card.UniversalId));
      if (result != null)
        result.Quantity++;
      else
      {
        result = new Card(card) { Quantity = 1 };
        Add(result);
      }
      return result;
    }

    /// <summary>
    /// Substracts a card from this list of cards by decreasing the quantity by 1.
    /// If the quantity reaches 0, removes the card from the list.
    /// </summary>
    /// <param name="card">The card to substract from the list</param>
    /// <returns>The card substracted, or null if the card was not found or the last copy was removed.</returns>
    public Card SubstractCard(Card card)
    {
      Card result = Find(c => (c.UniversalId == card.UniversalId));
      if (result != null)
      {
        result.Quantity--;
        if (result.Quantity == 0)
        {
          Remove(result);
          result = null;
        }
      }
      return result;
    }

  }
}
