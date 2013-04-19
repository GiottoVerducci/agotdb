// NRADB - A card searcher and deck builder tool for the CCG "A Game of Thrones"
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

namespace NRADB.BusinessObjects
{
	public class NraDeck : Deck<NraCardList, NraCard>
	{
		/// <summary>
		/// The factions chosen for this deck.
		/// </summary>
		public int Factions { get; set; }
		/// <summary>
		/// The agenda chosen for this deck (if any).
		/// </summary>
		public CardList<NraCard> Agenda { get; set; }

		#region Constructors and clone
		public NraDeck()
		{
			Agenda = new NraCardList();
		}

		protected NraDeck(NraDeck originalDeck)
			: base(originalDeck)
		{
			Factions = originalDeck.Factions;
			Agenda = new NraCardList();
			Agenda.AddRange(originalDeck.Agenda);
		}

		public override IDeck<NraCardList, NraCard> Clone()
		{
			return new NraDeck(this);
		}
		#endregion

		#region Implementation of IXmlizable
		protected override void WriteXmlElements(XmlDocument doc, XmlElement deckRoot)
		{
			base.WriteXmlElements(doc, deckRoot);

			XmlToolbox.AddElementValue(doc, deckRoot, "Factions", Factions.ToString());
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
			if (!string.IsNullOrEmpty(value = XmlToolbox.GetElementValue(doc, root, "Factions")))
				Factions = Convert.ToInt32(value, CultureInfo.InvariantCulture);
			// we read the agenda cards
			Agenda.Clear();
			XmlNode agendaRoot;
			var j = 0;
			while (null != (agendaRoot = XmlToolbox.FindNode(root, GetAgendaListNodeName(j))))
			{
				var agendaCard = new NraCard();
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
			var deck = obj as NraDeck;
			if (deck == null)
			{
				return false;
			}

			return (Factions == deck.Factions) && StaticComparer.AreEqual(Agenda, deck.Agenda) &&
				base.Equals(obj);
		}

		public override int GetHashCode()
		{
			return base.GetHashCode();
		}
		#endregion

		public override IDeck<NraCardList, NraCard> CreateRevision()
		{
			var result = new NraDeck();
			result.CardLists.Clear();
			for (var i = 0; i < CardLists.Count; ++i)
				result.CardLists.Add((NraCardList)CardLists[i].Clone());
			result.Factions = Factions;
			result.Agenda = Agenda;
			return result;
		}
	}
}