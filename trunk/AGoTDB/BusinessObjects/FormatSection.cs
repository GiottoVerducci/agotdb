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

namespace AGoT.AGoTDB.BusinessObjects
{
  /// <summary>
  /// Represents a section of text in a given format. 
  /// Section is defined by a begin and end index. 
  /// Format is defined by a TextFormat property.
  /// <example>A section from index 12 to 24 in red and bold.</example>
  /// <remarks>The string itself is not stored in this structure.</remarks>
  /// </summary>
  public struct FormatSection
  {
    /// <summary>
    /// The index of the first (included) character of the section.
    /// </summary>
    public int Begin { get; private set; }
    /// <summary>
    /// The index of the last (included) character of the section.
    /// </summary>
    public int End { get; private set; }
    /// <summary>
    /// The text format of the section.
    /// </summary>
    public TextFormat Format { get; private set; }

    public FormatSection(int begin, int end, TextFormat format) : this()
    {
      Begin = begin;
      End = end;
      Format = format;
    }

    /// Styles are: errata (in human mode: {errata}), trait (~trait~)
    /// Style is encoded as follows: style, start-stop
    /// For non-text column, start and stop are ignored. The style is applied to the whole field
    public override String ToString()
    {
      return String.Format("{0}, {1}-{2}", Format.Name, Begin, End);
    }
  }
}