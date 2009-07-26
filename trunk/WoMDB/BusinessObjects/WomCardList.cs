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
	public class WomCardList : CardList<WomCard>
	{
		#region Constructors and clone
		public WomCardList()
		{
		}

		protected WomCardList(WomCardList other)
		{
			CopyCardList(other);
		}

		public override ICardList<WomCard> Clone()
		{
			return new WomCardList(this);
		}
		#endregion
	}
}
