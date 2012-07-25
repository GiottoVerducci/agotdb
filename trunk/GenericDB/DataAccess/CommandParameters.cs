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

using System;
using System.Collections.Generic;
using System.Data.OleDb;

namespace GenericDB.DataAccess
{
	public class CommandParameters
	{
		private readonly IDictionary<string, object> _parameters = new Dictionary<string, object>();

		public CommandParameters Add(string parameterName, object parameterValue)
		{
			parameterName = parameterName.Trim();
			if (string.IsNullOrEmpty(parameterName))
				throw new ArgumentOutOfRangeException("parameterName");
			if (_parameters.ContainsKey(parameterName))
				throw new ArgumentException(String.Format("parameterName '{0}' already exists", parameterName));
			_parameters.Add(new KeyValuePair<string, object>(parameterName, parameterValue));
			return this;
		}

		internal void AppendToCommand(OleDbCommand command)
		{
			foreach (var value in _parameters)
				command.Parameters.AddWithValue(value.Key, value.Value);
		}
	}
}