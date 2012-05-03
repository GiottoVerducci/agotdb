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
using System.Xml;

namespace GenericDB.BusinessObjects
{
	public abstract class Card : ICard
	{
		public Int32 UniversalId { get; set; }
		public int Quantity { get; set; }

		#region Constructors and clone
		public abstract ICard Clone();
		#endregion

		#region Implementation of IXmlizable
		/// <summary>
		/// Gets the XML representation of this card.
		/// </summary>
		/// <param name="doc">The XML document for which the XML representation is generated.</param>
		/// <returns>A XML node representing this card.</returns>
		public abstract XmlNode ToXml(XmlDocument doc);
		/// <summary>
		/// Initializes the properties of this card from an XML node that was generated 
		/// using the ToXml method.
		/// </summary>
		/// <param name="doc">The XML document containing the XML node.</param>
		/// <param name="root">The XML node containing the XML data representing the object.</param>
		public abstract void InitializeFromXml(XmlDocument doc, XmlNode root);
		#endregion

		#region Implementation of IComparable
		public abstract int CompareTo(ICard otherCard);
		#endregion

		public abstract string ToPlainFullString();
		public abstract string GetShortSet();
		public abstract string GetSummaryInfo();
		public abstract IList<FormattedText> ToFormattedString();
		public abstract bool IsDrawable();

		/// <summary>
		/// Applies formats to list of FormattedText elements representing a formatted text. The formats are applied
		/// on portions of the formatted text (the begin (included) and end (excluded) indexes are stored in each FormatSection element),
		/// which means that a formatted element may be splitted to apply a format on a part of it.
		/// Eg. A plain text element "babar" on which a format 2-4 bold is applied will result in a list of 3
		/// FormattedText elements : "ba" (plain) + "ba" (bold) + "r" (plain).
		/// </summary>
		/// <param name="result">The list representing the text on which the formats are applied.</param>
		/// <param name="formats">The list of formats to apply.</param>
		protected static void ApplyFormatsToFormattedText(List<FormattedText> result, IList<FormatSection> formats)
		{
			for (var i = 0; i < formats.Count; ++i)
			{
				// go to the first FormattedText element affected by the format
				int j = 0;
				int pos = 0;
				bool formatAppliesToNext;
				do
				{
					while (pos + result[j].Text.Length < formats[i].Begin)
					{
						pos += result[j].Text.Length;
						j++;
					}

					int relBegin = Math.Max(formats[i].Begin - pos, 0); // peut être inférieur à 0 si formatAppliesToNext a précédemment été mis à vrai
					int relEnd = formats[i].End - pos;
					if (relEnd > result[j].Text.Length)
					{
						relEnd = result[j].Text.Length;
						formatAppliesToNext = true;
					}
					else
						formatAppliesToNext = false;

					// we split the FormattedText element and apply the format to the right portion
					var subResult = new List<FormattedText>();
					if (relBegin != 0)
						subResult.Add(new FormattedText(result[j].Text.Substring(0, relBegin), result[j].Format));
					subResult.Add(new FormattedText(result[j].Text.Substring(relBegin, relEnd - relBegin),
						TextFormat.Merge(result[j].Format, formats[i].Format)));
					if (relEnd != result[j].Text.Length)
						subResult.Add(new FormattedText(result[j].Text.Substring(relEnd, result[j].Text.Length - relEnd), result[j].Format));

					// we replace the old element by the new splitted one
					result.RemoveAt(j);
					result.InsertRange(j, subResult);

					// we have to adjust the indexes if we carry the format on the next part
					if (formatAppliesToNext)
					{
						if (relBegin != 0) { pos += result[j].Text.Length; ++j; }
						pos += result[j].Text.Length; ++j;
					}
				}
				while (formatAppliesToNext);
			}
		}

		protected static List<FormattedText> FormattedValueStringToFormattedText(FormattedValue<string> value, TextFormat defaultFormat)
		{
			var result = new List<FormattedText>();
			if (value != null)
			{
				result.Add(new FormattedText(value.Value, defaultFormat));
				// we start from one piece of text, then split the pieces according to the begin and end indexes of the format
				ApplyFormatsToFormattedText(result, value.Formats);
			}
			return result;
		}

		protected static List<FormattedText> FormattedValueBoolToFormattedText(FormattedValue<bool> value, string yesValue, string noValue, TextFormat defaultFormat)
		{
			var result = new List<FormattedText>();
			if (value != null)
			{
				if (value.Formats.Count > 0)
					result.Add(new FormattedText(value.Value ? yesValue : noValue, TextFormat.Merge(value.Formats[0].Format, defaultFormat)));
				else
					result.Add(new FormattedText(value.Value ? yesValue : noValue, defaultFormat));
			}
			return result;
		}

		protected static List<FormattedText> FormattedValueXIntToFormattedText(FormattedValue<XInt> value, TextFormat defaultFormat)
		{
			var result = new List<FormattedText>();
			if (value != null)
			{
				if (value.Formats.Count > 0)
					result.Add(new FormattedText(value.Value.ToString(), TextFormat.Merge(value.Formats[0].Format, defaultFormat)));
				else
					result.Add(new FormattedText(value.Value.ToString(), defaultFormat));
			}
			return result;
		}
	}
}