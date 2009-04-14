// AGoTDB - A card searcher and deck builder tool for the CCG "A Game of Thrones"
// Copyright © 2007, 2008, 2009 Vincent Ripoll
// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.

// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.

// You should have received a copy of the GNU General Public License
// along with this program.  If not, see <http://www.gnu.org/licenses/>.
// You can contact me at v.ripoll@gmail.com
// © A Game of Thrones 2005 George R. R. Martin
// © A Game of Thrones CCG 2005 Fantasy Flight Games Inc.
// © Le Trône de Fer JCC 2005-2007 Stratagèmes éditions / Xénomorphe Sàrl

using System.Collections.Generic;
using System.Windows.Forms;
using AGoT.AGoTDB.BusinessObjects;

namespace AGoT.AGoTDB.Components
{
  /// <summary>
  /// A tree view with additional business informations.
  /// </summary>
  public class CardTreeView : TreeView
  {
    /// <summary>
    /// Node from the treeview containing the hint about how to add cards.
    /// </summary>
    public TreeNode NodeInfo { get; set; }

    /// <summary>
    /// Cards displayed by this treeview.
    /// </summary>
    public List<Card> Cards { get; set; }
  }
}