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
	/// <summary>
	/// Represents a pair of string and TextFormat.
	/// <example>A "Hello world" string in bold and red.</example>
	/// </summary>
	public struct FormattedText
	{
		public string Text { get; private set; }
		public TextFormat Format { get; private set; }

		public FormattedText(string text, TextFormat format)
			: this()
		{
			Text = text;
			Format = format;
		}

		public FormattedText(string text)
			: this()
		{
			Text = text;
			Format = TextFormat.Regular;
		}
	}
}