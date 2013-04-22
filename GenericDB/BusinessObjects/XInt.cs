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
using System.Globalization;

namespace GenericDB.BusinessObjects
{
	///<summary>
	/// Represents an positive integer that may be variable ('X').
	///</summary>
	public class XInt : IComparable<XInt>
	{
		public int Value { get; private set; }
		public bool IsX { get; private set; }
        public bool IsSet { get; set; }

		/// <summary>
		/// Initializes a new instance of XInt, representing an integer value. 
		/// </summary>
		/// <param name="value">The value represented.</param>
		public XInt(int value)
		{
			Value = value;
			IsX = false;
		}

		/// <summary>
		/// Initializes a new instance of XInt, representing a variable value ('X'). 
		/// </summary>
		public XInt()
		{
			IsX = true;
		}

		/// <summary>
		/// Indicates whether the value represented is equal to zero (False) or not (True).
		/// X values are not equal to 0.
		/// </summary>
		/// <returns>True if the value is X or different from 0, False otherwise.</returns>
		public bool IsNonzero()
		{
			return (IsX || (Value > 0));
		}

		public int CompareTo(XInt other)
		{
			if (IsX)
				return (other.IsX) ? 0 : +1;
			return (other.IsX) ? -1 : Value.CompareTo(other.Value);
		}

		public override string ToString()
		{
			return (IsX) ? "X" : Value.ToString(CultureInfo.CurrentCulture);
		}


	}
}