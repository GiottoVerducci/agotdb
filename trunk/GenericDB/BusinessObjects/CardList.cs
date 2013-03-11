// GenericDB - A generic card searcher and deck builder library for CCGs
// Copyright © 2007, 2008, 2009, 2010, 2011, 2012, 2013 Vincent Ripoll
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
using System.Xml;

namespace GenericDB.BusinessObjects
{
	public abstract class CardList<T> : List<T>, ICardList<T>
		where T : class, ICard, new()
	{
		#region Constructors and clone
		public abstract ICardList<T> Clone();
		#endregion

		#region Implementation of IXmlizable
		/// <summary>
		/// Gets the XML representation of this card list.
		/// </summary>
		/// <param name="doc">The XML document for which the XML representation is generated.</param>
		/// <returns>A XML node representing this card.</returns>
		public XmlNode ToXml(XmlDocument doc)
		{
			XmlElement result = doc.CreateElement("CardList");
			for (var i = 0; i < Count; ++i)
				result.AppendChild(this[i].ToXml(doc));
			return result;
		}

		/// <summary>
		/// Initializes the properties of this card list from an XML node that was generated 
		/// using the ToXml method.
		/// </summary>
		/// <param name="doc">The XML document containing the XML node.</param>
		/// <param name="root">The XML node containing the XML data representing the object.</param>
		public void InitializeFromXml(XmlDocument doc, XmlNode root)
		{
			if (root == null) throw new ArgumentNullException("root");

			// tweak for legacy format
			if ((root.FirstChild != null) && (root.FirstChild.Name == "CardList"))
				root = root.FirstChild;
			// end of tweak

			foreach (XmlNode cardNode in root.ChildNodes)
			{
				var card = new T();
				card.InitializeFromXml(doc, cardNode);
				Add(card);
			}
		}
		#endregion

		#region Equality
		public override bool Equals(object obj)
		{
			// if parameter cannot be cast to this, return false
			var cardList = obj as CardList<T>;
			if (cardList == null)
			{
				return false;
			}

			if (Count != cardList.Count)
				return false;
			var i = 0;
			while (i < Count)
			{
				int index = i;
				if (!StaticComparer.AreEqual(this[i], cardList[i]) && // quick test if both card lists are in the same order
					(cardList.Find(c => StaticComparer.AreEqual(c, this[index])) == null))
					return false;
				++i;
			}
			return true;
		}

		public override int GetHashCode()
		{
			return base.GetHashCode();
		}
		#endregion

		protected virtual void CopyCardList(ICardList<T> source)
		{
		    Clear();
		    foreach (T card in source)
		        Add((T)card.Clone());
		}

	    /// <summary>
		/// Adds a card to this list of cards. If the card is already present, increments
		/// the quantity by 1.
		/// </summary>
		/// <param name="card">The card to add to the list.</param>
		/// <returns>The card added or modified</returns>
		public T AddCard(T card)
		{
			var result = Find(c => (c.UniversalId == card.UniversalId));
			if (result != null)
				result.Quantity++;
			else
			{
				result = (T)card.Clone();
				result.Quantity = 1;
				Add(result);
			}
			return result;
		}

		/// <summary>
		/// Subtracts a card from this list of cards by decreasing the quantity by 1.
		/// If the quantity reaches 0, removes the card from the list.
		/// </summary>
		/// <param name="card">The card to subtract from the list</param>
		/// <returns>The card subtracted, or null if the card was not found or the last copy was removed.</returns>
		public T SubtractCard(T card)
		{
			var result = Find(c => (c.UniversalId == card.UniversalId));
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
