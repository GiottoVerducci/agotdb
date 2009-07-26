// WoMDB - A card searcher and deck builder tool for the CCG "Wizards of Mickey"
// Copyright © 2009 Vincent Ripoll
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
// © Wizards of Mickey CCG ??? ???

using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Xml;
using GenericDB;
using GenericDB.BusinessObjects;
using GenericDB.DataAccess;

namespace WoMDB.BusinessObjects
{
	public class WomCard : Card
	{
		public FormattedValue<string> Name, Title, Team, Text, Set;
		public FormattedValue<int> Type, Color;
		public FormattedValue<XInt> Power;

		private string fSummaryInfo = ""; // cache for GetSummaryInfo method

		public enum CardType { Unknown = -1, None = 0, Wizard = 1, Castle = 2, Spell = 4 };
		public enum CardColor { Unknown = -1, None = 0, Blue = 1, Green = 2, Red = 4, Yellow = 8, Black = 16 };

		public static readonly TextFormat ErrataFormat = new TextFormat("errata", System.Drawing.Color.Red);
		public static readonly TextFormat SpecialFormat = new TextFormat("special", FontStyle.Bold);
		private static readonly RowFormattedDataExtractor fRowFormattedDataExtractor =
			new RowFormattedDataExtractor(ErrataFormat, SpecialFormat);
		public static IDictionary<int, string> CardTypeNames, CardColorNames;

		#region Constructors and clone
		public WomCard()
		{
			Quantity = 1;
		}

		public WomCard(DataRow row)
			: this()
		{
			InitializeFromDataRow(row);
		}

		private void InitializeFromDataRow(DataRow row)
		{
			var rfde = fRowFormattedDataExtractor; // alias

			UniversalId = (int)row["UniversalId"];
			Name = rfde.GetTextAndStyleFromRow(row, "Name");
			Title = rfde.GetTextAndStyleFromRow(row, "Title");
			Team = rfde.GetTextAndStyleFromRow(row, "Team");
			Type = rfde.GetIntAndStyleFromRow(row, "Type");

			int color = 0;
			if ((bool)row["ColorBlue"])
				color += (int)CardColor.Blue;
			if ((bool)row["ColorGreen"])
				color += (int)CardColor.Green;
			if ((bool)row["ColorRed"])
				color += (int)CardColor.Red;
			if ((bool)row["ColorYellow"])
				color += (int)CardColor.Yellow;
			if ((bool)row["ColorBlack"])
				color += (int)CardColor.Black;
			Color = new FormattedValue<int>(color, rfde.GetFormatFromErrata(row["ColorErrated"].ToString()));

			Text = rfde.GetTextAndStyleFromRow(row, "Text");
			Power = rfde.GetXIntAndStyleFromRow(row, "Power");
			Set = rfde.GetTextAndStyleFromRow(row, "Set");
		}

		private WomCard(WomCard original)
			: this()
		{
			UniversalId = original.UniversalId;
			Quantity = original.Quantity;
			Name = original.Name;
			Title = original.Title;
			Team = original.Team;
			Type = original.Type;
			Color = original.Color;
			Text = original.Text;
			Power = original.Power;
			Set = original.Set;
		}

		/// <summary>
		/// Clones this card object by copying all its properties. This is a deep copy.
		/// </summary>
		/// <returns>A clone object of this object.</returns>
		public override ICard Clone()
		{
			return new WomCard(this);
		}
		#endregion

		#region Implementation of IXmlizable
		public override XmlNode ToXml(XmlDocument doc)
		{
			XmlElement value = doc.CreateElement("Card");
			XmlToolbox.AddElementValue(doc, value, "UniversalId", UniversalId.ToString(CultureInfo.InvariantCulture));
			XmlToolbox.AddElementValue(doc, value, "Quantity", Quantity.ToString(CultureInfo.InvariantCulture));
			XmlToolbox.AddElementValue(doc, value, "Name", Name.Value);
			XmlToolbox.AddElementValue(doc, value, "Title", Title.Value);
			XmlToolbox.AddElementValue(doc, value, "Set", Set.Value);
			return value;
		}

		/// <summary>
		/// Constructs this object from an XMLNode that was generated using the ToXml method.
		/// </summary>
		/// <param name="doc">The XmlDocument containing the XMLNode.</param>
		/// <param name="root">The root node containing the XML data representing the card.</param>
		/// <remarks>The card is built by using only the UniversalId and quantity informations stored in the node. If
		/// no card with the same UniversalId can be found in the database, we create an object only partially
		/// initialized with the informations that were stored in the node. This allows us to support cards that
		/// are not in the database, such as cards from older sets, proxy cards or new cards.</remarks>
		public override void InitializeFromXml(XmlDocument doc, XmlNode root)
		{
			if (root == null) throw new ArgumentNullException("root");
			if (root.Name != "Card") throw new XmlException("Invalid card root node");

			UniversalId = Int32.Parse(XmlToolbox.GetElementValue(doc, root, "UniversalId"));
			var table = ApplicationSettings.DatabaseManager.GetCardFromUniversalId(UniversalId);
			if (table.Rows.Count <= 0)
			{
				var recordedName = XmlToolbox.GetElementValue(doc, root, "Name");
				var recordedTitle = XmlToolbox.GetElementValue(doc, root, "Title");
				var recordedSet = XmlToolbox.GetElementValue(doc, root, "Set");
				// try to match the card with its name and expansion number (for cards that were merged in previous versions)
				table = ApplicationSettings.DatabaseManager.GetCardFromNameAndSet(recordedName, recordedSet);
				if (table.Rows.Count <= 0)
				{
					Name = new FormattedValue<string>(recordedName, new List<FormatSection>());
					Title = new FormattedValue<string>(recordedTitle, new List<FormatSection>());
					Set = new FormattedValue<string>(recordedSet, new List<FormatSection>());
					Text = new FormattedValue<string>(Resource1.UnknownCard, new List<FormatSection>());
				}
			}

			if (table.Rows.Count > 0)
				InitializeFromDataRow(table.Rows[0]);
			Quantity = Int32.Parse(XmlToolbox.GetElementValue(doc, root, "Quantity"), CultureInfo.InvariantCulture);
		}
		#endregion

		#region Implementation of IComparable
		/// <summary>
		/// Returns the comparison relationship between this card and another card.
		/// Both cards must have the same type, otherwise a plain name comparison is done.
		/// </summary>
		/// <param name="otherCard">The card to compare this card to.</param>
		/// <returns>-1 if this card is before the other card, 0 if they're equal, 1 otherwise</returns>
		public override int CompareTo(ICard otherCard)
		{
			if (otherCard == null) throw new ArgumentNullException("otherCard");

			var otherWomCard = (WomCard)otherCard;
			if (otherWomCard.Type != null && Type != null && otherWomCard.Type.Value == Type.Value)
			{
				switch ((CardType)Type.Value)
				{
					case CardType.Wizard:
						return Tools.MultipleCompare(
							Team.Value.CompareTo(otherWomCard.Team.Value),
							string.Compare(Name.Value, otherWomCard.Name.Value, StringComparison.CurrentCulture));
					case CardType.Castle:
						return Tools.MultipleCompare(
							Color.Value.CompareTo(otherWomCard.Color.Value),
							string.Compare(Name.Value, otherWomCard.Name.Value, StringComparison.CurrentCulture));
					case CardType.Spell:
						return Tools.MultipleCompare(
							Power.Value.CompareTo(otherWomCard.Power.Value),
							Color.Value.CompareTo(otherWomCard.Color.Value),
							string.Compare(Name.Value, otherWomCard.Name.Value, StringComparison.CurrentCulture));
				}
			}
			return Name.Value.CompareTo(otherWomCard.Name.Value);
		}
		#endregion

		/// <summary>
		/// Returns the card text to a plain text string.
		/// </summary>
		/// <returns>The card text in plain text.</returns>
		public override string ToPlainFullString()
		{
			var result = string.Format(CultureInfo.CurrentCulture, "{0}{1}{2}",
				Resource1.NameText, Resource1.SeparatorText, Name.Value);
			IList<FormattedText> formatted = ToFormattedString();
			for (var i = 1; i < formatted.Count; ++i) // start at index 1 to skip the name part
				if ((formatted[i].Text != "\r\n") || ((i + 1 < formatted.Count) && (formatted[i + 1].Text != "\r\n")))
					result += formatted[i].Text;
			return result;
		}

		/// <summary>
		/// Returns the name of the card.
		/// </summary>
		/// <returns>The name of the card.</returns>
		public override string ToString()
		{
			return string.IsNullOrEmpty(Title.Value)
				? Name.Value
				: string.Format("{0}, {1}", Name.Value, Title.Value);
		}

		/// <summary>
		/// Returns the set (abbreviated) in which the card first appeared.
		/// </summary>
		/// <returns>The set as a string.</returns>
		public override string GetShortSet()
		{
			return Set.Value.Substring(0, Set.Value.IndexOf('('));
		}

		/// <summary>
		/// Returns a string giving a summary of the main characteristics of the card.
		/// </summary>
		/// <returns>The summary.</returns>
		public override string GetSummaryInfo()
		{
			if (fSummaryInfo != "")
				return fSummaryInfo;
			if (Type == null) // for virtual cards
				return "";
			switch ((CardType)Type.Value)
			{
				case CardType.Wizard:
					fSummaryInfo = string.Format(CultureInfo.CurrentCulture, "{0}",
						WomCard.GetColorName(Color.Value));
					break;
				case CardType.Castle:
					fSummaryInfo = string.Format(CultureInfo.CurrentCulture, "{0}",
						WomCard.GetColorName(Color.Value));
					break;
				case CardType.Spell:
					fSummaryInfo = string.Format(CultureInfo.CurrentCulture, "[{0}] {1}",
						Power.Value, WomCard.GetColorName(Color.Value));
					break;
				default: fSummaryInfo = ""; break;
			}
			fSummaryInfo = fSummaryInfo.Trim();
			return fSummaryInfo;
		}

		/// <summary>
		/// Searches the CardTypeNames dictionary of types for the type with the same id in its tag and returns the associated string.
		/// The CardTypeNames dictionary must have been already initialized.
		/// The type must be present in the dictionary, otherwise an exception is thrown.
		/// </summary>
		/// <param name="type">The integer value representing the type.</param>
		/// <returns>The string associated to the type.</returns>
		public static string GetTypeName(int type)
		{
			return CardTypeNames[type];
		}

		/// <summary>
		/// Searches the CardColorNames dictionary of colors for the colors with the same id (bitwise) in its tag and returns the associated string.
		/// The CardColorNames dictionary must have been already initialized.
		/// All colors must be present in the dictionary, otherwise an exception is thrown.
		/// </summary>
		/// <param name="color">The integer value representing the colors.</param>
		/// <returns>The string associated to the colors (separated by '/' if more than one color is present).</returns>
		public static string GetColorName(int color)
		{
			if (color == 0) // no color
				return CardColorNames[0];
			var colorNames = from c in CardColorNames where (c.Key & color) != 0 select c.Value;
			return string.Join("/", colorNames.ToArray());
		}

		/// <summary>
		/// Gets a list of ordonnated formatted elements representing this card.
		/// </summary>
		/// <returns>A list of FormattedText elements containing the informations about this card.</returns>
		public override IList<FormattedText> ToFormattedString()
		{
			var separator = Resource1.SeparatorText;

			var result = new List<FormattedText>();

			var nameFormat = new TextFormat("name", FontStyle.Bold);
			var teamFormat = new TextFormat("team", FontStyle.Bold | FontStyle.Italic);

			result.AddRange(FormattedValueStringToFormattedText(Name, nameFormat));

			if (!string.IsNullOrEmpty(Title.Value))
			{
				result.Add(new FormattedText(", ", nameFormat));
				result.AddRange(FormattedValueStringToFormattedText(Title, nameFormat));
			}

			result.Add(new FormattedText("\r\n"));

			result.Add(new FormattedText(Resource1.SetText + separator));
			result.AddRange(FormattedValueStringToFormattedText(Set, TextFormat.Regular));
			result.Add(new FormattedText("\r\n"));

			if (Type != null)
			{
				result.Add(new FormattedText(Resource1.TypeText + separator));
				result.Add(new FormattedText(GetTypeName(Type.Value), (Type.Formats.Count > 0) ? Type.Formats[0].Format : TextFormat.Regular));
				result.Add(new FormattedText("\r\n"));
			}

			if (Color != null)
			{
				result.Add(new FormattedText(Resource1.ColorText + separator));
				result.Add(new FormattedText(GetColorName(Color.Value), (Color.Formats.Count > 0) ? Color.Formats[0].Format : TextFormat.Regular));
				result.Add(new FormattedText("\r\n"));
			}

			switch (Type != null ? (CardType)Type.Value : CardType.Unknown)
			{
				case CardType.Wizard:
					result.Add(new FormattedText(Resource1.TeamText + separator));
					result.AddRange(FormattedValueStringToFormattedText(Team, teamFormat));
					result.Add(new FormattedText("\r\n"));
					break;
				case CardType.Spell:
					result.Add(new FormattedText(Resource1.PowerText + separator));
					result.AddRange(FormattedValueXIntToFormattedText(Power, TextFormat.Regular));
					result.Add(new FormattedText("\r\n"));
					break;
			}

			if (Text != null && !string.IsNullOrEmpty(Text.Value))
			{
				result.AddRange(FormattedValueStringToFormattedText(Text, TextFormat.Regular));
			}

			// remove the \r\n from the last line
			var lastLine = new FormattedText(result[result.Count - 1].Text.TrimEnd('\r', '\n'), result[result.Count - 1].Format);
			result.RemoveAt(result.Count - 1);
			result.Add(lastLine);
			return result;
		}

		public override bool IsDrawable()
		{
			return (Type.Value == (int)CardType.Spell);
		}

	}
}
