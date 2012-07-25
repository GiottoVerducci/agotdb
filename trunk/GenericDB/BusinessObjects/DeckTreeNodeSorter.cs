// GenericDB - A generic card searcher and deck builder library for CCGs
// Copyright © 2007, 2008, 2009, 2010, 2011, 2012 Vincent Ripoll
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
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace GenericDB.BusinessObjects
{
	public delegate bool IsCardNodeDelegate(TreeNode treeNode);
	public delegate int GetTypeDelegate(TreeNode treeNode);

	/// <summary>
	/// Represents a sorter for deck tree nodes.
	/// </summary>
	public class DeckTreeNodeSorter : IComparer<TreeNode>
	{
		private readonly List<int> _orderedTypes;
		private readonly IsCardNodeDelegate _isCardNodeDelegateMethod;
		private readonly GetTypeDelegate _getTypeDelegateMethod;

		/// <summary>
		/// Initializes a new instance of DeckTreeNodeSorter with a string containing the type values 
		/// ordered and separated by a column.
		/// </summary>
		/// <param name="orderedTypeValues">The ordered type values.</param>
		/// <param name="isCardNodeDelegateMethod">[Delegate] Indicates whether the node is a card node (true) or not (false).</param>
		/// <param name="getTypeDelegateMethod">[Delegate] Gets the type of card represented by the node.</param>
		public DeckTreeNodeSorter(string[] orderedTypeValues,
			IsCardNodeDelegate isCardNodeDelegateMethod,
			GetTypeDelegate getTypeDelegateMethod)
		{
			_orderedTypes = orderedTypeValues.Length > 0
				? orderedTypeValues.Select(type => Convert.ToInt32(type)).ToList()
				: new List<int>();
			_isCardNodeDelegateMethod = isCardNodeDelegateMethod;
			_getTypeDelegateMethod = getTypeDelegateMethod;
		}


		/// <summary>
		/// Compares two TreeNode objects and returns an indication of their relative values.
		/// </summary>
		/// <param name="x">The first TreeNode.</param>
		/// <param name="y">The second TreeNode.</param>
		/// <returns>0 is x and y are equal, -1 if x is lesser than y, 1 if x is greater than y.</returns>
		public int Compare(TreeNode x, TreeNode y)
		{
			//if (DeckBuilderForm.IsCardNode(x))
			if (_isCardNodeDelegateMethod(x))
			{
				var cx = x.Tag as Card;
				var cy = y.Tag as Card;
				return cx.CompareTo(cy);
			}
			//int xType = GetTypeOrder(((DeckBuilderForm.TypeNodeInfo)x.Tag).Type);
			//int yType = GetTypeOrder(((DeckBuilderForm.TypeNodeInfo)y.Tag).Type);
			//return xType.CompareTo(yType);
			int xOrder = GetTypeOrder(_getTypeDelegateMethod(x));
			int yOrder = GetTypeOrder(_getTypeDelegateMethod(y));
			return xOrder.CompareTo(yOrder);
		}

		/// <summary>
		/// Gets the order for a given type.
		/// </summary>
		/// <param name="type">The type for which the order is returned.</param>
		/// <returns>The order of the type.</returns>
		public int GetTypeOrder(Int32 type)
		{
			// search our type in our order list.
			return _orderedTypes.FindLastIndex(t => (t == type));
		}
	}
}