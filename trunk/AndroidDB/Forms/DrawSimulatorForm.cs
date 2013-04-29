// AndroidDB - A card searcher and deck builder tool for the LCG "Netrunner Android"
// Copyright � 2013 Vincent Ripoll
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
// � Fantasy Flight Games 2012


using System;
using System.Linq;
using System.Windows.Forms;
using NRADB.BusinessObjects;

namespace NRADB.Forms
{
	/// <summary>
	/// Form used to perform draws from a deck.
	/// </summary>
	public partial class DrawSimulatorForm : Form
	{
		private static readonly Object _singletonLock = new Object();
		private static DrawSimulatorForm _singleton;
		private NraDeck _deck;
		private const int HandSize = 7;

		/// <summary>
		/// Gets the unique shared singleton instance of this class.
		/// </summary>
		public static DrawSimulatorForm Singleton
		{
			get
			{
				lock (_singletonLock)
				{
					return _singleton ?? (_singleton = new DrawSimulatorForm());
				}
			}
		}

		/// <summary>
		/// Default form constructor.
		/// </summary>
		public DrawSimulatorForm()
		{
			InitializeComponent();
			lbHand.DisplayMember = "Description";
		}

		/// <summary>
		/// Sets the deck used for the draws.
		/// </summary>
		/// <param name="deck">The deck to use.</param>
		public void SetDeck(NraDeck deck)
		{
			_deck = deck;
			InitializeFromDeck();
		}

		private void InitializeFromDeck()
		{
			ListsBeginUpdate(lbDeck, lbHand);
			ClearLists();
			ShuffleToDeckList(lbDeck, _deck.CardLists[1]); // TODO : modify to handle multiple deck lists
			for (var i = 0; i < HandSize; ++i)
				MoveFromOneListToAnother(lbDeck, 0, lbHand);
			ListsEndUpdate(lbDeck, lbHand);
		}

		/// <summary>
		/// Clears the display lists.
		/// </summary>
		public void ClearLists()
		{
			lbDeck.Items.Clear();
			lbHand.Items.Clear();
		}

		public static void ShuffleToDeckList(ListBox list, NraCardList cardList)
		{
			list.BeginUpdate();
			var alea = new Random();
			foreach (NraCard c in cardList.Where(c => c.IsDrawable()))
			{
				for (var j = 0; j < c.Quantity; ++j)
					list.Items.Insert(alea.Next(list.Items.Count + 1), c);
			}
			list.EndUpdate();
		}

		public static void MoveFromOneListToAnother(ListBox sourceList, int index, ListBox destinationList)
		{
			if (sourceList.Items.Count <= 0)
				return;
			ListsBeginUpdate(sourceList, destinationList);
			destinationList.Items.Add(sourceList.Items[index]);
			sourceList.Items.RemoveAt(index);
			ListsEndUpdate(sourceList, destinationList);
		}

		private static void ListsBeginUpdate(params ListBox[] lists)
		{
			foreach (ListBox list in lists)
				list.BeginUpdate();
		}

		private static void ListsEndUpdate(params ListBox[] lists)
		{
			foreach (ListBox list in lists)
				list.EndUpdate();
		}

		private void DrawSimulatorForm_FormClosed(object sender, FormClosedEventArgs e)
		{
			lock (_singletonLock)
			{
				_singleton = null;
			}
		}

		private void btnMoveRight_Click(object sender, EventArgs e)
		{
			ListBox.SelectedIndexCollection indices = lbHand.SelectedIndices; // given by ascending order
			for (int i = indices.Count - 1; i >= 0; --i)
				MoveFromOneListToAnother(lbHand, indices[i], lbDeck);
		}


		private void btnMoveLeft_Click(object sender, EventArgs e)
		{
			ListBox.SelectedIndexCollection indices = lbDeck.SelectedIndices; // given by ascending order
			for (int i = indices.Count - 1; i >= 0; --i)
				MoveFromOneListToAnother(lbDeck, indices[i], lbHand);
		}

		private void btnDiscard_Click(object sender, EventArgs e)
		{
			ListBox.SelectedIndexCollection indices = lbHand.SelectedIndices; // given by ascending order
			for (int i = indices.Count - 1; i >= 0; --i)
				lbHand.Items.RemoveAt(indices[i]);
		}

		private void btnShuffle_Click(object sender, EventArgs e)
		{
			var temp = lbDeck.Items.Cast<object>().ToList();
			lbDeck.BeginUpdate();
			lbDeck.Items.Clear();
			var alea = new Random();
			for (var i = 0; i < temp.Count; ++i)
				lbDeck.Items.Insert(alea.Next(i + 1), temp[i]);
			lbDeck.EndUpdate();
		}

		private void btnRestart_Click(object sender, EventArgs e)
		{
			InitializeFromDeck();
		}

		private void btnClose_Click(object sender, EventArgs e)
		{
			Close();
		}
	}
}