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

namespace GenericDB.DataAccess
{
	/// <summary>
	/// Link between the GUI filters and the matching database request parameters.
	/// </summary>
	public class DbFilter
	{
		/// <summary>
		/// Value used for the display
		/// </summary>
		public string LongName { get; private set; }

		/// <summary>
		/// Value that is truly used in the database
		/// </summary>
		public string ShortName { get; private set; }

		public string Column { get; private set; }

		public DbFilter(string longName, string column)
		{
			LongName = longName;
			ShortName = longName;
			Column = column;
		}

		public DbFilter(string longName, string column, string shortName)
		{
			LongName = longName;
			Column = column;
			ShortName = shortName;
		}

		public override string ToString()
		{
			return LongName;
		}
	}
}