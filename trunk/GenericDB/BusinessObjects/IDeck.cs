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

using System;
using System.Collections.Generic;
using GenericDB.BusinessObjects;

namespace GenericDB.BusinessObjects
{
	public interface IDeck<TCL, TC> : IXmlizable
		where TCL : class, ICardList<TC>, new()
		where TC : class, ICard, new()
	{
		List<TCL> CardLists { get; }
		String RevisionComments { get; set; }
		DateTime CreationDate { get; }
		DateTime LastModifiedDate { get; }
		bool Editable { get; set; }

		bool Equals(object obj);

		int GetHashCode();

		IDeck<TCL, TC> Clone();
		IDeck<TCL, TC> CreateRevision();
	}
}