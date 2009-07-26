// GenericDB - A generic card searcher and deck builder library for CCGs
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
// along with this program. If not, see <http://www.gnu.org/licenses/>.
// You can contact me at v.ripoll@gmail.com

using System;
using GenericDB.BusinessObjects;

namespace GenericDB.BusinessObjects
{
	/// <summary>
	/// Represents a pair of section of text and of text format. 
	/// Section is defined by a begin and end index. 
	/// Format is defined by a TextFormat property.
	/// <example>A section from index 12 to 24 in red and bold.</example>
	/// <remarks>The string itself is not stored in this structure.</remarks>
	/// </summary>
	public struct FormatSection : IComparable<FormatSection>
	{
		/// <summary>
		/// The index of the first (included) character of the section.
		/// </summary>
		public int Begin { get; private set; }
		/// <summary>
		/// The index of the last (excluded) character of the section, that is
		/// the index of the character that follows the last included character of the section.
		/// </summary>
		public int End { get; private set; }
		/// <summary>
		/// The text format of the section.
		/// </summary>
		public TextFormat Format { get; private set; }

		public FormatSection(int begin, int end, TextFormat format)
			: this()
		{
			Begin = begin;
			End = end;
			Format = format;
		}

		public int CompareTo(FormatSection other)
		{
			return Tools.MultipleCompare(
				Begin.CompareTo(other.Begin),
				End.CompareTo(other.End));
		}

		/// Style is encoded as follows: style, start-stop
		/// For non-text column, start and stop are ignored. The style is applied to the whole field
		public override String ToString()
		{
			return String.Format("{0}, {1}-{2}", Format.Name, Begin, End);
		}
	}
}