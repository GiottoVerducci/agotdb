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

using System;
using System.Collections.Generic;
using System.Windows.Forms;
using AGoT.AGoTDB.BusinessObjects;
using AGoT.AGoTDB.Forms;

namespace AGoT.AGoTDB.BusinessObjects
{
  /// <summary>
  /// Represents a sorter for deck tree nodes.
  /// </summary>
  public class DeckTreeNodeSorter : IComparer<TreeNode>
  {
    private readonly List<int> orders = new List<int>();

    /// <summary>
    /// Initializes a new instance of DeckTreeNodeSorter, by reading the sort values in the UserSettings singleton.
    /// </summary>
    public DeckTreeNodeSorter()
    {
      int i = 0;
      try
      {
        int type;
        while (-1 != (type = UserSettings.Singleton.ReadInt("DeckBuilderTypeOrder", String.Format("Type{0}", i), -1)))
        {
          orders.Add(type);
          i++;
        }
      }
      catch { }
    }

    /// <summary>
    /// Compares two TreeNode objects and returns an indication of their relative values.
    /// </summary>
    /// <param name="x">The first TreeNode.</param>
    /// <param name="y">The second TreeNode.</param>
    /// <returns>0 is x and y are equal, -1 if x is lesser than b, 1 if a is greater than b.</returns>
    public int Compare(TreeNode x, TreeNode y)
    {
      if (DeckBuilderForm.isCardNode(x))
      {
        Card cx = x.Tag as Card;
        Card cy = y.Tag as Card;
        return cx.CompareOrder(cy);
      }
      int xType = GetTypeOrder(((DeckBuilderForm.TypeNodeInfo) x.Tag).type);
      int yType = GetTypeOrder(((DeckBuilderForm.TypeNodeInfo) y.Tag).type);
      return xType.CompareTo(yType);
    }

    /// <summary>
    /// Gets the order for a given type.
    /// </summary>
    /// <param name="type">The type for which the order is returned.</param>
    /// <returns>The order of the type.</returns>
    public int GetTypeOrder(Int32 type)
    {
      // search our type in our order list.
      return orders.FindLastIndex(delegate(int t) { return (t == type); });
    }
  }
}