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
using System.Data;
using System.Globalization;
using NRADB.BusinessObjects;
using GenericDB.BusinessObjects;
using GenericDB.DataAccess;

namespace NRADB.DataAccess
{
    public class NraDatabaseManager : DatabaseManager
    {
        public NraDatabaseManager(string dbFileName, string exDbFileName)
            : base(dbFileName, exDbFileName)
        {
        }

        public override string TableNameMain { get { return "NRA-db"; } }
        public override string TableNameVersion { get { return "TableVersion"; } }
        public string TableNameFilterText { get { return "TableFilterText"; } }
        public string TableNameFaction { get { return "TableFaction"; } }
        public string TableNameSide { get { return "TableSide"; } }
        public string TableNameKeyword { get { return "TableKeyword"; } }
        public string TableNameMechanism { get { return "TableMecanism"; } }
        public string TableNameProvides { get { return "TableProvides"; } }
        public string TableNameSet { get { return "TableSet"; } }
        public string TableNameTrigger { get { return "TableTrigger"; } }
        public string TableNameType { get { return "TableType"; } }
        public string TableNamePattern { get { return "TablePattern"; } }
        public string TableNameIceType { get { return "TableIceType"; } }
        public override TextFormat ErrataFormat { get { return NraCard.ErrataFormat; } }

        protected override void ConvertCard(DataRow sourceRow, DataRowCollection destinationRows)
        {
            Int32 universalId;
            Guid octgnId;
            FormattedValue<string> name, subtitle, keywords, text, set, originalName, flavor, instructions;
            FormattedValue<int> type, faction, side, iceType;
            FormattedValue<bool?> unique, banned, restricted;
            FormattedValue<XInt> cost, influence, requirement, mu, deckSize, stat, strength, agendaPoints, 
                link, trashCost, recurringCredits, creditsIncome, providesMu;

            universalId = Int32.Parse(GetRowValue(sourceRow, "UniversalId"), CultureInfo.InvariantCulture);
            octgnId = Guid.Parse(GetRowValue(sourceRow, "OctgnId"));

            var errataBoundFormat = new BoundFormat('{', '}', NraCard.ErrataFormat);
            var keywordBoundFormat = new BoundFormat('~', NraCard.KeywordsFormat);

            name = ExtractFormattedStringValueFromRow(sourceRow, "Name", errataBoundFormat, keywordBoundFormat);
            subtitle = ExtractFormattedStringValueFromRow(sourceRow, "Subtitle", errataBoundFormat, keywordBoundFormat);
            keywords = ExtractFormattedStringValueFromRow(sourceRow, "Keywords", errataBoundFormat, keywordBoundFormat);
            text = ExtractFormattedStringValueFromRow(sourceRow, "Text", errataBoundFormat, keywordBoundFormat);
            set = ExtractFormattedStringValueFromRow(sourceRow, "Set", errataBoundFormat, keywordBoundFormat);
            originalName = ExtractFormattedStringValueFromRow(sourceRow, "OriginalName", errataBoundFormat, keywordBoundFormat);
            flavor = ExtractFormattedStringValueFromRow(sourceRow, "Flavor", errataBoundFormat, keywordBoundFormat);
            instructions = ExtractFormattedStringValueFromRow(sourceRow, "Instructions", errataBoundFormat, keywordBoundFormat);

            type = ExtractFormattedIntValueFromRow(sourceRow, "Type", TableNameType);
            faction = ExtractFormattedIntValueFromRow(sourceRow, "Faction", TableNameFaction);
            side = ExtractFormattedIntValueFromRow(sourceRow, "Side", TableNameSide);
            iceType = ExtractFormattedIntValueFromRow(sourceRow, "IceType", TableNameIceType);

            unique = ExtractFormattedBoolValueFromRow(sourceRow, "Unique", errataBoundFormat);
            banned = ExtractFormattedBoolValueFromRow(sourceRow, "Banned", errataBoundFormat);
            restricted = ExtractFormattedBoolValueFromRow(sourceRow, "Restricted", errataBoundFormat);

            influence = ExtractFormattedXIntValueFromRow(sourceRow, "Influence", errataBoundFormat);
            cost = ExtractFormattedXIntValueFromRow(sourceRow, "Cost", errataBoundFormat);
            stat = ExtractFormattedXIntValueFromRow(sourceRow, "Stat", errataBoundFormat);
            strength = ExtractFormattedXIntValueFromRow(sourceRow, "Strength", errataBoundFormat);
            agendaPoints = ExtractFormattedXIntValueFromRow(sourceRow, "AgendaPoints", errataBoundFormat);
            link = ExtractFormattedXIntValueFromRow(sourceRow, "Link", errataBoundFormat);
            trashCost = ExtractFormattedXIntValueFromRow(sourceRow, "TrashCost", errataBoundFormat);
            requirement = ExtractFormattedXIntValueFromRow(sourceRow, "Requirement", errataBoundFormat);
            mu = ExtractFormattedXIntValueFromRow(sourceRow, "MU", errataBoundFormat);
            deckSize = ExtractFormattedXIntValueFromRow(sourceRow, "DeckSize", errataBoundFormat);
            recurringCredits = ExtractFormattedXIntValueFromRow(sourceRow, "RecurringCredits", errataBoundFormat);
            creditsIncome = ExtractFormattedXIntValueFromRow(sourceRow, "CreditsIncome", errataBoundFormat);
            providesMu = ExtractFormattedXIntValueFromRow(sourceRow, "ProvidesMU", errataBoundFormat);
#if DEBUG
            //// we compare the case of the name of the card in its title and its cardtext.
            //int index;
            //string textValue = text.Value;
            //do
            //{
            //    index = textValue.IndexOf(name.Value, StringComparison.CurrentCultureIgnoreCase);
            //    if (index != -1)
            //    {
            //        if (string.Compare(textValue.Substring(index, name.Value.Length), name.Value, StringComparison.CurrentCulture) != 0)
            //            System.Diagnostics.Debug.WriteLine(CultureInfo.CurrentCulture, String.Format("{0} {1}", name.Value, universalId));
            //        textValue = textValue.Substring(index + name.Value.Length);
            //    }
            //} while (index != -1);
#endif

            destinationRows.Add(
                universalId,
                name.Value, name.FormatsToString(),
                subtitle.Value, subtitle.FormatsToString(),
                side.Value, (side.Formats.Count > 0),
                type.Value, (type.Formats.Count > 0),
                (faction.Value == 0),
                (faction.Value & (Int32)NraCard.CardFaction.Anarch) != 0,
                (faction.Value & (Int32)NraCard.CardFaction.Criminal) != 0,
                (faction.Value & (Int32)NraCard.CardFaction.Shaper) != 0,
                (faction.Value & (Int32)NraCard.CardFaction.HaasBioroid) != 0,
                (faction.Value & (Int32)NraCard.CardFaction.Jinteki) != 0,
                (faction.Value & (Int32)NraCard.CardFaction.NBN) != 0,
                (faction.Value & (Int32)NraCard.CardFaction.Weyland) != 0,
                faction.Formats.Count > 0,
                unique.Value, unique.Formats.Count > 0,
                keywords.Value, keywords.FormatsToString(),
                text.Value, text.FormatsToString(),
                instructions.Value, instructions.FormatsToString(),
                XIntToString(cost.Value), cost.Formats.Count > 0,
                XIntToString(stat.Value), stat.Formats.Count > 0,
                XIntToString(strength.Value), strength.Formats.Count > 0,
                XIntToString(agendaPoints.Value), agendaPoints.Formats.Count > 0,
                XIntToString(link.Value), link.Formats.Count > 0,
                XIntToString(trashCost.Value), trashCost.Formats.Count > 0,
                XIntToString(influence.Value), influence.Formats.Count > 0,
                XIntToString(requirement.Value), requirement.Formats.Count > 0,
                XIntToString(mu.Value), mu.Formats.Count > 0,
                XIntToString(deckSize.Value), deckSize.Formats.Count > 0,
                set.Value, set.FormatsToString(),
                originalName.Value, originalName.FormatsToString(),
                banned.Value, banned.Formats.Count > 0,
                restricted.Value, restricted.Formats.Count > 0,
                octgnId,
                flavor.Value, flavor.FormatsToString(),
                XIntToString(recurringCredits.Value), recurringCredits.Formats.Count > 0,
                XIntToString(creditsIncome.Value), creditsIncome.Formats.Count > 0,
                XIntToString(providesMu.Value), providesMu.Formats.Count > 0,
                iceType.Value, iceType.Formats.Count > 0
            );
        }

        public DataTable GetIdentities()
        {
            return GetResultFromRequest(
                string.Format("SELECT * FROM [{0}] WHERE Type = :type ORDER BY Name", ApplicationSettings.DatabaseManager.TableNameMain),
                new CommandParameters().Add("type", (Int32)NraCard.CardType.Identity));
        }

        public DataTable GetCardTypeNames()
        {
            return GetResultFromRequest(
                string.Format("SELECT * FROM [{0}]", TableNameType));
        }

        public DataTable GetCardFactionNames()
        {
            return GetResultFromRequest(
                string.Format("SELECT * FROM [{0}]", TableNameFaction));
        }

        public DataTable GetCardSideNames()
        {
            return GetResultFromRequest(
                string.Format("SELECT * FROM [{0}]", TableNameSide));
        }

        public DataTable GetCardTriggerNames()
        {
            return GetResultFromRequest(
                string.Format("SELECT * FROM [{0}]", TableNameTrigger));
        }

        public DataTable GetCardPatterns()
        {
            return GetResultFromRequest(
                string.Format("SELECT * FROM [{0}]", TableNamePattern));
        }

        public DataTable GetExpansionSets()
        {
            return GetResultFromRequest(
                string.Format("SELECT * FROM [{0}]", TableNameSet));
        }

        public DataTable GetIceTypes()
        {
            return GetResultFromRequest(
                string.Format("SELECT * FROM [{0}]", TableNameIceType));
        }

        public bool HasOctgnData()
        {
            var table = GetResultFromRequest(
                string.Format("SELECT TOP 1 * FROM [{0}]  WHERE [OctgnId] IS NOT NULL", TableNameMain));
            return table.Rows.Count > 0;
        }

        public void ResetAndImportSets(Func<DataRowCollection, OperationResult> importAction)
        {
            ResetAndImportTable(TableNameSet, importAction, "WHERE Id > -1");
        }
    }
}
