// AGoTDB - A card searcher and deck builder tool for the CCG "A Game of Thrones"
// Copyright © 2007, 2008, 2009, 2010 Vincent Ripoll
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
// © A Game of Thrones CCG 2005 Fantasy Flight Publishing, Inc.
// © A Game of Thrones LCG 2008 Fantasy Flight Publishing, Inc.
// © Le Trône de Fer JCC 2005-2007 Stratagèmes éditions / Xénomorphe Sàrl
// © Le Trône de Fer JCE 2008 Edge Entertainment

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
		public CardList<AgotCard> Agenda { get; set; }

        public AgotDeckFormat DeckFormat { get; set; }

		#region Constructors and clone
		public AgotDeck()
		{
			Agenda = new AgotCardList();
		}

		protected AgotDeck(AgotDeck originalDeck)
			: base(originalDeck)
		{
			Houses = originalDeck.Houses;
			Agenda = new AgotCardList();
			Agenda.AddRange(originalDeck.Agenda);
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
            XmlToolbox.AddElementValue(doc, deckRoot, "DeckFormat", DeckFormat.ToString());
            for (var j = 0; j < Agenda.Count; ++j)
			{
				string cardListNodeName = GetAgendaListNodeName(j);
				XmlElement agendaElement = doc.CreateElement(cardListNodeName);
				agendaElement.AppendChild(Agenda[j].ToXml(doc));
				deckRoot.AppendChild(agendaElement);
			}
		}

		/// <summary>
		/// Returns the node name for xml storage associated to the index in the list of Agenda elements.
		/// </summary>
		/// <param name="agendaListIndex">The index.</param>
		/// <returns>The node name.</returns>
		protected static string GetAgendaListNodeName(int agendaListIndex)
		{
			return agendaListIndex == 0
				? "Agenda"
				: String.Format(CultureInfo.CurrentCulture, "Agenda{0}", agendaListIndex);
		}


		protected override void ReadXmlElements(XmlNode root, XmlDocument doc)
		{
			base.ReadXmlElements(root, doc);
			string value;
			if (!string.IsNullOrEmpty(value = XmlToolbox.GetElementValue(doc, root, "Houses")))
				Houses = Convert.ToInt32(value, CultureInfo.InvariantCulture);
            if (!string.IsNullOrEmpty(value = XmlToolbox.GetElementValue(doc, root, "DeckFormat")))
                DeckFormat = (AgotDeckFormat)Enum.Parse(typeof(AgotDeckFormat), value);
            // we read the agenda cards
			Agenda.Clear();
			XmlNode agendaRoot;
			var j = 0;
			while (null != (agendaRoot = XmlToolbox.FindNode(root, GetAgendaListNodeName(j))))
			{
				var agendaCard = new AgotCard();
				agendaCard.InitializeFromXml(doc, agendaRoot.FirstChild);
				Agenda.Add(agendaCard);
				++j;
			}
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

    public enum AgotDeckFormat
    {
        Joust,
        Melee
    }
}