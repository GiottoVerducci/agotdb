// WoMDB - A card searcher and deck builder tool for the CCG "Wizards of Mickey"
// Copyright © 2009 Vincent Ripoll
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
// © Wizards of Mickey CCG ??? ???

using GenericDB.BusinessObjects;

namespace WoMDB.BusinessObjects
{
	public class WomDeck : Deck<WomCardList, WomCard>
	{
		#region Constructors and clone
		public WomDeck()
		{ 
		}

		protected WomDeck(WomDeck originalDeck)
			: base(originalDeck)
		{
		}

		public override IDeck<WomCardList, WomCard> Clone()
		{
			return new WomDeck(this);
		}
		#endregion

		public override IDeck<WomCardList, WomCard> CreateRevision()
		{
			var result = new WomDeck();
			result.CardLists.Clear();
			for (var i = 0; i < CardLists.Count; ++i)
				result.CardLists.Add((WomCardList)CardLists[i].Clone());
			return result;
		}
	}
}
