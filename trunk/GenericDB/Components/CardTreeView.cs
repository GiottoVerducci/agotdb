// GenericDB - A generic card searcher and deck builder library for CCGs
// Copyright © 2007, 2008, 2009, 2010, 2011, 2012, 2013, 2014 Vincent Ripoll
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

using System.Windows.Forms;
using GenericDB.BusinessObjects;

namespace GenericDB.Components
{
	/// <summary>
	/// A tree view with additional business informations.
	/// </summary>
	public class CardTreeView<TCL, TC> : TreeView
		where TC : class, ICard, new()
		where TCL : class, ICardList<TC>, new()
	{
		/// <summary>
		/// Node from the treeview containing the hint about how to add cards.
		/// </summary>
		public TreeNode NodeInfo { get; set; }

		/// <summary>
		/// Cards displayed by this treeview.
		/// </summary>
		public TCL Cards { get; set; }

		public CardTreeView()
			:base()
		{
			SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
		}
	}
}