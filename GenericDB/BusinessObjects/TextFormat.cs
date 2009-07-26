// GenericDB - A generic card searcher and deck builder library for CCGs
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
// along with this program. If not, see <http://www.gnu.org/licenses/>.
// You can contact me at v.ripoll@gmail.com

using System.Drawing;

namespace GenericDB.BusinessObjects
{
	/// <summary>
	/// Represents the format for some text (= a string).
	/// Exposes a static property Regular for black and regular text.
	/// Eg. Bold and red formatting.
	/// </summary>
	public class TextFormat
	{
		public FontStyle Style { get; private set; }
		public Color Color { get; private set; }
		public string Name { get; private set; }

		public static readonly Color DefaultColor = Color.Black;
		public static readonly FontStyle DefaultFontStyle = FontStyle.Regular;

		public static readonly TextFormat Regular = new TextFormat("regular", DefaultFontStyle, DefaultColor);

		public TextFormat(string name, FontStyle style)
		{
			Name = name;
			Style = style;
			Color = DefaultColor;
		}

		public TextFormat(string name, Color color)
		{
			Name = name;
			Color = color;
			Style = DefaultFontStyle;
		}

		public TextFormat(string name, FontStyle style, Color color)
		{
			Name = name;
			Style = style;
			Color = color;
		}

		/// <summary>
		/// Gets the combinaison of two TextFormat objects.
		/// Regular TextFormat is always discarded in favor of another TextFormat, 
		/// so if one of the text format is regular, the other one is returned.
		/// If both are non-regular, a new TextFormat instance is returned, 
		/// whose name is "<format1name>+<format2name>".
		/// DefaultColor color is always discarded against another color.
		/// If there is a conflict of colors, the color of the second format is kept.
		/// </summary>
		/// <param name="format1">The first format.</param>
		/// <param name="format2">The second format.</param>
		/// <returns>The combinaison of the two formats.</returns>
		public static TextFormat Merge(TextFormat format1, TextFormat format2)
		{
			if (format1 == TextFormat.Regular)
				return format2;
			if (format2 == TextFormat.Regular)
				return format1;
			return new TextFormat(format1.Name + "+" + format2.Name,
				format1.Style | format2.Style, // merge styles
				(format2.Color == TextFormat.DefaultColor) ? format1.Color : format2.Color); // merge colors with the following rule : color2 is prefered to color1 if color2 is not the default color
		}
	}
}