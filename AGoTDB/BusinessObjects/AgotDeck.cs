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
// along with this program. If not, see <http://www.gnu.org/licenses/>.
// You can contact me at v.ripoll@gmail.com
// © A Game of Thrones 2005 George R. R. Martin
// © A Game of Thrones CCG 2005 Fantasy Flight Games Inc.
// © Le Trône de Fer JCC 2005-2007 Stratagèmes éditions / Xénomorphe Sàrl

using System;
using System.Globalization;
using System.Xml;
using GenericDB.BusinessObjects;

namespace AGoTDB.BusinessObjects
{
	public class AgotDeck : Deck<AgotCardList, AgotCard>
	{
		/// <summary>
		/// The houses chosen for this deck (more than one for a treaty deck, for example).
		/// </summary>
		public int Houses { get; set; }
		/// <summary>
		/// The agenda chosen for this deck (if any).
		/// </summary>
		public AgotCard Agenda { get; set; }

		#region Constructors and clone
		public AgotDeck()
		{
			Agenda = null;
		}

		protected AgotDeck(AgotDeck originalDeck)
			: base(originalDeck)
		{
			Houses = originalDeck.Houses;
			Agenda = originalDeck.Agenda;
		}

		public override IDeck<AgotCardList, AgotCard> Clone()
		{
			return new AgotDeck(this);
		}
		#endregion

		#region Implementation of IXmlizable
		protected override void WriteXmlElements(XmlDocument doc, XmlElement deckRoot)
		{
			base.WriteXmlElements(doc, deckRoot);

			XmlToolbox.AddElementValue(doc, deckRoot, "Houses", Houses.ToString());
			if (Agenda != null)
			{
				XmlElement agendaElement = doc.CreateElement("Agenda");
				agendaElement.AppendChild(Agenda.ToXml(doc));
				deckRoot.AppendChild(agendaElement);
			}
		}

		protected override void ReadXmlElements(XmlNode root, XmlDocument doc)
		{
			base.ReadXmlElements(root, doc);
			string value;
			if (!string.IsNullOrEmpty(value = XmlToolbox.GetElementValue(doc, root, "Houses")))
				Houses = Convert.ToInt32(value, CultureInfo.InvariantCulture);
			XmlNode agendaNode = XmlToolbox.FindNode(root, "Agenda");
			if (agendaNode != null)
			{
				Agenda = new AgotCard();
				Agenda.InitializeFromXml(doc, agendaNode.FirstChild);
			}
			else Agenda = null;
		}
		#endregion

		#region Equality
		public override bool Equals(object obj)
		{
			// if parameter cannot be cast to this, return false
			var deck = obj as AgotDeck;
			if (deck == null)
			{
				return false;
			}

			return (Houses == deck.Houses) && StaticComparer.AreEqual(Agenda, deck.Agenda) &&
				base.Equals(obj);
		}

		public override int GetHashCode()
		{
			return base.GetHashCode();
		}
		#endregion

		public override IDeck<AgotCardList, AgotCard> CreateRevision()
		{
			var result = new AgotDeck();
			result.CardLists.Clear();
			for (var i = 0; i < CardLists.Count; ++i)
				result.CardLists.Add((AgotCardList)CardLists[i].Clone());
			result.Houses = Houses;
			result.Agenda = Agenda;
			return result;
		}
	}
}