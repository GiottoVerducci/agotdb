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
    public static Query operator +(Query firstQuery, Query secondQuery)
    {
      if (secondQuery.SqlQuery == "")
        return firstQuery;

      if (firstQuery.SqlQuery == "")
        return secondQuery;

      return new Query(string.Format("{0} AND {1}", firstQuery.SqlQuery, secondQuery.SqlQuery),
                       string.Format("{0} {1} {2}", firstQuery.HumanQuery, Resource1.And, secondQuery.HumanQuery));
    }
  }
}