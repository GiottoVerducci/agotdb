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
using System.Collections.Generic;

namespace GenericDB.BusinessObjects
{
	///<summary>
	/// Represents a value and a list of format sections.
	///</summary>
	///<typeparam name="T">The type of the value.</typeparam>
	public class FormattedValue<T>
	{
		/// <summary>
		/// The value to which the formatted sections applies.
		/// </summary>
		public T Value { get; private set; }
		/// <summary>
		/// The format sections that apply to the value.
		/// </summary>
		public List<FormatSection> Formats { get; private set; }

		public FormattedValue(T value, List<FormatSection> formats)
		{
			Value = value;
			Formats = formats;
		}

		/// Styles are: errata (in human mode: {errata}), trait (~trait~)
		/// Style is encoded as follows: style1, start1-stop1; ... ;styleN, startN-stopN;
		/// For non-text column, start and stop are ignored. The style is applied to the whole field
		public String FormatsToString()
		{
			return string.Join("; ", Formats.ConvertAll(f => f.ToString()).ToArray());
		}
	}
}