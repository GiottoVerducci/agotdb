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


using GenericDB.BusinessObjects;

namespace NRADB.BusinessObjects
{
	public class NraCardList : CardList<NraCard>
	{
		#region Constructors and clone
		public NraCardList()
		{
		}

		protected NraCardList(NraCardList other)
		{
			CopyCardList(other);
		}

		public override ICardList<NraCard> Clone()
		{
			return new NraCardList(this);
		}
		#endregion
	}
}