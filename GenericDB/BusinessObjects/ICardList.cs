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

using System.Collections.Generic;

namespace GenericDB.BusinessObjects
{
	public interface ICardList<T> : IList<T>, IXmlizable
		where T : class, ICard, new()
	{
		/// <summary>
		/// Creates a new instance of this object by performing a deep copy of its properties.		
		/// </summary>
		/// <returns>A clone object of this object.</returns>
		ICardList<T> Clone();

		bool Equals(object obj);
		int GetHashCode();

		/// <summary>
		/// Adds a card to this list of cards. If the card is already present, increments
		/// its quantity by 1.
		/// </summary>
		/// <param name="card">The card to add to the list.</param>
		/// <returns>The card added or modified.</returns>
		T AddCard(T card);

		/// <summary>
		/// Subtracts a card from this list of cards by decreasing its quantity by 1.
		/// If the quantity reaches 0, removes the card from the list.
		/// </summary>
		/// <param name="card">The card to subtract from the list.</param>
		/// <returns>The card subtracted, or null if the card was not found or the last copy was removed.</returns>
		T SubtractCard(T card);
	}
}