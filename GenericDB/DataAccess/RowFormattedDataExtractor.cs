// GenericDB - A generic card searcher and deck builder library for CCGs
// Copyright © 2007, 2008, 2009, 2010, 2011, 2012, 2013 Vincent Ripoll
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
using System.Data;
using System.Globalization;
using System.Linq;
using GenericDB.BusinessObjects;

namespace GenericDB.DataAccess
{
    /// <summary>
    /// Help class for extracting formatted values from a data row.
    /// </summary>
    public class RowFormattedDataExtractor
    {
        protected IDictionary<string, TextFormat> Formats { get; set; }

        /// <summary>
        /// Creates a new intance of a RowFormattedDataExtractor, using a list of formats
        /// to return formatted values. The list must at least contain a text format called "errata" 
        /// associated to the errata text format, or an ArgumentException is thrown.
        /// The errata text format is used for all the values, the other text formats
        /// are used only by string values and their names must match the keys stored in the "style" columns.
        /// </summary>
        /// <param name="formats">An array of formats.</param>
        public RowFormattedDataExtractor(params TextFormat[] formats)
        {
            if (formats == null) throw new ArgumentNullException("formats");

            Formats = new Dictionary<string, TextFormat>();
            foreach (var format in formats)
                Formats.Add(format.Name, format);

            if (!Formats.ContainsKey("errata"))
                throw new ArgumentException("The dictionary of formats must at least contain the \"errata\" key associated to the errata text format.");
        }

        /// <summary>
        /// Extracts an int value from a column in a row, and formats it according to
        /// the presence or absence of errata (information stored in the column "[columnName]Errated")
        /// </summary>
        /// <param name="row">The row from which the int value is extracted.</param>
        /// <param name="column">The name of the column from which the int value is extracted.</param>
        /// <returns>A formatted int value.</returns>
        public virtual FormattedValue<int> GetIntAndStyleFromRow(DataRow row, string column)
        {
            string text = row[column].ToString().Trim();
            var isNull = string.IsNullOrEmpty(text);
            int value = isNull ? 0 : Int32.Parse(text);
            string errated = row[column + "Errated"].ToString().Trim();
            return new FormattedValue<int>(value, GetFormatFromErrata(errated), isNull);
        }

        /// <summary>
        /// Extracts an xint value from a column in a row, and formats it according to
        /// the presence or absence of errata (information stored in the column "[columnName]Errated")
        /// </summary>
        /// <param name="row">The row from which the xint value is extracted.</param>
        /// <param name="column">The name of the column from which the xint value is extracted.</param>
        /// <returns>A formatted xint value.</returns>
        public virtual FormattedValue<XInt> GetXIntAndStyleFromRow(DataRow row, string column)
        {
            string errated = row[column + "Errated"].ToString().Trim();
            string text = row[column].ToString().Trim();
            var isSet = !string.IsNullOrEmpty(text);
            var value = !isSet ? 0 : Int32.Parse(text, CultureInfo.InvariantCulture);
            var xIntResult = (value == -1) ? new XInt() : new XInt(value);
            xIntResult.IsSet = isSet;
            return new FormattedValue<XInt>(xIntResult, GetFormatFromErrata(errated));
        }

        /// <summary>
        /// Extracts a bool value from a column in a row, and formats it according to
        /// the presence or absence of errata (information stored in the column "[columnName]Errated")
        /// </summary>
        /// <param name="row">The row from which the bool value is extracted.</param>
        /// <param name="column">The name of the column from which the bool value is extracted.</param>
        /// <returns>A formatted bool value.</returns>
        public virtual FormattedValue<bool> GetBoolAndStyleFromRow(DataRow row, string column)
        {
            bool value = row[column].ToString().ToUpperInvariant() == true.ToString().ToUpperInvariant();
            string errated = row[column + "Errated"].ToString().Trim();
            return new FormattedValue<bool>(value, GetFormatFromErrata(errated));
        }

        /// <summary>
        /// Extracts a string value from a column in a row, and formats it according to
        /// the style stored in the column "[columnName]Style".
        /// </summary>
        /// <param name="row">The row from which the string value is extracted.</param>
        /// <param name="column">The name of the column from which the string value is extracted.</param>
        /// <returns>A formatted string value.</returns>
        public virtual FormattedValue<string> GetTextAndStyleFromRow(DataRow row, string column)
        {
            string text = row[column].ToString();
            string style = row[column + "Style"].ToString().Trim();
            return new FormattedValue<string>(text, GetFormatFromStyle(style));
        }

        public virtual Guid GetGuidFromRow(DataRow row, string column)
        {
            string text = row[column].ToString().Trim();
            Guid result;
            return Guid.TryParse(text, out result) ? result : Guid.Empty;
        }

        public virtual Guid[] GetGuidsFromRow(DataRow row, string column, char separator)
        {
            var result = row[column].ToString().Trim()
                .Split(separator)
                .Select(w =>
                    {
                        Guid guid;
                        return Guid.TryParse(w, out guid) ? guid : Guid.Empty;
                    })
                .Where(guid => guid != Guid.Empty)
                .ToArray();
            if (result.Length == 0)
            {
                result = new[] { Guid.Empty };
            }
            return result;
        }

        /// <summary>
        /// Returns a list of FormatSection for a non-string value, depending on whether 
        /// there is an errata or not for this value.
        /// If there is, the list contains an errata FormatSection applying to the totality of the value.
        /// If there isn't, the list is empty.
        /// </summary>
        /// <param name="errated">The value stored in the errated column.</param>
        /// <returns>A list of FormatSection representing the styles applied to the value.</returns>
        public virtual List<FormatSection> GetFormatFromErrata(string errated)
        {
            var formats = new List<FormatSection>();
            if (String.Equals(errated, "YES", StringComparison.OrdinalIgnoreCase)
                || String.Equals(errated, "True", StringComparison.OrdinalIgnoreCase))
                formats.Add(new FormatSection(0, 0, Formats["errata"]));
            return formats;
        }

        /// <summary>
        /// Returns a list of FormatSection for a string value, depending on the data stored
        /// in the associated "style" column. Styles must be stored under the following format:
        /// style[1]; style[2]; ...; style[n] 
        /// with style[i] under the following format:
        /// styleName, startindex-endindex
        /// </summary>
        /// <param name="style">The value stored in the style column.</param>
        /// <returns>A list of FormatSection representing the styles applied to the value.</returns>
        protected virtual List<FormatSection> GetFormatFromStyle(string style)
        {
            var formats = new List<FormatSection>();

            if (!string.IsNullOrEmpty(style))
            {
                string[] styles = style.Split(';');
                for (var i = 0; i < styles.Length; ++i)
                {
                    string[] styleinfo = styles[i].Trim().Split(',', '-');
                    TextFormat textFormat;
                    if (Formats.TryGetValue(styleinfo[0].Trim(), out textFormat))
                        formats.Add(new FormatSection(
                            Int32.Parse(styleinfo[1], CultureInfo.InvariantCulture),
                            Int32.Parse(styleinfo[2], CultureInfo.InvariantCulture),
                            textFormat));
                }
            }
            return formats;
        }
    }
}
