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
using System.Data.OleDb;

namespace AGoT.AGoTDB.BusinessObjects
{
  public class CommandParameters
  {
    private readonly List<KeyValuePair<string, object>> fParameters = new List<KeyValuePair<string, object>>();

    public CommandParameters Add(string parameterName, object parameterValue)
    {
      parameterName = parameterName.Trim();
      if(String.IsNullOrEmpty(parameterName))
        throw new ArgumentOutOfRangeException("parameterName");
      if(fParameters.Exists(p => p.Key == parameterName))
        throw new ApplicationException(String.Format("parameterName '{0}' already exists", parameterName));
      fParameters.Add(new KeyValuePair<string, object>(parameterName, parameterValue));
      return this;
    }

    internal void AppendToCommand(OleDbCommand command)
    {
      fParameters.ForEach(p => command.Parameters.AddWithValue(p.Key, p.Value));
    }
  }
}