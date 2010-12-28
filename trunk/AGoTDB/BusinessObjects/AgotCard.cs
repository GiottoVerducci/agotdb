// AGoTDB - A card searcher and deck builder tool for the CCG "A Game of Thrones"
// Copyright © 2007, 2008, 2009, 2010, 2011 Vincent Ripoll
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
// © A Game of Thrones 2005 George R. R. Martin
// © A Game of Thrones CCG 2005 Fantasy Flight Publishing, Inc.
// © A Game of Thrones LCG 2008 Fantasy Flight Publishing, Inc.
// © Le Trône de Fer JCC 2005-2007 Stratagèmes éditions / Xénomorphe Sàrl
// © Le Trône de Fer JCE 2008 Edge Entertainment

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

namespace AGoTDB.BusinessObjects
{
	public class AgotCard : Card
	{
		public FormattedValue<string> Name, Traits, Keywords, Text, Set, OriginalName;
		public FormattedValue<int> Type, House;
		public FormattedValue<bool> Unique, Doomed, Endless, Military, Intrigue, Power, War, Holy, Noble, Learned, Shadow, Multiplayer;
		public FormattedValue<XInt> Cost, Strength, Income, Initiative, Claim, Influence;

		private string fSummaryInfo = ""; // cache for GetSummaryInfo method

		public enum CardType { Unknown = -1, None = 0, Character = 1, Location = 2, Attachment = 4, Event = 8, Plot = 16, Agenda = 32, Title = 64 };
		public enum CardHouse { Unknown = -1, Neutral = 0, Stark = 1, Lannister = 2, Baratheon = 4, Greyjoy = 8, Martell = 16, Targaryen = 32 };

		public static readonly TextFormat ErrataFormat = new TextFormat("errata", Color.Red);
		public static readonly TextFormat TraitsFormat = new TextFormat("traits", FontStyle.Bold | FontStyle.Italic);
		public static readonly TextFormat TriggerFormat = new TextFormat("trigger", FontStyle.Bold);
		private static readonly RowFormattedDataExtractor fRowFormattedDataExtractor =
			new RowFormattedDataExtractor(ErrataFormat, TraitsFormat, TriggerFormat);
		public static IDictionary<int, string> CardTypeNames, CardHouseNames;
		public static IList<string> CardTriggerNames;

		// "Id", "Name", "Type", "House", "Unique", "Traits", "Keywords", "Text", "Doomed", "Endless", "Cost", "Strength",
		// "Military", "Intrigue", "Power", "War", "Holy", "Noble", "Learned", "Shadow", "Income", "Initiative", "Claim",
		// "Influence", "Multiplayer", "Set", "OriginalName", "UniversalId");

		#region Constructors and clone
		public AgotCard()
		{
			Quantity = 1;
		}

		public AgotCard(DataRow row)
			: this()
		{
			InitializeFromDataRow(row);
		}

		private void InitializeFromDataRow(DataRow row)
		{
			var rfde = fRowFormattedDataExtractor; // alias

			UniversalId = (Int32)row["UniversalId"];
			Name = rfde.GetTextAndStyleFromRow(row, "Name");
			Type = rfde.GetIntAndStyleFromRow(row, "Type");
			int house = 0;
			if ((Boolean)row["HouseStark"])
				house += (int)CardHouse.Stark;
			if ((Boolean)row["HouseLannister"])
				house += (int)CardHouse.Lannister;
			if ((Boolean)row["HouseBaratheon"])
				house += (int)CardHouse.Baratheon;
			if ((Boolean)row["HouseGreyjoy"])
				house += (int)CardHouse.Greyjoy;
			if ((Boolean)row["HouseMartell"])
				house += (int)CardHouse.Martell;
			if ((Boolean)row["HouseTargaryen"])
				house += (int)CardHouse.Targaryen;
			House = new FormattedValue<int>(house, rfde.GetFormatFromErrata(row["HouseErrated"].ToString()));

			Unique = rfde.GetBoolAndStyleFromRow(row, "Unique");
			Traits = rfde.GetTextAndStyleFromRow(row, "Traits");
			Keywords = rfde.GetTextAndStyleFromRow(row, "Keywords");
			Text = rfde.GetTextAndStyleFromRow(row, "Text");
			Doomed = rfde.GetBoolAndStyleFromRow(row, "Doomed");
			Endless = rfde.GetBoolAndStyleFromRow(row, "Endless");
			Cost = rfde.GetXIntAndStyleFromRow(row, "Cost");
			Strength = rfde.GetXIntAndStyleFromRow(row, "Strength");
			Military = rfde.GetBoolAndStyleFromRow(row, "Military");
			Intrigue = rfde.GetBoolAndStyleFromRow(row, "Intrigue");
			Power = rfde.GetBoolAndStyleFromRow(row, "Power");
			War = rfde.GetBoolAndStyleFromRow(row, "War");
			Holy = rfde.GetBoolAndStyleFromRow(row, "Holy");
			Noble = rfde.GetBoolAndStyleFromRow(row, "Noble");
			Learned = rfde.GetBoolAndStyleFromRow(row, "Learned");
			Shadow = rfde.GetBoolAndStyleFromRow(row, "Shadow");
			Income = rfde.GetXIntAndStyleFromRow(row, "Income");
			Initiative = rfde.GetXIntAndStyleFromRow(row, "Initiative");
			Claim = rfde.GetXIntAndStyleFromRow(row, "Claim");
			Influence = rfde.GetXIntAndStyleFromRow(row, "Influence");
			Multiplayer = rfde.GetBoolAndStyleFromRow(row, "Multiplayer");
			Set = rfde.GetTextAndStyleFromRow(row, "Set");
			OriginalName = rfde.GetTextAndStyleFromRow(row, "OriginalName");
		}

		private AgotCard(AgotCard original)
			: this()
		{
			UniversalId = original.UniversalId;
			Quantity = original.Quantity;
			Name = original.Name;
			Traits = original.Traits;
			Keywords = original.Keywords;
			Text = original.Text;
			Set = original.Set;
			OriginalName = original.OriginalName;
			Type = original.Type;
			House = original.House;
			Unique = original.Unique;
			Doomed = original.Doomed;
			Endless = original.Endless;
			Military = original.Military;
			Intrigue = original.Intrigue;
			Power = original.Power;
			War = original.War;
			Holy = original.Holy;
			Noble = original.Noble;
			Learned = original.Learned;
			Shadow = original.Shadow;
			Multiplayer = original.Multiplayer;
			Cost = original.Cost;
			Strength = original.Strength;
			Income = original.Income;
			Initiative = original.Initiative;
			Claim = original.Claim;
			Influence = original.Influence;
		}

		/// <summary>
		/// Clones this card object by copying all its properties. This is a deep copy.
		/// </summary>
		/// <returns>A clone object of this object.</returns>
		public override ICard Clone()
		{
			return new AgotCard(this);
		}
		#endregion

		#region Implementation of IXmlizable
		public override XmlNode ToXml(XmlDocument doc)
		{
			XmlElement value = doc.CreateElement("Card");
			XmlToolbox.AddElementValue(doc, value, "UniversalId", UniversalId.ToString(CultureInfo.InvariantCulture));
			XmlToolbox.AddElementValue(doc, value, "Quantity", Quantity.ToString(CultureInfo.InvariantCulture));
			XmlToolbox.AddElementValue(doc, value, "Name", Name.Value);
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
				var recordedSet = XmlToolbox.GetElementValue(doc, root, "Set");
				// try to match the card with its name and expansion number (for cards that were merged in previous versions)
				table = ApplicationSettings.DatabaseManager.GetCardFromNameAndSet(recordedName, recordedSet);
				if (table.Rows.Count <= 0)
				{
					Name = new FormattedValue<string>(recordedName, new List<FormatSection>());
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

			var otherAgotCard = (AgotCard)otherCard;
			if (otherAgotCard.Type != null && Type != null && otherAgotCard.Type.Value == Type.Value)
			{
				switch ((CardType)Type.Value)
				{
					case CardType.Character:
					case CardType.Location:
					case CardType.Attachment: return Tools.MultipleCompare(
						Cost.Value.CompareTo(otherAgotCard.Cost.Value),
						string.Compare(Name.Value, otherAgotCard.Name.Value, StringComparison.CurrentCulture));
					default:
					case CardType.Event:
					case CardType.Agenda:
					case CardType.Title: return string.Compare(Name.Value, otherAgotCard.Name.Value, StringComparison.CurrentCulture);
					case CardType.Plot: return Tools.MultipleCompare(
						Claim.Value.CompareTo(otherAgotCard.Claim.Value),
						Income.Value.CompareTo(otherAgotCard.Income.Value),
						string.Compare(Name.Value, otherAgotCard.Name.Value, StringComparison.CurrentCulture));
				}
			}
			return Name.Value.CompareTo(otherAgotCard.Name.Value);
		}
		#endregion

		#region Equality
		/// <summary>
		/// Compares this card with another card by comparing their universal id and their quantity.
		/// If one reference is null and the other isn't, the method returns false.
		/// </summary>
		/// <param name="obj">The other card.</param>
		/// <returns>True if the two cards are identical by content, false otherwise.</returns>
		public override bool Equals(object obj)
		{
			// if parameter cannot be cast to this, return false
			var card = obj as AgotCard;
			if (card == null)
				return false;

			return (UniversalId == card.UniversalId) && (Quantity == card.Quantity);
		}

		public override int GetHashCode()
		{
			return base.GetHashCode();
		}
		#endregion

		/// <summary>
		/// Returns the card text to a plain text string.
		/// </summary>
		/// <returns>The card text in plain text.</returns>
		public override string ToPlainFullString()
		{
			var result = String.Format(CultureInfo.CurrentCulture, "{0}{1}{2}",
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
			return Name.Value;
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
				case CardType.Plot:
					fSummaryInfo = String.Format(CultureInfo.CurrentCulture, "{0,2} {1,2} {2,2}", Income.Value, Initiative.Value, Claim.Value);
					if (!string.IsNullOrEmpty(Traits.Value))
						fSummaryInfo += " - " + Traits.Value;
					break;
				case CardType.Attachment:
					fSummaryInfo = String.Format(CultureInfo.CurrentCulture, "[{0}]", Cost.Value);
					if (Shadow.Value) fSummaryInfo += String.Format(" [{0}]", Resource1.ShadowVirtueText);
					if (!string.IsNullOrEmpty(Keywords.Value))
						fSummaryInfo += " - " + Keywords.Value;
					break;
				case CardType.Character:
					fSummaryInfo = String.Format(CultureInfo.CurrentCulture, "[{0}] {1,2} {2} {3} {4}", Cost.Value, Strength.Value, (Military.Value) ? Resource1.MilitaryIconAbrev[0] : ' ', (Intrigue.Value) ? Resource1.IntrigueIconAbrev[0] : ' ', (Power.Value) ? Resource1.PowerIconAbrev[0] : ' ');
					if (!string.IsNullOrEmpty(Traits.Value))
						fSummaryInfo += " - " + Traits.Value;
					if (Noble.Value) fSummaryInfo += String.Format(" [{0}]", Resource1.NobleVirtueText);
					if (War.Value) fSummaryInfo += String.Format(" [{0}]", Resource1.WarVirtueText);
					if (Holy.Value) fSummaryInfo += String.Format(" [{0}]", Resource1.HolyVirtueText);
					if (Learned.Value) fSummaryInfo += String.Format(" [{0}]", Resource1.LearnedVirtueText);
					if (Shadow.Value) fSummaryInfo += String.Format(" [{0}]", Resource1.ShadowVirtueText);
					if (!string.IsNullOrEmpty(Keywords.Value))
						fSummaryInfo += " - " + Keywords.Value;
					break;
				case CardType.Event:
					if (Cost.Value.IsNonzero())  // for old events with a gold cost
						fSummaryInfo = String.Format(CultureInfo.CurrentCulture, "[{0}]", Cost.Value);
					if (!string.IsNullOrEmpty(Traits.Value))
						fSummaryInfo += " - " + Traits.Value;
					if (Shadow.Value) fSummaryInfo += String.Format(" [{0}]", Resource1.ShadowVirtueText);
					if (!string.IsNullOrEmpty(Keywords.Value))
						fSummaryInfo += " - " + Keywords.Value;
					break;
				case CardType.Location:
					fSummaryInfo = String.Format(CultureInfo.CurrentCulture, "[{0}]", Cost.Value);
					fSummaryInfo += (Income.Value.IsNonzero()) ? (string.Format(CultureInfo.CurrentCulture, "+{0}", Income.Value)) : "  ";
					fSummaryInfo += (Influence.Value.IsNonzero()) ? String.Format(CultureInfo.CurrentCulture, "|{0}|", Influence.Value) : "   ";
					fSummaryInfo += (Initiative.Value.IsNonzero()) ? String.Format(CultureInfo.CurrentCulture, "<{0}>", Initiative.Value) : "   ";
					if (!string.IsNullOrEmpty(Traits.Value))
						fSummaryInfo += " - " + Traits.Value;
					if (Shadow.Value) fSummaryInfo += String.Format(" [{0}]", Resource1.ShadowVirtueText);
					if (!string.IsNullOrEmpty(Keywords.Value))
						fSummaryInfo += " - " + Keywords.Value;
					break;
				default: fSummaryInfo = ""; break;
			}
			fSummaryInfo = fSummaryInfo.Trim();
			if (Doomed.Value)
				fSummaryInfo += " " + Resource1.DoomedText;
			return fSummaryInfo;
		}

		/// <summary>
		/// Searches the CardTypeNames dictionary of types for the type with the same id in its tag and returns the associated string.
		/// The CardTypeNames dictionary must have been already initialized.
		/// The type must be present in the dictionary, otherwise an exception is thrown.
		/// </summary>
		/// <param name="type">The integer value representing the type.</param>
		/// <returns>The string associated to the type.</returns>
		public static string GetTypeName(Int32 type)
		{
			return CardTypeNames[type];
		}

		/// <summary>
		/// Searches the CardHouseNames dictionary of houses for the houses with the same id (bitwise) in its tag and returns the associated string.
		/// The CardHouseNames dictionary must have been already initialized.
		/// All houses must be present in the dictionary, otherwise an exception is thrown.
		/// </summary>
		/// <param name="house">The integer value representing the houses.</param>
		/// <returns>The string associated to the houses (separated by '/' if more than one house is present).</returns>
		public static string GetHouseName(Int32 house)
		{
			if (house == 0) // neutral house
				return CardHouseNames[0];
			var houseNames = from h in CardHouseNames where (h.Key & house) != 0 select h.Value;
			return string.Join("/", houseNames.ToArray());
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

			result.AddRange(FormattedValueStringToFormattedText(Name, nameFormat));
			result.AddRange(FormattedValueBoolToFormattedText(Unique, " *", "", nameFormat));
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
			switch (Type != null ? (CardType)Type.Value : CardType.Unknown)
			{
				case CardType.Location:
				case CardType.Attachment:
					result.Add(new FormattedText(Resource1.HouseText + separator));
					result.Add(new FormattedText(GetHouseName(House.Value), (House.Formats.Count > 0) ? House.Formats[0].Format : TextFormat.Regular));
					result.Add(new FormattedText("\r\n"));
					result.Add(new FormattedText((Shadow.Value ? Resource1.ShadowCostText : Resource1.CostText) + separator));
					result.AddRange(FormattedValueXIntToFormattedText(Cost, TextFormat.Regular));
					result.Add(new FormattedText("\r\n"));
					if (Shadow.Value)
					{
						result.Add(new FormattedText(Resource1.VirtuesText + separator));
						result.AddRange(FormattedValueBoolToFormattedText(Shadow, Resource1.ShadowVirtueText + ". ", "", TextFormat.Regular));
						result.Add(new FormattedText("\r\n"));
					}
					break;
				case CardType.Event:
					result.Add(new FormattedText(Resource1.HouseText + separator));
					result.Add(new FormattedText(GetHouseName(House.Value), (House.Formats.Count > 0) ? House.Formats[0].Format : TextFormat.Regular));
					result.Add(new FormattedText("\r\n"));
					if (Shadow.Value)
					{
						result.Add(new FormattedText(Resource1.ShadowCostText + separator));
						result.AddRange(FormattedValueXIntToFormattedText(Cost, TextFormat.Regular));
						result.Add(new FormattedText("\r\n"));
						result.Add(new FormattedText(Resource1.VirtuesText + separator));
						result.AddRange(FormattedValueBoolToFormattedText(Shadow, Resource1.ShadowVirtueText + ". ", "", TextFormat.Regular));
						result.Add(new FormattedText("\r\n"));
					}
					break;
				case CardType.Character:
					result.Add(new FormattedText(Resource1.HouseText + separator));
					result.Add(new FormattedText(GetHouseName(House.Value), (House.Formats.Count > 0) ? House.Formats[0].Format : TextFormat.Regular));
					result.Add(new FormattedText("\r\n"));
					result.Add(new FormattedText((Shadow.Value ? Resource1.ShadowCostText : Resource1.CostText) + separator));
					result.AddRange(FormattedValueXIntToFormattedText(Cost, TextFormat.Regular));
					result.Add(new FormattedText("\r\n"));
					result.Add(new FormattedText(Resource1.StrengthText + separator));
					result.AddRange(FormattedValueXIntToFormattedText(Strength, TextFormat.Regular));
					result.Add(new FormattedText("\r\n"));
					result.Add(new FormattedText(Resource1.IconsText + separator));
					if (Military.Value || Intrigue.Value || Power.Value)
					{
						result.AddRange(FormattedValueBoolToFormattedText(Military, Resource1.MilitaryIconAbrev + " ", "", TextFormat.Regular));
						result.AddRange(FormattedValueBoolToFormattedText(Intrigue, Resource1.IntrigueIconAbrev + " ", "", TextFormat.Regular));
						result.AddRange(FormattedValueBoolToFormattedText(Power, Resource1.PowerIconAbrev + " ", "", TextFormat.Regular));
					}
					else
						result.Add(new FormattedText("-"));
					result.Add(new FormattedText("\r\n"));
					if (Noble.Value || War.Value || Holy.Value || Learned.Value || Shadow.Value)
					{
						result.Add(new FormattedText(Resource1.VirtuesText + separator));
						result.AddRange(FormattedValueBoolToFormattedText(Noble, Resource1.NobleVirtueText + ". ", "", TextFormat.Regular));
						result.AddRange(FormattedValueBoolToFormattedText(War, Resource1.WarVirtueText + ". ", "", TextFormat.Regular));
						result.AddRange(FormattedValueBoolToFormattedText(Holy, Resource1.HolyVirtueText + ". ", "", TextFormat.Regular));
						result.AddRange(FormattedValueBoolToFormattedText(Learned, Resource1.LearnedVirtueText + ". ", "", TextFormat.Regular));
						result.AddRange(FormattedValueBoolToFormattedText(Shadow, Resource1.ShadowVirtueText + ". ", "", TextFormat.Regular));
						result.Add(new FormattedText("\r\n"));
					}
					break;
				case CardType.Plot:
					result.Add(new FormattedText(Resource1.IncomeText + separator));
					result.AddRange(FormattedValueXIntToFormattedText(Income, TextFormat.Regular));
					result.Add(new FormattedText("\r\n"));
					result.Add(new FormattedText(Resource1.InitiativeText + separator));
					result.AddRange(FormattedValueXIntToFormattedText(Initiative, TextFormat.Regular));
					result.Add(new FormattedText("\r\n"));
					result.Add(new FormattedText(Resource1.ClaimText + separator));
					result.AddRange(FormattedValueXIntToFormattedText(Claim, TextFormat.Regular));
					result.Add(new FormattedText("\r\n"));
					break;
			}

			result.Add(new FormattedText("\r\n"));

			if (Traits != null && !string.IsNullOrEmpty(Traits.Value))
			{
				result.AddRange(FormattedValueStringToFormattedText(Traits, TraitsFormat));
				result.Add(new FormattedText("\r\n"));
			}

			if (Keywords != null && !string.IsNullOrEmpty(Keywords.Value))
			{
				result.AddRange(FormattedValueStringToFormattedText(Keywords, TextFormat.Regular));
				result.Add(new FormattedText("\r\n"));
			}

			if (Text != null && !string.IsNullOrEmpty(Text.Value))
			{
				var cardText = new List<FormattedText>();
				FormatCardText(Text.Value, ref cardText);
				ApplyFormatsToFormattedText(cardText, Text.Formats);
				result.AddRange(cardText);
			}
			if (Type != null && Type.Value != (Int32)CardType.Plot)
			{
				if (Income.Value.IsX || (Income.Value.Value != 0))
				{
					result.Add(new FormattedText("\r\n+"));
					result.AddRange(FormattedValueXIntToFormattedText(Income, TextFormat.Regular));
					result.Add(new FormattedText(" " + Resource1.ProvidedGoldText));
				}

				if (Initiative.Value.IsX || (Initiative.Value.Value != 0))
				{
					result.Add(new FormattedText("\r\n+"));
					result.AddRange(FormattedValueXIntToFormattedText(Initiative, TextFormat.Regular));
					result.Add(new FormattedText(" " + Resource1.ProvidedInitiativeText));
				}

				if (Influence.Value.IsX || (Influence.Value.Value != 0))
				{
					result.Add(new FormattedText("\r\n+"));
					result.AddRange(FormattedValueXIntToFormattedText(Influence, TextFormat.Regular));
					result.Add(new FormattedText(" " + Resource1.ProvidedInfluenceText));
				}
			}

			if (Doomed != null && Doomed.Value)
			{
				result.Add(new FormattedText("\r\n"));
				result.AddRange(FormattedValueBoolToFormattedText(Doomed, Resource1.DoomedText, "", TextFormat.Regular));
				result.Add(new FormattedText("\r\n"));
			}

			if (Multiplayer != null && Multiplayer.Value)
			{
				result.Add(new FormattedText("\r\n"));
				result.AddRange(FormattedValueBoolToFormattedText(Multiplayer, Resource1.MultiplayerText, "", TextFormat.Regular));
				result.Add(new FormattedText("\r\n"));
			}

			// remove the \r\n from the last line
			var lastLine = new FormattedText(result[result.Count - 1].Text.TrimEnd('\r', '\n'), result[result.Count - 1].Format);
			result.RemoveAt(result.Count - 1);
			result.Add(lastLine);
			return result;
		}

		/// <summary>
		/// Formats a string value by adding bold when a trigger (xxx:) is detected, bold italic when a trait (~xxx~) is detected and so on.
		/// </summary>
		/// <param name="text">The text containing the formatting marks.</param>
		/// <param name="formattedText">A list of ordonnated FormattedText elements representing the formatted text.</param>
		private static void FormatCardText(string text, ref List<FormattedText> formattedText)
		{
			if (text.Length == 0)
				return;

			int posBegin, posEnd;

			// look for the trigger formatting (trigger:)
			posEnd = text.IndexOf(':');
			if (posEnd > 0) // not found or first character (the latest is an error case, therefore we don't highlight anything)
			{
				posBegin = Math.Max(text.Substring(0, posEnd).LastIndexOf("\n"),
					Math.Max(text.Substring(0, posEnd).LastIndexOf("("),
						Math.Max(text.Substring(0, posEnd).LastIndexOf("«"),
							Math.Max(text.Substring(0, posEnd).LastIndexOf("“"),
								text.Substring(0, posEnd).LastIndexOf("\""))))) + 1;

				if (((posBegin != 0) && (text[posBegin - 1] == '(')) // we do not want to bold the text (it's probably a "limit:" sequence)
						|| (text[posEnd - 1] == ' ') // or is not a trigger
						|| CardTriggerNames.All(t => posEnd < t.Length || text.Substring(posEnd - t.Length, t.Length) != t))
					formattedText.Add(new FormattedText(text.Substring(0, posEnd + 1)));
				else
				{
					if (posBegin != 0) // add the text before the "xxx:" sequence
						formattedText.Add(new FormattedText(text.Substring(0, posBegin)));
					// add the "xxx:" sequence
					formattedText.Add(new FormattedText(text.Substring(posBegin, posEnd - posBegin + 1), TriggerFormat));
				}
				// add recursively the text after the "xxx: sequence
				if (posEnd + 1 < text.Length)
					FormatCardText(text.Substring(posEnd + 1), ref formattedText); // recursive with the rest of the text
			}
			else
			{
				// look for trait formatting (~trait~)
				posBegin = text.IndexOf('~');
				if (posBegin != -1)
				{
					posEnd = text.IndexOf('~', posBegin + 1);
					if (posEnd != -1) // ~ pair found
					{
						FormatCardText(text.Substring(0, posBegin), ref formattedText);
						formattedText.Add(new FormattedText(text.Substring(posBegin + 1, posEnd - posBegin - 1), TraitsFormat));
						FormatCardText(text.Substring(posEnd + 1), ref formattedText);
						return;
					}
				}
				else
					formattedText.Add(new FormattedText(text));
			}
		}

		/// <summary>
		/// Indicates whether the card could be drawn during a game.
		/// </summary>
		/// <returns>True if the card could be drawn, false otherwise.</returns>
		public override bool IsDrawable()
		{
			return (Type.Value == (Int32)CardType.Attachment) ||
						 (Type.Value == (Int32)CardType.Character) ||
						 (Type.Value == (Int32)CardType.Event) ||
						 (Type.Value == (Int32)CardType.Location);
		}
	}
}