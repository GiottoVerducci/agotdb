// AndroidDB - A card searcher and deck builder tool for the LCG "Netrunner Android"
// Copyright © 2013 Vincent Ripoll
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
// © Fantasy Flight Games 2012


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

        public NraCard.CardSide Side { get; set; }

        ///// <summary>
        ///// The agenda chosen for this deck (if any).
        ///// </summary>
        //public CardList<NraCard> Agenda { get; set; }

		#region Constructors and clone
		public NraDeck()
		{
            Side = NraCard.CardSide.Runner; // default (to avoid detecting a false change when closing a new deck)
            //Agenda = new NraCardList();
		}

		protected NraDeck(NraDeck originalDeck)
			: base(originalDeck)
		{
            Side = originalDeck.Side;
			Factions = originalDeck.Factions;
            //Agenda = new NraCardList();
            //Agenda.AddRange(originalDeck.Agenda);
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

            XmlToolbox.AddElementValue(doc, deckRoot, "Side", Side.ToString());
            XmlToolbox.AddElementValue(doc, deckRoot, "Factions", Factions.ToString());
            //for (var j = 0; j < Agenda.Count; ++j)
            //{
            //    string cardListNodeName = GetAgendaListNodeName(j);
            //    XmlElement agendaElement = doc.CreateElement(cardListNodeName);
            //    agendaElement.AppendChild(Agenda[j].ToXml(doc));
            //    deckRoot.AppendChild(agendaElement);
            //}
		}

        ///// <summary>
        ///// Returns the node name for xml storage associated to the index in the list of Agenda elements.
        ///// </summary>
        ///// <param name="agendaListIndex">The index.</param>
        ///// <returns>The node name.</returns>
        //protected static string GetAgendaListNodeName(int agendaListIndex)
        //{
        //    return agendaListIndex == 0
        //        ? "Agenda"
        //        : String.Format(CultureInfo.CurrentCulture, "Agenda{0}", agendaListIndex);
        //}

		protected override void ReadXmlElements(XmlNode root, XmlDocument doc)
		{
			base.ReadXmlElements(root, doc);
			string value;
            if (!string.IsNullOrEmpty(value = XmlToolbox.GetElementValue(doc, root, "Side")))
                Side = (NraCard.CardSide)Enum.Parse(typeof(NraCard.CardSide), value);
			if (!string.IsNullOrEmpty(value = XmlToolbox.GetElementValue(doc, root, "Factions")))
				Factions = Convert.ToInt32(value, CultureInfo.InvariantCulture);
            // we read the agenda cards
            //Agenda.Clear();
            //XmlNode agendaRoot;
            //var j = 0;
            //while (null != (agendaRoot = XmlToolbox.FindNode(root, GetAgendaListNodeName(j))))
            //{
            //    var agendaCard = new NraCard();
            //    agendaCard.InitializeFromXml(doc, agendaRoot.FirstChild);
            //    Agenda.Add(agendaCard);
            //    ++j;
            //}
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

            return (Side == deck.Side) && (Factions == deck.Factions) && //StaticComparer.AreEqual(Agenda, deck.Agenda) &&
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
            result.Side = Side;
            result.Factions = Factions;
            //result.Agenda = Agenda;
			return result;
		}
	}
}