// AGoTDB - A card searcher and deck builder tool for the CCG "A Game of Thrones"
// Copyright © 2007, 2008, 2009, 2010, 2011 Vincent Ripoll
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

using AGoTDB.BusinessObjects;
using GenericDB.Components;

namespace AGoTDB.Components
{
	public class AgotCardTreeView : CardTreeView<AgotCardList, AgotCard>
	{
		/// <summary>
		/// Deck partially represented by this treeview (contains global informations such as the house,
		/// used to highlight out of House cards)
		/// </summary>
		public AgotDeck Deck { get; set; }

		/// <summary>
		/// Keeps the count of calls to BeginUpdate and EndUpdate to be used by the IsBeingUpdated method.
		/// </summary>
		protected int updateCount = 0;

		/// <summary>
		/// Indicates whether the control is being updated or not. When it is, drawing must be skipped.
		/// </summary>
		/// <returns>True if the control is being updated, false otherwise.</returns>
		public bool IsBeingUpdated()
		{
			return updateCount > 0;
		}

		public new void BeginUpdate()
		{
			++updateCount;
			base.BeginUpdate();
		}

		public new void EndUpdate()
		{
			--updateCount;
			base.EndUpdate();
		}
	}
}