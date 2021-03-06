// AndroidDB - A card searcher and deck builder tool for the LCG "Netrunner Android"
// Copyright © 2013 Vincent Ripoll
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
// © Fantasy Flight Games 2012


using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Xml;
using AndroidDB;
using GenericDB;
using GenericDB.BusinessObjects;
using GenericDB.DataAccess;

namespace NRADB.BusinessObjects
{
    public class NraCard : Card
    {
        public FormattedValue<string> Name, Subtitle, Keywords, Text, Set, OriginalName, Flavor, Instructions;
        public FormattedValue<int> Type, Faction, Influence, Side, IceType;
        public FormattedValue<bool> Unique, Banned, Restricted;
        public FormattedValue<XInt> Cost, Requirement, MU, DeckSize, Stat, Strength, AgendaPoints, Link, TrashCost;

        private string _summaryInfo = ""; // cache for GetSummaryInfo method

        public enum CardType { Unknown = -1, None = 0, Identity = 1, Agenda = 2, Hardware = 4, Ice = 8, Program = 16, Resource = 32, Upgrade = 64, Asset = 128, Operation = 256, Event = 512 };
        public enum CardFaction { Unknown = -1, Neutral = 0, Anarch = 1, Criminal = 2, Shaper = 4, HaasBioroid = 8, Jinteki = 16, NBN = 32, Weyland = 64 };
        public enum CardSide { Unknown = -1, None = 0, Runner = 1, Corp = 2 };
        public enum Pattern { Unknown = -1, XOnly = 0, CostsInfluence = 1 };

        public static readonly TextFormat ErrataFormat = new TextFormat("errata", Color.Red);
        public static readonly TextFormat KeywordsFormat = new TextFormat("keywords", FontStyle.Bold | FontStyle.Italic);
        public static readonly TextFormat TriggerFormat = new TextFormat("trigger", FontStyle.Bold);
        public static readonly TextFormat BannedFormat = new TextFormat("banned", FontStyle.Bold, Color.Red);
        public static readonly TextFormat RestrictedFormat = new TextFormat("restricted", FontStyle.Bold, Color.OrangeRed);
        private static readonly RowFormattedDataExtractor _rowFormattedDataExtractor =
            new RowFormattedDataExtractor(ErrataFormat, KeywordsFormat, TriggerFormat);
        public static IDictionary<int, string> CardTypeNames, CardFactionNames, CardSideNames;
        public static IDictionary<int, int> CardTypeSide, CardFactionSide;
        public static IList<string> CardTriggerNames;
        public static IDictionary<string, bool> ExpansionSets;
        public static IDictionary<Pattern, string> CardPatterns;
        /// <summary>
        /// The dictionary containing for each short set name the list of chapters names, joined with commas ", "
        /// </summary>
        public static IDictionary<string, string> ChaptersNames;

        // "Id", "Name", "Subtitle", "Type", "Side", "Faction", "Unique", "Traits", "Keywords", "Text", "Instructions",
        // "Cost", "Stat", "Influence", "Requirement", "Set", "OriginalName", "UniversalId", "Banned", "Restricted", "Flavor");

        #region Constructors and clone
        public NraCard()
        {
            Quantity = 1;
        }

        public NraCard(DataRow row)
            : this()
        {
            InitializeFromDataRow(row);
        }

        private void InitializeFromDataRow(DataRow row)
        {
            var rfde = _rowFormattedDataExtractor; // alias

            UniversalId = (Int32)row["UniversalId"];
            Name = rfde.GetTextAndStyleFromRow(row, "Name");
            Subtitle = rfde.GetTextAndStyleFromRow(row, "Subtitle");
            Type = rfde.GetIntAndStyleFromRow(row, "Type");
            Side = rfde.GetIntAndStyleFromRow(row, "Side");
            int faction = 0;
            if ((Boolean)row["FactionAnarch"])
                faction += (int)CardFaction.Anarch;
            if ((Boolean)row["FactionCriminal"])
                faction += (int)CardFaction.Criminal;
            if ((Boolean)row["FactionShaper"])
                faction += (int)CardFaction.Shaper;
            if ((Boolean)row["FactionHaasBioroid"])
                faction += (int)CardFaction.HaasBioroid;
            if ((Boolean)row["FactionJinteki"])
                faction += (int)CardFaction.Jinteki;
            if ((Boolean)row["FactionNBN"])
                faction += (int)CardFaction.NBN;
            if ((Boolean)row["FactionWeyland"])
                faction += (int)CardFaction.Weyland;
            Faction = new FormattedValue<int>(faction, rfde.GetFormatFromErrata(row["FactionErrated"].ToString()));

            Unique = rfde.GetBoolAndStyleFromRow(row, "Unique");
            Keywords = rfde.GetTextAndStyleFromRow(row, "Keywords");
            Text = rfde.GetTextAndStyleFromRow(row, "Text");
            Instructions = rfde.GetTextAndStyleFromRow(row, "Instructions");
            Cost = rfde.GetXIntAndStyleFromRow(row, "Cost");
            Stat = rfde.GetXIntAndStyleFromRow(row, "Stat");
            Strength = rfde.GetXIntAndStyleFromRow(row, "Strength");
            AgendaPoints = rfde.GetXIntAndStyleFromRow(row, "AgendaPoints");
            Link = rfde.GetXIntAndStyleFromRow(row, "Link");
            TrashCost = rfde.GetXIntAndStyleFromRow(row, "TrashCost");
            Influence = rfde.GetIntAndStyleFromRow(row, "Influence");
            Requirement = rfde.GetXIntAndStyleFromRow(row, "Requirement");
            MU = rfde.GetXIntAndStyleFromRow(row, "MU");
            DeckSize = rfde.GetXIntAndStyleFromRow(row, "DeckSize");
            Set = rfde.GetTextAndStyleFromRow(row, "Set");
            OriginalName = rfde.GetTextAndStyleFromRow(row, "OriginalName");
            Banned = rfde.GetBoolAndStyleFromRow(row, "Banned");
            Restricted = rfde.GetBoolAndStyleFromRow(row, "Restricted");
            OctgnId = rfde.GetGuidFromRow(row, "OctgnId");
            Flavor = rfde.GetTextAndStyleFromRow(row, "Flavor");
            IceType = rfde.GetIntAndStyleFromRow(row, "IceType");
        }

        private NraCard(NraCard original)
            : this()
        {
            UniversalId = original.UniversalId;
            Quantity = original.Quantity;
            Name = original.Name;
            Subtitle = original.Subtitle;
            Type = original.Type;
            Side = original.Side;
            Faction = original.Faction;
            Unique = original.Unique;
            Keywords = original.Keywords;
            Text = original.Text;
            Instructions = original.Instructions;
            Cost = original.Cost;
            Stat = original.Stat;
            Strength = original.Strength;
            AgendaPoints = original.AgendaPoints;
            Link = original.Link;
            TrashCost = original.TrashCost;
            Influence = original.Influence;
            Requirement = original.Requirement;
            MU = original.MU;
            DeckSize = original.DeckSize;
            Set = original.Set;
            OriginalName = original.OriginalName;
            Banned = original.Banned;
            Restricted = original.Restricted;
            OctgnId = original.OctgnId;
            Flavor = original.Flavor;
            IceType = original.IceType;
        }

        /// <summary>
        /// Clones this card object by copying all its properties. This is a deep copy.
        /// </summary>
        /// <returns>A clone object of this object.</returns>
        public override ICard Clone()
        {
            return new NraCard(this);
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

            Func<int> universalIdGetter = () => Int32.Parse(XmlToolbox.GetElementValue(doc, root, "UniversalId"), CultureInfo.InvariantCulture);
            Func<string> recordedNameGetter = () => XmlToolbox.GetElementValue(doc, root, "Name");
            Func<string> recordedSetGetter = () => XmlToolbox.GetElementValue(doc, root, "Set");
            Func<int> quantityGetter = () => Int32.Parse(XmlToolbox.GetElementValue(doc, root, "Quantity"), CultureInfo.InvariantCulture);

            InitializeFromValues(universalIdGetter, recordedNameGetter, recordedSetGetter, quantityGetter);
        }

        public void InitializeFromValues(Func<int> universalIdGetter, Func<string> recordedNameGetter, Func<string> recordedSetGetter, Func<int> quantityGetter)
        {
            this.UniversalId = universalIdGetter();
            var table = ApplicationSettings.Instance.DatabaseManager.GetCardFromUniversalId(this.UniversalId);
            if (table.Rows.Count <= 0)
            {
                var recordedName = recordedNameGetter();
                var recordedSet = recordedSetGetter();
                // try to match the card with its name and expansion number (for cards that were merged in previous versions)
                table = ApplicationSettings.Instance.DatabaseManager.GetCardFromNameAndSet(recordedName, recordedSet);
                if (table.Rows.Count <= 0)
                {
                    Name = new FormattedValue<string>(recordedName, new List<FormatSection>());
                    Set = new FormattedValue<string>(recordedSet, new List<FormatSection>());
                    Text = new FormattedValue<string>(Resource1.UnknownCard, new List<FormatSection>());
                }
            }

            if (table.Rows.Count > 0)
                InitializeFromDataRow(table.Rows[0]);
            Quantity = quantityGetter();
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

            var otherNraCard = (NraCard)otherCard;
            if (otherNraCard.Type != null && Type != null && otherNraCard.Type.Value == Type.Value)
            {
                switch ((CardType)Type.Value)
                {
                    case CardType.Ice:
                        return Tools.MultipleCompare(IceType.Value.CompareTo(otherNraCard.IceType.Value),
                            Cost.Value.CompareTo(otherNraCard.Cost.Value),
                            string.Compare(Name.Value, otherNraCard.Name.Value, StringComparison.CurrentCulture));
                    case CardType.Agenda:
                        return Tools.MultipleCompare(AgendaPoints.Value.CompareTo(otherNraCard.AgendaPoints.Value),
                            string.Compare(Name.Value, otherNraCard.Name.Value, StringComparison.CurrentCulture));
                    case CardType.Asset:
                    case CardType.Hardware:
                    case CardType.Event:
                    case CardType.Operation:
                    case CardType.Resource:
                        return Tools.MultipleCompare(Cost.Value.CompareTo(otherNraCard.Cost.Value),
                            string.Compare(Name.Value, otherNraCard.Name.Value, StringComparison.CurrentCulture));
                    case CardType.Program:
                        return Tools.MultipleCompare(MU.Value.CompareTo(otherNraCard.MU.Value),
                            Cost.Value.CompareTo(otherNraCard.Cost.Value),
                            string.Compare(Name.Value, otherNraCard.Name.Value, StringComparison.CurrentCulture));

                    //    case CardType.Agenda:
                    //    case CardType.Location:
                    //    case CardType.Attachment: return Tools.MultipleCompare(
                    //        Shadow.Value.CompareTo(otherAgotCard.Shadow.Value),
                    //        Cost.Value.CompareTo(otherAgotCard.Cost.Value),
                    //        string.Compare(Name.Value, otherAgotCard.Name.Value, StringComparison.CurrentCulture));
                    //    default:
                    //    case CardType.Event:
                    //    case CardType.Agenda:
                    //    case CardType.Title: return string.Compare(Name.Value, otherAgotCard.Name.Value, StringComparison.CurrentCulture);
                    //    case CardType.Plot: return Tools.MultipleCompare(
                    //        Claim.Value.CompareTo(otherAgotCard.Claim.Value),
                    //        Income.Value.CompareTo(otherAgotCard.Income.Value),
                    //        string.Compare(Name.Value, otherAgotCard.Name.Value, StringComparison.CurrentCulture));
                }
            }
            return Name.Value.CompareTo(otherNraCard.Name.Value);
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
            var card = obj as NraCard;
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
            var result = String.Format(CultureInfo.CurrentCulture, "{0}{1}{2}{3}",
                Resource1.NameText, Resource1.SeparatorText, Name.Value, !string.IsNullOrEmpty(Subtitle.Value) ? " - " + Subtitle.Value : null);
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
        /// Returns the name of the card and its summary.
        /// </summary>
        public string Description { get { return String.Format("{0} {1}", Name.Value, GetSummaryInfo()); } }

        /// <summary>
        /// Returns the set (abbreviated) in which the card first appeared.
        /// </summary>
        /// <returns>The set as a string.</returns>
        public override string GetShortSet()
        {
            var chapteredSets = GetChapteredSets();
            return chapteredSets.Substring(0, chapteredSets.IndexOf('('));
        }

        /// <summary>
        /// Returns a string giving a summary of the main characteristics of the card.
        /// </summary>
        /// <returns>The summary.</returns>
        public override string GetSummaryInfo()
        {
            if (_summaryInfo != "")
                return _summaryInfo;
            if (Type == null) // for virtual cards
                return "";
            switch ((CardType)Type.Value)
            {
                case CardType.Agenda:
                    _summaryInfo = String.Format(CultureInfo.CurrentCulture, "[{0}] {1,2}", Cost.Value, Stat.Value);
                    if (!string.IsNullOrEmpty(Keywords.Value))
                        _summaryInfo += " - " + Keywords.Value;
                    break;
                case CardType.Upgrade:
                case CardType.Resource:
                case CardType.Hardware:
                case CardType.Asset:
                case CardType.Operation:
                case CardType.Event:
                    _summaryInfo = String.Format(CultureInfo.CurrentCulture, "[{0}]", Cost.Value);
                    if (!string.IsNullOrEmpty(Keywords.Value))
                        _summaryInfo += " - " + Keywords.Value;
                    break;
                case CardType.Program:
                    _summaryInfo = String.Format(CultureInfo.CurrentCulture, "[{0}] {1,2}", Cost.Value, MU.Value);
                    if (!string.IsNullOrEmpty(Keywords.Value))
                        _summaryInfo += " - " + Keywords.Value;
                    break;
                case CardType.Ice:
                    _summaryInfo = String.Format(CultureInfo.CurrentCulture, "[{0}] {1,2}", Cost.Value, Stat.Value);
                    if (!string.IsNullOrEmpty(Keywords.Value))
                        _summaryInfo += " - " + Keywords.Value;
                    break;
                //case CardType.Identity:
                //    if (!string.IsNullOrEmpty(Traits.Value))
                //        _summaryInfo += " - " + Traits.Value;
                //    if (Shadow.Value) _summaryInfo += String.Format(" [{0}]", Resource1.ShadowVirtueText);
                //    if (!string.IsNullOrEmpty(Keywords.Value))
                //        _summaryInfo += " - " + Keywords.Value;
                //    break;
                default: _summaryInfo = ""; break;
            }
            _summaryInfo = _summaryInfo.Trim();
            //if (Doomed.Value)
            //    _summaryInfo += " " + Resource1.DoomedText;
            return _summaryInfo;
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
            return type != (Int32)CardType.Unknown
                ? CardTypeNames[type]
                : Resource1.UnknownCardType;
        }

        /// <summary>
        /// Searches the CardFactionNames dictionary of factions for the factions with the same id (bitwise) in its tag and returns the associated string.
        /// The CardFactionNames dictionary must have been already initialized.
        /// All factions must be present in the dictionary, otherwise an exception is thrown.
        /// </summary>
        /// <param name="faction">The integer value representing the factions.</param>
        /// <returns>The string associated to the factions (separated by '/' if more than one faction is present).</returns>
        public static string GetFactionName(Int32 faction)
        {
            if (faction == 0) // neutral faction
                return CardFactionNames[0];
            var factionNames = from f in CardFactionNames where (f.Key & faction) != 0 select f.Value;
            return string.Join("/", factionNames.ToArray());
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
            var subtitleFormat = new TextFormat("subtitle", FontStyle.Bold | FontStyle.Italic);

            var type = Type != null ? (CardType)Type.Value : CardType.Unknown;

            result.AddRange(FormattedValueStringToFormattedText(Name, nameFormat));
            result.AddRange(FormattedValueBoolToFormattedText(Unique, " *", "", nameFormat));
            result.Add(new FormattedText("\r\n"));

            if (Subtitle != null && !string.IsNullOrEmpty(Subtitle.Value))
            {
                result.AddRange(FormattedValueStringToFormattedText(Subtitle, subtitleFormat));
                result.Add(new FormattedText("\r\n"));
            }

            result.Add(new FormattedText(Resource1.SetText + separator));
            result.Add(new FormattedText(GetChapteredSets()));
            result.Add(new FormattedText("\r\n"));

            if (Type != null)
            {
                result.Add(new FormattedText(Resource1.TypeText + separator));
                result.Add(new FormattedText(GetTypeName(Type.Value), (Type.Formats.Count > 0) ? Type.Formats[0].Format : TextFormat.Regular));
                result.Add(new FormattedText("\r\n"));
            }

            result.Add(new FormattedText(Resource1.FactionText + separator));
            result.Add(new FormattedText(GetFactionName(Faction.Value), (Faction.Formats.Count > 0) ? Faction.Formats[0].Format : TextFormat.Regular));
            result.Add(new FormattedText("\r\n"));

            if (type != CardType.Identity && Influence != null && Influence.Value > 0)
            {
                result.Add(new FormattedText(Resource1.InfluenceText + separator));
                result.Add(new FormattedText(new string('•', Influence.Value), (Influence.Formats.Count > 0) ? Influence.Formats[0].Format : TextFormat.Regular));
                result.Add(new FormattedText("\r\n"));
            }

            switch (type)
            {
                case CardType.Hardware:
                case CardType.Resource:
                case CardType.Upgrade:
                case CardType.Asset:
                case CardType.Operation:
                case CardType.Event:
                    result.Add(new FormattedText(Resource1.CostText + separator));
                    result.AddRange(FormattedValueXIntToFormattedText(Cost, TextFormat.Regular));
                    result.Add(new FormattedText("\r\n"));
                    break;
                case CardType.Agenda:
                    result.Add(new FormattedText(Resource1.AdvancementText + separator));
                    result.AddRange(FormattedValueXIntToFormattedText(Cost, TextFormat.Regular));
                    result.Add(new FormattedText("\r\n"));
                    result.Add(new FormattedText(Resource1.AgendaPointsText + separator));
                    result.AddRange(FormattedValueXIntToFormattedText(AgendaPoints, TextFormat.Regular));
                    result.Add(new FormattedText("\r\n"));
                    break;
                case CardType.Ice:
                    result.Add(new FormattedText(Resource1.CostText + separator));
                    result.AddRange(FormattedValueXIntToFormattedText(Cost, TextFormat.Regular));
                    result.Add(new FormattedText("\r\n"));
                    if (Strength != null && Strength.Value.IsSet)
                    {
                        result.Add(new FormattedText(Resource1.StrengthText + separator));
                        result.AddRange(FormattedValueXIntToFormattedText(Strength, TextFormat.Regular));
                        result.Add(new FormattedText("\r\n"));
                    }
                    break;
                case CardType.Program:
                    result.Add(new FormattedText(Resource1.CostText + separator));
                    result.AddRange(FormattedValueXIntToFormattedText(Cost, TextFormat.Regular));
                    result.Add(new FormattedText("\r\n"));
                    result.Add(new FormattedText(Resource1.MuText + separator));
                    result.AddRange(FormattedValueXIntToFormattedText(MU, TextFormat.Regular));
                    result.Add(new FormattedText("\r\n"));
                    if (Strength != null && Strength.Value.IsSet)
                    {
                        result.Add(new FormattedText(Resource1.StrengthText + separator));
                        result.AddRange(FormattedValueXIntToFormattedText(Strength, TextFormat.Regular));
                        result.Add(new FormattedText("\r\n"));
                    }
                    break;
                case CardType.Identity:
                    result.Add(new FormattedText("\r\n"));
                    result.Add(new FormattedText(Resource1.DeckSizeText + separator));
                    result.Add(new FormattedText(DeckSize.Value.ToString(), (DeckSize.Formats.Count > 0) ? Influence.Formats[0].Format : TextFormat.Regular));
                    result.Add(new FormattedText("\r\n"));

                    result.Add(new FormattedText(Resource1.InfluenceText + separator));
                    result.Add(new FormattedText(Influence.Value.ToString(), (Influence.Formats.Count > 0) ? Influence.Formats[0].Format : TextFormat.Regular));
                    result.Add(new FormattedText("\r\n"));

                    result.Add(new FormattedText(Resource1.LinkText + separator));
                    result.Add(new FormattedText(Link.Value.ToString(), (Link.Formats.Count > 0) ? Influence.Formats[0].Format : TextFormat.Regular));
                    result.Add(new FormattedText("\r\n"));
                    break;
            }

            if (TrashCost != null && TrashCost.Value.IsSet)
            {
                result.Add(new FormattedText(Resource1.TrashCostText + separator));
                result.AddRange(FormattedValueXIntToFormattedText(TrashCost, TextFormat.Regular));
                result.Add(new FormattedText("\r\n"));
            }

            result.Add(new FormattedText("\r\n"));

            if (Keywords != null && !string.IsNullOrEmpty(Keywords.Value))
            {
                result.AddRange(FormattedValueStringToFormattedText(Keywords, KeywordsFormat));
                result.Add(new FormattedText("\r\n"));
            }

            if (Text != null && !string.IsNullOrEmpty(Text.Value))
            {
                var cardText = new List<FormattedText>();
                FormatCardText(Text.Value, ref cardText);
                ApplyFormatsToFormattedText(cardText, Text.Formats);
                result.AddRange(cardText);
            }
            //if (Type != null && Type.Value != (Int32)CardType.Plot)
            //{
            //    if (Income.Value.IsX || (Income.Value.Value != 0))
            //    {
            //        result.Add(new FormattedText("\r\n+"));
            //        result.AddRange(FormattedValueXIntToFormattedText(Income, TextFormat.Regular));
            //        result.Add(new FormattedText(" " + Resource1.ProvidedGoldText));
            //    }

            //    if (Initiative.Value.IsX || (Initiative.Value.Value != 0))
            //    {
            //        result.Add(new FormattedText("\r\n+"));
            //        result.AddRange(FormattedValueXIntToFormattedText(Initiative, TextFormat.Regular));
            //        result.Add(new FormattedText(" " + Resource1.ProvidedInitiativeText));
            //    }

            //    if (Influence.Value.IsX || (Influence.Value.Value != 0))
            //    {
            //        result.Add(new FormattedText("\r\n+"));
            //        result.AddRange(FormattedValueXIntToFormattedText(Influence, TextFormat.Regular));
            //        result.Add(new FormattedText(" " + Resource1.ProvidedInfluenceText));
            //    }
            //}

            if (Banned != null && Banned.Value)
            {
                result.Add(new FormattedText("\r\n"));
                result.AddRange(FormattedValueBoolToFormattedText(Banned, Resource1.BannedText, "", BannedFormat));
                result.Add(new FormattedText("\r\n"));
            }

            if (Restricted != null && Restricted.Value)
            {
                result.Add(new FormattedText("\r\n"));
                result.AddRange(FormattedValueBoolToFormattedText(Restricted, Resource1.RestrictedText, "", RestrictedFormat));
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

            formattedText.Add(new FormattedText(text));

            //int posBegin, posEnd;

            //// look for the trigger formatting (trigger:)
            //posEnd = text.IndexOf(':');
            //if (posEnd > 0) // not found or first character (the latest is an error case, therefore we don't highlight anything)
            //{
            //    posBegin = Math.Max(text.Substring(0, posEnd).LastIndexOf("\n"),
            //        Math.Max(text.Substring(0, posEnd).LastIndexOf("("),
            //        Math.Max(text.Substring(0, posEnd).LastIndexOf("«"),
            //        Math.Max(text.Substring(0, posEnd).LastIndexOf("“"),
            //        Math.Max(text.Substring(0, posEnd).LastIndexOf("."),
            //            text.Substring(0, posEnd).LastIndexOf("\"")))))) + 1;

            //    if (((posBegin != 0) && (text[posBegin - 1] == '(')) // we do not want to bold the text (it's probably a "limit:" sequence)
            //        || (text[posEnd - 1] == ' ') // or is not a trigger
            //        || CardTriggerNames.All(t => posEnd < t.Length || text.Substring(posEnd - t.Length, t.Length) != t))
            //        formattedText.Add(new FormattedText(text.Substring(0, posEnd + 1)));
            //    else
            //    {
            //        if (posBegin != 0) // add the text before the "xxx:" sequence
            //            formattedText.Add(new FormattedText(text.Substring(0, posBegin)));
            //        // add the "xxx:" sequence
            //        formattedText.Add(new FormattedText(text.Substring(posBegin, posEnd - posBegin + 1), TriggerFormat));
            //    }
            //    // add recursively the text after the "xxx: sequence
            //    if (posEnd + 1 < text.Length)
            //        FormatCardText(text.Substring(posEnd + 1), ref formattedText); // recursive with the rest of the text
            //}
            //else
            //{
            //    // look for trait formatting (~trait~)
            //    posBegin = text.IndexOf('~');
            //    if (posBegin != -1)
            //    {
            //        posEnd = text.IndexOf('~', posBegin + 1);
            //        if (posEnd != -1) // ~ pair found
            //        {
            //            FormatCardText(text.Substring(0, posBegin), ref formattedText);
            //            formattedText.Add(new FormattedText(text.Substring(posBegin + 1, posEnd - posBegin - 1), TraitsFormat));
            //            FormatCardText(text.Substring(posEnd + 1), ref formattedText);
            //            return;
            //        }
            //    }
            //    else
            //        formattedText.Add(new FormattedText(text));
            //}
        }

        /// <summary>
        /// Indicates whether the card could be drawn during a game.
        /// </summary>
        /// <returns>True if the card could be drawn, false otherwise.</returns>
        public override bool IsDrawable()
        {
            return (Type.Value > (Int32)CardType.Identity);
        }

        /// <summary>
        /// Gets the sets this card appears in, replacing the set name by the chapter name
        /// when applicable.
        /// </summary>
        /// <returns>The string containing the sets in which this card appears.</returns>
        public string GetChapteredSets()
        {
            const char setSeparator = '/';
            const char setNumberSeparator = '(';
            const char chapterNumberSeparator = '-';

            // replaces a set "XX-N(R??)" by the associated chapter name "ChapterName(R??)"
            Func<string, string> replaceChapterFunc = delegate(string fullSet)
            {
                var setEnd = fullSet.IndexOf(setNumberSeparator);
                var shortSet = fullSet.Substring(0, setEnd);
                var chapterEnd = shortSet.IndexOf(chapterNumberSeparator);
                if (chapterEnd < 0)
                    return fullSet;
                var chapter = shortSet.Substring(0, chapterEnd);
                string chaptersNames;
                if (!ChaptersNames.TryGetValue(chapter, out chaptersNames))
                    return fullSet;
                var splitChapterNames = chaptersNames.Split(',').ToList().ConvertAll(s => s.Trim());
                int chapterNumber;
                if (!Int32.TryParse(shortSet.Substring(chapterEnd + 1, 1), out chapterNumber)
                    || chapterNumber > splitChapterNames.Count)
                    return fullSet;
                if(chapterNumber > 0)
                    return fullSet.Replace(shortSet, splitChapterNames[chapterNumber - 1]);
                return fullSet;
            };

            var sets = Set.Value.Split(setSeparator);

            return String.Join(" / ", sets.Select(t => replaceChapterFunc(t.Trim())).ToArray());
        }
    }
}
