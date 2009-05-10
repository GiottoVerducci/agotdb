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
using System.Data;
using System.Drawing;
using System.Xml;

namespace AGoT.AGoTDB.BusinessObjects
{
  /// <summary>
  /// A pair field structure containing a string and an integer.
  /// </summary>
  public struct TagText { public string Text; public int Tag; public TagText(string text, int tag) { Text = text; Tag = tag; } }

  public class Card
  {
    public Int32 UniversalId;
    public FormattedValue<string> Name, Traits, Keywords, Text, Set, OriginalName;
    public FormattedValue<int> Type, House;
    public FormattedValue<bool> Unique, Doomed, Endless, Military, Intrigue, Power, War, Holy, Noble, Learned, Shadow, Multiplayer;
    public FormattedValue<XInt> Cost, Strength, Income, Initiative, Claim, Influence;

    public int Quantity = 1; // used by decks
    private string fSummaryInfo = ""; // cache for GetSummaryInfo method

    public enum CardType { Unknown = -1, Character = 1, Location = 2, Attachment = 4, Event = 8, Plot = 16, Agenda = 32, Title = 64 };
    public enum CardHouse { Unknown = -1, Neutral = 0, Stark = 1, Lannister = 2, Baratheon = 4, Greyjoy = 8, Martell = 16, Targaryen = 32 };

    public static TextFormat ErrataFormat = new TextFormat("errata", Color.Red);
    public static TextFormat TraitsFormat = new TextFormat("traits", FontStyle.Bold | FontStyle.Italic);
    public static TextFormat TriggerFormat = new TextFormat("trigger", FontStyle.Bold);
    public static List<TagText> CardTypeNames, CardHouseNames; // to convert ids in human strings

    // "Id", "Name", "Type", "House", "Unique", "Traits", "Keywords", "Text", "Doomed", "Endless", "Cost", "Strength",
    // "Military", "Intrigue", "Power", "War", "Holy", "Noble", "Learned", "Shadow", "Income", "Initiative", "Claim",
    // "Influence", "Multiplayer", "Set", "OriginalName", "UniversalId");

    private static List<FormatSection> GetFormat(string style)
    {
      var formats = new List<FormatSection>();

      if (style != "")
      {
        string[] styles = style.Split(';');
        for (var i = 0; i < styles.Length; ++i)
        {
          string[] styleinfo = styles[i].Trim().Split(',', '-');
          formats.Add(new FormatSection(Int32.Parse(styleinfo[1]), Int32.Parse(styleinfo[2]), (styleinfo[0].Trim() == "errata") ? ErrataFormat : TraitsFormat));
        }
      }
      return formats;
    }

    private static List<FormatSection> GetFormatFromErrata(string value)
    {
      bool errated = (value == "YES");
      var formats = new List<FormatSection>();
      if (errated)
        formats.Add(new FormatSection(0, 0, ErrataFormat));
      return formats;
    }

    private static FormattedValue<string> GetTextAndStyleFromRow(DataRow row, string column)
    {
      string text = row[column].ToString();
      string style = row[column + "Style"].ToString().Trim();
      return new FormattedValue<string>(text, GetFormat(style));
    }

    private static FormattedValue<int> GetIntAndStyleFromRow(DataRow row, string column)
    {
      string text = row[column].ToString();
      int value = (text.Trim() == "") ? 0 : Int32.Parse(text);
      string errated = row[column + "Errated"].ToString().Trim();
      return new FormattedValue<int>(value, GetFormatFromErrata(errated));
    }

    private static FormattedValue<XInt> GetXIntAndStyleFromRow(DataRow row, string column)
    {
      string text = row[column].ToString();
      int value = (text.Trim() == "") ? 0 : Int32.Parse(text);
      string errated = row[column + "Errated"].ToString().Trim();
      return new FormattedValue<XInt>((value == -1) ? new XInt() : new XInt(value), GetFormatFromErrata(errated));
    }

    private static FormattedValue<bool> GetBoolAndStyleFromRow(DataRow row, string column)
    {
      bool value = row[column].ToString().ToLowerInvariant() == true.ToString().ToLowerInvariant();
      string errated = row[column + "Errated"].ToString().Trim();
      return new FormattedValue<bool>(value, GetFormatFromErrata(errated));
    }

    public Card(DataRow row)
    {
      InitializeFromDataRow(row);
    }

    private void InitializeFromDataRow(DataRow row)
    {
      UniversalId = Int32.Parse(row["UniversalId"].ToString());
      Name = GetTextAndStyleFromRow(row, "Name");
      Type = GetIntAndStyleFromRow(row, "Type");
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
      House = new FormattedValue<int>(house, GetFormatFromErrata(row["HouseErrated"].ToString()));

      Unique = GetBoolAndStyleFromRow(row, "Unique");
      Traits = GetTextAndStyleFromRow(row, "Traits");
      Keywords = GetTextAndStyleFromRow(row, "Keywords");
      Text = GetTextAndStyleFromRow(row, "Text");
      Doomed = GetBoolAndStyleFromRow(row, "Doomed");
      Endless = GetBoolAndStyleFromRow(row, "Endless");
      Cost = GetXIntAndStyleFromRow(row, "Cost");
      Strength = GetXIntAndStyleFromRow(row, "Strength");
      Military = GetBoolAndStyleFromRow(row, "Military");
      Intrigue = GetBoolAndStyleFromRow(row, "Intrigue");
      Power = GetBoolAndStyleFromRow(row, "Power");
      War = GetBoolAndStyleFromRow(row, "War");
      Holy = GetBoolAndStyleFromRow(row, "Holy");
      Noble = GetBoolAndStyleFromRow(row, "Noble");
      Learned = GetBoolAndStyleFromRow(row, "Learned");
      Shadow = GetBoolAndStyleFromRow(row, "Shadow");
      Income = GetXIntAndStyleFromRow(row, "Income");
      Initiative = GetXIntAndStyleFromRow(row, "Initiative");
      Claim = GetXIntAndStyleFromRow(row, "Claim");
      Influence = GetXIntAndStyleFromRow(row, "Influence");
      Multiplayer = GetBoolAndStyleFromRow(row, "Multiplayer");
      Set = GetTextAndStyleFromRow(row, "Set");
      OriginalName = GetTextAndStyleFromRow(row, "OriginalName");
    }

    public Card(Card original)
    {
      UniversalId = original.UniversalId;
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
      Quantity = original.Quantity;
    }

    /// <summary>
    /// Returns the card text to a plain text string.
    /// </summary>
    /// <returns>The card text in plain text.</returns>
    public string ToPlainFullString()
    {
      //String result = ("Nom: " + Name.Value + " " + ReportBrackets(unique, (unique.Contains("Yes") ? "*" : ""))).Trim() + "\r\n";
      String result = String.Format("{0}: {1}", Resource1.NameText, Name.Value); //, (Unique.Value)? "*" : "");
      List<FormattedText> formatted = ToFormattedString();
      for (var i = 1; i < formatted.Count; ++i) // start at index 1 to skip the name part
        if ((formatted[i].Text != "\r\n") || ((i + 1 < formatted.Count) && (formatted[i + 1].Text != "\r\n")))
          result += formatted[i].Text;
      return result;
    }

    public override string ToString()
    {
      return Name.Value;
    }

    /// <summary>
    /// Returns the set (abbreviated) in which the card first appeared.
    /// </summary>
    /// <returns>The set as a string.</returns>
    public string GetShortSet()
    {
      return Set.Value.Substring(0, Set.Value.IndexOf('('));
    }
    /// <summary>
    /// Returns a string giving a summary of the main characteristics of the card.
    /// </summary>
    /// <returns>The summary.</returns>
    public string GetSummaryInfo()
    {
      if (fSummaryInfo != "")
        return fSummaryInfo;
      if (Type == null) // for virtual cards
        return "";
      switch ((CardType)Type.Value)
      {
        case CardType.Plot:
          fSummaryInfo = String.Format("{0,2} {1,2} {2,2}", Income.Value, Initiative.Value, Claim.Value);
          if (Traits.Value != "")
            fSummaryInfo += " - " + Traits.Value;
          break;
        case CardType.Attachment:
          fSummaryInfo = String.Format("[{0}]", Cost.Value);
          if (Keywords.Value != "")
            fSummaryInfo += " - " + Keywords.Value;
          break;
        case CardType.Character:
          fSummaryInfo = String.Format("[{0}] {1,2} {2} {3} {4}", Cost.Value, Strength.Value, (Military.Value) ? Resource1.MilitaryIconAbrev[0] : ' ', (Intrigue.Value) ? Resource1.IntrigueIconAbrev[0] : ' ', (Power.Value) ? Resource1.PowerIconAbrev[0] : ' ');
          if (Traits.Value != "")
            fSummaryInfo += " - " + Traits.Value;
          if (Noble.Value) fSummaryInfo += String.Format(" [{0}]", Resource1.NobleVirtueText);
          if (War.Value) fSummaryInfo += String.Format(" [{0}]", Resource1.WarVirtueText);
          if (Holy.Value) fSummaryInfo += String.Format(" [{0}]", Resource1.HolyVirtueText);
          if (Learned.Value) fSummaryInfo += String.Format(" [{0}]", Resource1.LearnedVirtueText);
          if (Shadow.Value) fSummaryInfo += String.Format(" [{0}]", Resource1.ShadowVirtueText);
          if (Keywords.Value != "")
            fSummaryInfo += " - " + Keywords.Value;
          break;
        case CardType.Event:
          if (Cost.Value.IsNonZero())  // for old events with a gold cost
            fSummaryInfo = String.Format("[{0}]", Cost.Value);
          if (Traits.Value != "")
            fSummaryInfo += " - " + Traits.Value;
          if (Keywords.Value != "")
            fSummaryInfo += " - " + Keywords.Value;
          break;
        case CardType.Location:
          fSummaryInfo = String.Format("[{0}]", Cost.Value);
          fSummaryInfo += (Income.Value.IsNonZero()) ? ("+" + Income.Value) : "  ";
          fSummaryInfo += (Influence.Value.IsNonZero()) ? String.Format("|{0}|", Influence.Value) : "   ";
          fSummaryInfo += (Initiative.Value.IsNonZero()) ? String.Format("<{0}>", Initiative.Value) : "   ";
          if (Traits.Value != "")
            fSummaryInfo += " - " + Traits.Value;
          if (Keywords.Value != "")
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
    /// Gets the combinaison of two TextFormat objects.
    /// </summary>
    /// <param name="format1">The first format.</param>
    /// <param name="format2">The second format.</param>
    /// <returns>The combinaison of the two formats.</returns>
    private static TextFormat MergeTextFormat(TextFormat format1, TextFormat format2)
    {
      if (format1 == TextFormat.Regular)
        return format2;
      if (format2 == TextFormat.Regular)
        return format1;
      return new TextFormat(format1.Name + "+" + format2.Name,
        format1.Style | format2.Style, // merge styles
        (format2.Color == TextFormat.DefaultColor) ? format1.Color : format2.Color); // merge colors with the following rule : color2 is prefered to color1 if color2 is not the default color
    }

    /// <summary>
    /// Applies formats to list of FormattedText elements representing a formatted text. The formats are applied
    /// on portions of the formatted text (the begin and end indexes are stored in each FormatSection element),
    /// which means that a formatted element may be splitted to apply a format on a part of it.
    /// Eg. A plain text element "babar" on which a format 2-3 bold is applied will result in a list of 3
    /// FormattedText elements : "ba" (plain) + "ba" (bold) + "r" (plain).
    /// </summary>
    /// <param name="result">The list representing the text on which the formats are applied.</param>
    /// <param name="formats">The list of formats to apply.</param>
    private static void ApplyFormatsToFormattedText(List<FormattedText> result, IList<FormatSection> formats)
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
                                          MergeTextFormat(result[j].Format, formats[i].Format)));
          if (relEnd != result[j].Text.Length)
            subResult.Add(new FormattedText(result[j].Text.Substring(relEnd, result[j].Text.Length - relEnd), result[j].Format));

          // we replace the old element by the new splitted one
          result.RemoveAt(j);
          result.InsertRange(j, subResult);

          // we have to adjust the indexes if the we carry on the next part
          if (formatAppliesToNext)
          {
            if (relBegin != 0) { pos += result[j].Text.Length; ++j; }
            pos += result[j].Text.Length; ++j;
          }
        }
        while (formatAppliesToNext);
      }
    }

    private static List<FormattedText> FormattedValueStringToFormattedText(FormattedValue<string> value, TextFormat defaultFormat)
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

    private static List<FormattedText> FormattedValueBoolToFormattedText(FormattedValue<bool> value, string yesValue, string noValue, TextFormat defaultFormat)
    {
      var result = new List<FormattedText>();
      if (value != null)
      {
        if (value.Formats.Count > 0)
          result.Add(new FormattedText(value.Value ? yesValue : noValue, MergeTextFormat(value.Formats[0].Format, defaultFormat)));
        else
          result.Add(new FormattedText(value.Value ? yesValue : noValue, defaultFormat));
      }
      return result;
    }

    private static List<FormattedText> FormattedValueXIntToFormattedText(FormattedValue<XInt> value, TextFormat defaultFormat)
    {
      var result = new List<FormattedText>();
      if (value != null)
      {
        if (value.Formats.Count > 0)
          result.Add(new FormattedText(value.Value.ToString(), MergeTextFormat(value.Formats[0].Format, defaultFormat)));
        else
          result.Add(new FormattedText(value.Value.ToString(), defaultFormat));
      }
      return result;
    }

    public static string GetTextFromList(List<TagText> tt, Int32 id)
    {
      var index = tt.FindIndex(t => (t.Tag == id));
      return index != -1 ? tt[index].Text : "";
    }

    /// <summary>
    /// Searches the CardTypeNames list of types for the type with the same id in its tag and returns the associated string.
    /// The CardTypeNames list must have been already initialized.
    /// </summary>
    /// <param name="type">The integer value representing the type.</param>
    /// <returns>The string associated to the type.</returns>
    public static string GetTypeName(Int32 type)
    {
      return GetTextFromList(CardTypeNames, type);
    }

    /// <summary>
    /// Searches the CardHouseNames list of types for the houses with the same id (bitwise) in its tag and returns the associated string.
    /// The CardHouseNames list must have been already initialized.
    /// </summary>
    /// <param name="house">The integer value representing the house.</param>
    /// <returns>The string associated to the house.</returns>
    public static string GetHouseName(Int32 house)
    {
      if (house == 0)
        return GetTextFromList(CardHouseNames, 0);
      string result = "";
      int id = 1;
      while (house > 0)
      {
        if ((house & 1) == 1)
          result += GetTextFromList(CardHouseNames, id) + "/";
        id = id << 1;
        house = house >> 1;
      }
      return result.Substring(0, result.Length - 1); // return without the last '/'
    }

    /// <summary>
    /// Gets a list of ordonnated formatted elements representing this card.
    /// </summary>
    /// <returns>A list of FormattedText elements containing the informations about this card.</returns>
    public List<FormattedText> ToFormattedString()
    {
      var result = new List<FormattedText>();

      var nameFormat = new TextFormat("name", FontStyle.Bold);

      result.AddRange(FormattedValueStringToFormattedText(Name, nameFormat));
      result.AddRange(FormattedValueBoolToFormattedText(Unique, " *", "", nameFormat));
      result.Add(new FormattedText("\r\n"));

      result.Add(new FormattedText(Resource1.SetText + ": "));
      result.AddRange(FormattedValueStringToFormattedText(Set, TextFormat.Regular));
      result.Add(new FormattedText("\r\n"));

      if (Type != null)
      {
        result.Add(new FormattedText(Resource1.TypeText + ": "));
        result.Add(new FormattedText(GetTypeName(Type != null ? Type.Value : (Int32)CardType.Unknown), (Type != null && Type.Formats.Count > 0) ? Type.Formats[0].Format : TextFormat.Regular));
        result.Add(new FormattedText("\r\n"));
      }
      switch (Type != null ? (CardType)Type.Value : CardType.Unknown)
      {
        case CardType.Location:
        case CardType.Attachment:
          result.Add(new FormattedText(Resource1.HouseText + ": "));
          result.Add(new FormattedText(GetHouseName(House.Value), (House.Formats.Count > 0) ? House.Formats[0].Format : TextFormat.Regular));
          result.Add(new FormattedText("\r\n"));
          result.Add(new FormattedText(Resource1.CostText + ": "));
          result.AddRange(FormattedValueXIntToFormattedText(Cost, TextFormat.Regular));
          result.Add(new FormattedText("\r\n"));
          break;
        case CardType.Event:
          result.Add(new FormattedText(Resource1.HouseText + ": "));
          result.Add(new FormattedText(GetHouseName(House.Value), (House.Formats.Count > 0) ? House.Formats[0].Format : TextFormat.Regular));
          result.Add(new FormattedText("\r\n"));
          break;
        case CardType.Character:
          result.Add(new FormattedText(Resource1.HouseText + ": "));
          result.Add(new FormattedText(GetHouseName(House.Value), (House.Formats.Count > 0) ? House.Formats[0].Format : TextFormat.Regular));
          result.Add(new FormattedText("\r\n"));
          result.Add(new FormattedText(Resource1.CostText + ": "));
          result.AddRange(FormattedValueXIntToFormattedText(Cost, TextFormat.Regular));
          result.Add(new FormattedText("\r\n"));
          result.Add(new FormattedText(Resource1.StrengthText + ": "));
          result.AddRange(FormattedValueXIntToFormattedText(Strength, TextFormat.Regular));
          result.Add(new FormattedText("\r\n"));
          result.Add(new FormattedText(Resource1.IconsText + ": "));
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
            result.Add(new FormattedText(Resource1.VirtuesText + ": "));
            result.AddRange(FormattedValueBoolToFormattedText(Noble, Resource1.NobleVirtueText + ". ", "", TextFormat.Regular));
            result.AddRange(FormattedValueBoolToFormattedText(War, Resource1.WarVirtueText + ". ", "", TextFormat.Regular));
            result.AddRange(FormattedValueBoolToFormattedText(Holy, Resource1.HolyVirtueText + ". ", "", TextFormat.Regular));
            result.AddRange(FormattedValueBoolToFormattedText(Learned, Resource1.LearnedVirtueText + ". ", "", TextFormat.Regular));
            result.AddRange(FormattedValueBoolToFormattedText(Shadow, Resource1.ShadowVirtueText + ". ", "", TextFormat.Regular));
            result.Add(new FormattedText("\r\n"));
          }
          break;
        case CardType.Plot:
          result.Add(new FormattedText(Resource1.IncomeText + ": "));
          result.AddRange(FormattedValueXIntToFormattedText(Income, TextFormat.Regular));
          result.Add(new FormattedText("\r\n"));
          result.Add(new FormattedText(Resource1.InitiativeText + ": "));
          result.AddRange(FormattedValueXIntToFormattedText(Initiative, TextFormat.Regular));
          result.Add(new FormattedText("\r\n"));
          result.Add(new FormattedText(Resource1.ClaimText + ": "));
          result.AddRange(FormattedValueXIntToFormattedText(Claim, TextFormat.Regular));
          result.Add(new FormattedText("\r\n"));
          break;
      }

      result.Add(new FormattedText("\r\n"));

      if (Traits != null && Traits.Value != "")
      {
        result.AddRange(FormattedValueStringToFormattedText(Traits, TraitsFormat));
        result.Add(new FormattedText("\r\n"));
      }

      if (Keywords != null && Keywords.Value != "")
      {
        result.AddRange(FormattedValueStringToFormattedText(Keywords, TextFormat.Regular));
        result.Add(new FormattedText("\r\n"));
      }

      if (Text != null && Text.Value != "")
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
                                              text.Substring(0, posEnd).LastIndexOf("\"")))) + 1;

        if (((posBegin != 0) && (text[posBegin - 1] == '(')) || // we do not want to bold the text (it's probably a "limit:" sequence)
            (text[posEnd - 1] == ' ')) // or is not a trigger
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
    /// Compares two cards by comparing their universal id and their quantity.
    /// If both references are null, the method returns true.
    /// If one reference is null and the other isn't, the method returns false.
    /// </summary>
    /// <param name="card1">The first card.</param>
    /// <param name="card2">The second card.</param>
    /// <returns>True if the two cards are identical by content or both null, false otherwise.</returns>
    public static bool AreEqual(Card card1, Card card2)
    {
      return (card1 == null && card2 == null) ||
             (card1 != null && card2 != null && card1.UniversalId == card2.UniversalId && card1.Quantity == card2.Quantity);
    }

    /// <summary>
    /// Indicates whether the card could be drawn during a game.
    /// </summary>
    /// <returns>True if the card could be drawn, false otherwise.</returns>
    public bool IsDrawable()
    {
      return (Type.Value == (Int32)CardType.Attachment) ||
             (Type.Value == (Int32)CardType.Character) ||
             (Type.Value == (Int32)CardType.Event) ||
             (Type.Value == (Int32)CardType.Location);
    }

    /// <summary>
    /// Gets the XML representation of this card.
    /// </summary>
    /// <param name="doc">The XmlDocument for which the XML representation is generated.</param>
    /// <returns>A XMLElement representing this card.</returns>
    public XmlElement ToXml(XmlDocument doc)
    {
      XmlElement value = doc.CreateElement("Card");
      XmlToolBox.AddElementValue(doc, value, "UniversalId", UniversalId.ToString());
      XmlToolBox.AddElementValue(doc, value, "Quantity", Quantity.ToString());
      XmlToolBox.AddElementValue(doc, value, "Name", Name.Value);
      XmlToolBox.AddElementValue(doc, value, "Set", Set.Value);
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
    public Card(XmlDocument doc, XmlNode root)
    {
      UniversalId = Int32.Parse(XmlToolBox.GetElementValue(doc, root, "UniversalId"));
      DataTable table = DatabaseInterface.Singleton.GetResultFromRequest(
        String.Format("SELECT * FROM [{0}] WHERE UniversalId = :universalId", DatabaseInterface.TableName.Main),
        new CommandParameters().Add("universalId", UniversalId));
      if (table.Rows.Count <= 0)
      {
        var recordedName = XmlToolBox.GetElementValue(doc, root, "Name");
        var recordedSet = XmlToolBox.GetElementValue(doc, root, "Set");
        // try to match the card with its name and expansion number (for cards that were merged in previous versions)
        table = DatabaseInterface.Singleton.GetResultFromRequest(
          String.Format("SELECT * FROM [{0}] WHERE Name = :name AND Set LIKE '%' + :set + '%'", DatabaseInterface.TableName.Main),
          new CommandParameters().Add("name", recordedName)
          .Add("set", recordedSet));
        if (table.Rows.Count <= 0)
        {
          Name = new FormattedValue<string>(recordedName, new List<FormatSection>());
          Set = new FormattedValue<string>(recordedSet, new List<FormatSection>());
          Text = new FormattedValue<string>(Resource1.UnknownCard, new List<FormatSection>());
        }
      }

      if (table.Rows.Count > 0)
        InitializeFromDataRow(table.Rows[0]);
      Quantity = Int32.Parse(XmlToolBox.GetElementValue(doc, root, "Quantity"));
    }

    /// <summary>
    /// Returns the comparaison relationship between this card and another card.
    /// Both cards must have the same type, otherwise a plain name comparaison is done.
    /// </summary>
    /// <param name="otherCard">The card to compare this card to.</param>
    /// <returns>-1 if this card is before the other card, 0 if they're equal, 1 otherwise</returns>
    public int CompareOrder(Card otherCard)
    {
      if (otherCard.Type != null && Type != null && otherCard.Type.Value == Type.Value)
      {
        switch (Type.Value)
        {
          case (Int32)CardType.Character:
          case (Int32)CardType.Location:
          case (Int32)CardType.Attachment: return MultipleCompare(Cost.Value.CompareTo(otherCard.Cost.Value), Name.Value.CompareTo(otherCard.Name.Value));
          default:
          case (Int32)CardType.Event:
          case (Int32)CardType.Agenda:
          case (Int32)CardType.Title: return Name.Value.CompareTo(otherCard.Name.Value);
          case (Int32)CardType.Plot: return MultipleCompare(Claim.Value.CompareTo(otherCard.Claim.Value), Income.Value.CompareTo(otherCard.Income.Value), Name.Value.CompareTo(otherCard.Name.Value));
        }
      }
      return Name.Value.CompareTo(otherCard.Name.Value);
    }

    /// <summary>
    /// Synthesize the result of multiple comparisons by returning -1
    /// if the first non-0 comparaison result is negative, 1 if the first non-0
    /// comparaison result is positive, and 0 if all comparaison results are 0.
    /// </summary>
    /// <param name="compResults">the list of results</param>
    /// <returns>-1, 0 or 1</returns>
    private static int MultipleCompare(params int[] compResults)
    {
      for (var i = 0; i < compResults.Length; ++i)
        if (compResults[i] != 0)
          return Math.Sign(compResults[i]);
      return 0;
    }
  }
}