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
using AGoT.AGoTDB.BusinessObjects;

namespace AGoT.AGoTDB
{
  ///<summary>
  /// Represents a value and a list of format sections.
  ///</summary>
  ///<typeparam name="T">The type of the value.</typeparam>
  public class FormattedValue<T>
  {
    public T Value;
    public List<FormatSection> Formats;

    public FormattedValue(T aValue, List<FormatSection> aFormats)
    {
      Value = aValue;
      Formats = aFormats;
    }

    /// Styles are: errata (in human mode: {errata}), trait (~trait~)
    /// Style is encoded as follows: style1, start1-stop1; ... ;styleN, startN-stopN;
    /// For non-text column, start and stop are ignored. The style is applied to the whole field
    public String FormatsToString()
    {
      string result = "";
      for (int i = 0; i < Formats.Count; ++i)
        result += Formats[i] + "; ";
      if (result != "")
        result = result.Substring(0, result.Length - 2);
      return result;
    }
  }
}