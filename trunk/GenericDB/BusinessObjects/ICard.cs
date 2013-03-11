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
using System.Collections.Generic;

namespace GenericDB.BusinessObjects
{
	public interface ICard : IXmlizable, IComparable<ICard>
	{
		/// <summary>
		/// The unique identifier for the card.
		/// </summary>
		Int32 UniversalId { get; set; }

		/// <summary>
		/// The number of copies of this card.
		/// </summary>
		int Quantity { get; set; }

		/// <summary>
		/// Creates a new instance of this object by performing a deep copy of its properties.		
		/// </summary>
		/// <returns>A clone object of this object.</returns>
		ICard Clone();

		/// <summary>
		/// Returns the card text to a plain text string.
		/// </summary>
		/// <returns>The card text in plain text.</returns>
		string ToPlainFullString();

		/// <summary>
		/// Returns the set (abbreviated) in which the card first appeared.
		/// </summary>
		/// <returns>The set as a string.</returns>
		string GetShortSet();

		/// <summary>
		/// Returns a string giving a summary of the main characteristics of the card.
		/// </summary>
		/// <returns>The summary.</returns>
		string GetSummaryInfo();

		/// <summary>
		/// Gets a list of ordonnated formatted elements representing this card.
		/// </summary>
		/// <returns>A list of FormattedText elements containing the informations about this card.</returns>
		IList<FormattedText> ToFormattedString();

		/// <summary>
		/// Indicates whether the card could be drawn during a game.
		/// </summary>
		/// <returns>True if the card could be drawn, false otherwise.</returns>
		bool IsDrawable();
	}
}