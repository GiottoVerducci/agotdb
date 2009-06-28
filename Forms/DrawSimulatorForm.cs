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

namespace AGoT.AGoTDB.Forms
{
  /// <summary>
  /// Form used to perform draws from a deck.
  /// </summary>
  public partial class DrawSimulatorForm : Form
  {
    private static readonly Object SingletonLock = new Object();
    private static DrawSimulatorForm fSingleton;
    private Deck fDeck;
    private const int HandSize = 7;

    /// <summary>
    /// Gets the unique shared singleton instance of this class.
    /// </summary>
    public static DrawSimulatorForm Singleton
    {
      get
      {
        lock (SingletonLock)
        {
          if (fSingleton == null)
            fSingleton = new DrawSimulatorForm();
          return fSingleton;
        }
      }
    }

    /// <summary>
    /// Default form constructor.
    /// </summary>
    public DrawSimulatorForm()
    {
      InitializeComponent();
    }

    /// <summary>
    /// Sets the deck used for the draws.
    /// </summary>
    /// <param name="deck">The deck to use.</param>
    public void SetDeck(Deck deck)
    {
      fDeck = deck;
      InitializeFromDeck();
    }

    private void InitializeFromDeck()
    {
      ListsBeginUpdate(lbDeck, lbHand);
      ClearLists();
      ShuffleToDeckList(lbDeck, fDeck.CardLists[1]); // TODO : modify to handle multiple deck lists
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

    public static void ShuffleToDeckList(ListBox list, CardList cardList)
    {
      list.BeginUpdate();
      var alea = new Random();
      for (var i = 0; i < cardList.Count; ++i)
        if (cardList[i].IsDrawable())
          for (var j = 0; j < cardList[i].Quantity; ++j)
            list.Items.Insert(alea.Next(list.Items.Count + 1), cardList[i]);
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
      lock (SingletonLock)
      {
        fSingleton = null;
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
      var temp = new List<object>();
      for (var i = 0; i < lbDeck.Items.Count; ++i)
        temp.Add(lbDeck.Items[i]);
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