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

namespace AGoT.AGoTDB.BusinessObjects
{
  public class Query
  {
    public string SqlQuery { get; set; }
    public string HumanQuery { get; set; }

    public Query()
    {
      SqlQuery = "";
      HumanQuery = "";
    }

    public Query(string sqlQuery, string humanQuery)
    {
      SqlQuery = sqlQuery;
      HumanQuery = humanQuery;
    }

    /// <summary>
    /// Concatenates a query string to another query string by using the keyword "AND".
    /// Queries with an empty SqlQuery property are treated correctly (ie. are ignored).
    /// </summary>
    /// <param name="firstQuery">The first query.</param>
    /// <param name="secondQuery">The second query.</param>
    /// <returns>The concatenated queries.</returns>
    public static Query Add(Query firstQuery, Query secondQuery)
    {
      if (string.IsNullOrEmpty(secondQuery.SqlQuery))
        return firstQuery;

      if (string.IsNullOrEmpty(firstQuery.SqlQuery))
        return secondQuery;

      return new Query(string.Format("{0} AND {1}", firstQuery.SqlQuery, secondQuery.SqlQuery),
                       string.Format("{0} {1} {2}", firstQuery.HumanQuery, Resource1.And, secondQuery.HumanQuery));
    }

    public static Query operator +(Query firstQuery, Query secondQuery)
    {
      return Add(firstQuery, secondQuery);
    }
  }
}