// GenericDB - A generic card searcher and deck builder library for CCGs
// Copyright © 2007, 2008, 2009, 2010, 2011, 2012 Vincent Ripoll
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
	public static class StaticComparer
	{
		/// <summary>
		/// Compares two objects by reference and using the Equals method.
		/// </summary>
		/// <param name="obj1">The first object.</param>
		/// <param name="obj2">The second object.</param>
		/// <returns>True if both are reference to the same object (or null) or if they are equal.</returns>
		public static bool AreEqual(object obj1, object obj2)
		{
			return (obj1 == obj2) || 
				(obj1 != null && obj2 != null && obj1.Equals(obj2));
		}
	}
}
