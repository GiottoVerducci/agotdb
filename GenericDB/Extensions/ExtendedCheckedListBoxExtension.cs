// GenericDB - A generic card searcher and deck builder library for CCGs
// Copyright © 2007, 2008, 2009, 2010, 2011, 2012, 2013 Vincent Ripoll
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
using Beyond.ExtendedControls;

namespace GenericDB.Extensions
{
	public static class ExtendedCheckedListBoxExtension
	{
		/// <summary>
		/// Performs an action on the expanded items of the extended checked list box control
		/// by de-condensing it and putting it back to its original state afterwards.
		/// </summary>
		/// <param name="ecl">The extended checked list box control.</param>
		/// <param name="actionToPerform">The action to perform on the extended checked list box control once it is expanded.</param>
		public static void WorkOnExpandedItems(this ExtendedCheckedListBox ecl, Action<ExtendedCheckedListBox> actionToPerform)
		{
			bool condensed = ecl.Condensed; // we'll use it to restore its previous state
			if (condensed)
				ecl.Condensed = false; // expand ecl to get access to the items
			actionToPerform(ecl);
			if (condensed)
				ecl.Condensed = true; // restore condensed state
		}
	}
}
