// GenericDB - A generic card searcher and deck builder library for CCGs
// Copyright © 2007, 2008, 2009, 2010, 2011 Vincent Ripoll
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

using GenericDB.BusinessObjects;

namespace GenericDB.DataAccess
{
	/// <summary>
	/// Contains a pair of characters (or a single character) that encloses a substring
	/// and associates to it a text format.
	/// <example>
	/// '{' and '}' could be associated to an errata text format (any substring
	/// between those two bounding characters would be associated to this text format).
	/// </example>
	/// <example>
	/// '~' could be associated to a special text format (any substring
	/// between two occurrences of this bounding characters would be associated to this text format).
	/// </example>
	/// </summary>
	public class BoundFormat
	{
		/// <summary>
		/// The opening bound character.
		/// </summary>
		public char Opening { get; private set; }
		/// <summary>
		/// The closing bound character.
		/// </summary>
		public char Closing { get; private set; }
		/// <summary>
		/// Indicates whether the opening and closing bound characters are different (true) or not (false)
		/// </summary>
		public bool DistinctOpenClose { get; private set; }
		/// <summary>
		/// The associated text format.
		/// </summary>
		public TextFormat Format { get; private set; }

		/// <summary>
		/// Creates a new instance of a BoundFormat using a single bound character.
		/// </summary>
		/// <param name="bound">The opening and closing bound character.</param>
		/// <param name="format">The associated text format.</param>
		public BoundFormat(char bound, TextFormat format)
		{
			Opening = bound;
			Closing = bound;
			DistinctOpenClose = false;
			Format = format;
		}

		/// <summary>
		/// Creates a new instance of a BoundFormat using a pair of bound characters.
		/// </summary>
		/// <param name="openingBound">The opening bound character.</param>
		/// <param name="closingBound">The closing bound character.</param>
		/// <param name="format">The associated text format.</param>
		public BoundFormat(char openingBound, char closingBound, TextFormat format)
		{
			Opening = openingBound;
			Closing = closingBound;
			DistinctOpenClose = true;
			Format = format;
		}
	}
}