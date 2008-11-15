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

using System.Drawing;

namespace AGoT.AGoTDB.BusinessObjects
{
  /// <summary>
  /// Represents a pair of string and TextFormat.
  /// Eg. a "Hello world" string in bold and red.
  /// </summary>
  public struct FormattedText
  {
    public string text;
    public TextFormat format;

    public FormattedText(string aText, TextFormat aFormat)
    {
      text = aText;
      format = aFormat;
    }

    public FormattedText(string aText)
    {
      text = aText;
      format = TextFormat.Regular;
    }
  }

  /// <summary>
  /// Represents the format for some text (= a string).
  /// Exposes a static property Regular for black and regular text.
  /// Eg. Bold and red formatting.
  /// </summary>
  public class TextFormat
  {
    public FontStyle style;
    public Color color;
    public string name;

    public static TextFormat Regular = new TextFormat("regular", FontStyle.Regular, Color.Black);

    public TextFormat(string aName, FontStyle aStyle, Color aColor)
    {
      name = aName;
      style = aStyle;
      color = aColor;
    }
  }
}