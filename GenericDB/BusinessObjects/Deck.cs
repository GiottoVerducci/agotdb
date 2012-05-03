// GenericDB - A generic card searcher and deck builder library for CCGs
// Copyright © 2007, 2008, 2009, 2010, 2011, 2012 Vincent Ripoll
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
	public abstract class Deck<TCL, TC> : IDeck<TCL, TC>
		where TC : class, ICard, new()
		where TCL : class, ICardList<TC>, new()
	{
		public List<TCL> CardLists { get; private set; }
		public string RevisionComments { get; set; }
		public DateTime CreationDate { get; private set; }
		public DateTime LastModifiedDate { get; private set; }
		public bool Editable { get; set; }

		#region Constructors and clone
		protected Deck()
		{
			CardLists = new List<TCL> { new TCL(), new TCL() }; // sideboard (0) and main deck (1)
			RevisionComments = "";
			CreationDate = DateTime.Now;
			LastModifiedDate = DateTime.Now;
			Editable = true;
		}

		/// <summary>
		/// This constructor is used to create a deep copy (clone)
		/// </summary>
		/// <param name="originalDeck">The deck to clone.</param>
		protected Deck(Deck<TCL, TC> originalDeck)
			: this()
		{
			CardLists = new List<TCL>();
			for (var i = 0; i < originalDeck.CardLists.Count; ++i)
				CardLists.Add((TCL)originalDeck.CardLists[i].Clone());
			RevisionComments = originalDeck.RevisionComments;
			CreationDate = originalDeck.CreationDate;
			LastModifiedDate = originalDeck.LastModifiedDate;
			Editable = originalDeck.Editable;
		}

		public abstract IDeck<TCL, TC> Clone();
		#endregion

		#region Implementation of IXmlizable
		/// <summary>
		/// Gets the XML representation of this deck.
		/// </summary>
		/// <param name="doc">The XML document for which the XML representation is generated.</param>
		/// <returns>A XML node representing this card.</returns>
		public XmlNode ToXml(XmlDocument doc)
		{
			XmlElement deckRoot = doc.CreateElement("Deck");

			WriteXmlElements(doc, deckRoot);
			return deckRoot;
		}

		protected virtual void WriteXmlElements(XmlDocument doc, XmlElement deckRoot)
		{
			XmlToolbox.AddElementValue(doc, deckRoot, "RevisionComments", RevisionComments);
			XmlToolbox.AddElementValue(doc, deckRoot, "CreationDate", CreationDate.ToBinary().ToString(CultureInfo.InvariantCulture));
			XmlToolbox.AddElementValue(doc, deckRoot, "LastModifiedDate", LastModifiedDate.ToBinary().ToString(CultureInfo.InvariantCulture));
			for (var j = 0; j < CardLists.Count; ++j)
			{
				string cardListNodeName = GetCardListNodeName(j);
				XmlElement cardListElement = doc.CreateElement(cardListNodeName);
				cardListElement.AppendChild(CardLists[j].ToXml(doc));
				deckRoot.AppendChild(cardListElement);
			}
		}

		/// <summary>
		/// Initializes the properties of this deck from an XML node that was generated 
		/// using the ToXml method.
		/// </summary>
		/// <param name="doc">The XML document containing the XML node.</param>
		/// <param name="root">The XML node containing the XML data representing the object.</param>
		public void InitializeFromXml(XmlDocument doc, XmlNode root)
		{
			if (root == null) throw new ArgumentNullException("root");
			if (root.Name != "Deck") throw new XmlException("Invalid deck root node");

			ReadXmlElements(root, doc);
		}

		protected virtual void ReadXmlElements(XmlNode root, XmlDocument doc)
		{
			RevisionComments = XmlToolbox.GetElementValue(doc, root, "RevisionComments");

			String value;
			if ((value = XmlToolbox.GetElementValue(doc, root, "CreationDate")) != null)
				CreationDate = DateTime.FromBinary(Int64.Parse(value, CultureInfo.InvariantCulture));
			if ((value = XmlToolbox.GetElementValue(doc, root, "LastModifiedDate")) != null)
				LastModifiedDate = DateTime.FromBinary(Int64.Parse(value, CultureInfo.InvariantCulture));
			// we read the cardlists
			CardLists.Clear();
			XmlNode cardsRoot;
			var j = 0;
			while (null != (cardsRoot = XmlToolbox.FindNode(root, GetCardListNodeName(j))))
			{
				//CardLists.Add(new CardList(doc, cardsRoot));
				var cardList = new TCL();
				cardList.InitializeFromXml(doc, cardsRoot);
				CardLists.Add(cardList);
				++j;
			}
		}

		/// <summary>
		/// Returns the node name for xml storage associated to the index in the list of CardList elements.
		/// </summary>
		/// <param name="cardListIndex">The index.</param>
		/// <returns>The node name.</returns>
		protected static string GetCardListNodeName(int cardListIndex)
		{
			return cardListIndex == 0
				? "Sideboard"
				: (cardListIndex == 1 ? "Cards" : String.Format(CultureInfo.CurrentCulture, "Cards{0}", cardListIndex));
		}
		#endregion

		#region Equality
		public override bool Equals(object obj)
		{
			// if parameter cannot be cast to this, return false
			var deck = obj as Deck<TCL, TC>;
			if (deck == null)
			{
				return false;
			}

			if ((RevisionComments != deck.RevisionComments) || (CreationDate.CompareTo(deck.CreationDate) != 0) ||
					(LastModifiedDate.CompareTo(deck.LastModifiedDate) != 0) ||
					(CardLists.Count != deck.CardLists.Count))
				return false;

			for (var i = 0; i < CardLists.Count; ++i)
				if (!StaticComparer.AreEqual(CardLists[i], deck.CardLists[i]))
					return false;
			return true;
		}

		public override int GetHashCode()
		{
			return base.GetHashCode();
		}
		#endregion

		/// <summary>
		/// Creates a new revision of the deck.
		/// </summary>
		public abstract IDeck<TCL, TC> CreateRevision();
	}
}
