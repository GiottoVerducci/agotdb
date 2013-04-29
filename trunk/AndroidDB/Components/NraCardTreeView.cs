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


using NRADB.BusinessObjects;
using GenericDB.Components;

namespace NRADB.Components
{
    public class NraCardTreeView : CardTreeView<NraCardList, NraCard>
    {
        /// <summary>
        /// Deck partially represented by this treeview (contains global informations such as the faction,
        /// used to highlight out of factions cards)
        /// </summary>
        public NraDeck Deck { get; set; }

        /// <summary>
        /// Keeps the count of calls to BeginUpdate and EndUpdate to be used by the IsBeingUpdated method.
        /// </summary>
        protected int _updateCount;

        /// <summary>
        /// Indicates whether the control is being updated or not. When it is, drawing must be skipped.
        /// </summary>
        /// <returns>True if the control is being updated, false otherwise.</returns>
        public bool IsBeingUpdated()
        {
            return _updateCount > 0;
        }

        public new void BeginUpdate()
        {
            ++_updateCount;
            base.BeginUpdate();
        }

        public new void EndUpdate()
        {
            base.EndUpdate();
            --_updateCount;
        }
    }
}