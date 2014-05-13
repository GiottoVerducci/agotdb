// GenericDB - A generic card searcher and deck builder library for CCGs
// Copyright © 2007, 2008, 2009, 2010, 2011, 2012, 2013, 2014 Vincent Ripoll
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

namespace GenericDB.BusinessObjects
{
	/// <summary>
	/// Represents a pair of strings.
	/// </summary>
	public class StringPair
	{
		public string Value1 { get; set; }
		public string Value2 { get; set; }

		public StringPair()
		{
			Value1 = "";
			Value2 = "";
		}

		public StringPair(string value1, string value2)
		{
			Value1 = value1;
			Value2 = value2;
		}

		public static StringPair Add(StringPair first, StringPair second)
		{
			return new StringPair(first.Value1 + second.Value1, first.Value2 + second.Value2);
		}

		public static StringPair operator +(StringPair first, StringPair second)
		{
			return Add(first, second);
		}
	}
}