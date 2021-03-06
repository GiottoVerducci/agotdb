// AGoTDB - A card searcher and deck builder tool for the CCG "A Game of Thrones"
// Copyright � 2007, 2008, 2009, 2010, 2011, 2012, 2013, 2014 Vincent Ripoll
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
// � A Game of Thrones 2005 George R. R. Martin
// � A Game of Thrones CCG 2005 Fantasy Flight Publishing, Inc.
// � A Game of Thrones LCG 2008 Fantasy Flight Publishing, Inc.
// � Le Tr�ne de Fer JCC 2005-2007 Stratag�mes �ditions / X�nomorphe S�rl
// � Le Tr�ne de Fer JCE 2008 Edge Entertainment

using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using AGoTDB.BusinessObjects;
using GenericDB.BusinessObjects;
using GenericDB.DataAccess;

namespace AGoTDB.DataAccess
{
    public class AgotDatabaseManager : DatabaseManager
    {
        public AgotDatabaseManager(string dbFileName, string exDbFileName)
            : base(dbFileName, exDbFileName)
        {
        }

        public override string TableNameMain { get { return "AGoT-db"; } }
        public override string TableNameVersion { get { return "TableVersion"; } }
        public string TableNameFilterText { get { return "TableFilterText"; } }
        public string TableNameHouse { get { return "TableHouse"; } }
        public string TableNameIcon { get { return "TableIcon"; } }
        public string TableNameKeyword { get { return "TableKeyword"; } }
        public string TableNameMechanism { get { return "TableMecanism"; } }
        public string TableNameProvides { get { return "TableProvides"; } }
        public string TableNameSet { get { return "TableSet"; } }
        public string TableNameTrigger { get { return "TableTrigger"; } }
        public string TableNameType { get { return "TableType"; } }
        public string TableNameChallengeEnhancement { get { return "TableChallengeEnhancement"; } }
        public string TableNameVirtue { get { return "TableVirtue"; } }
        public string TableNamePattern { get { return "TablePattern"; } }
        public override TextFormat ErrataFormat { get { return AgotCard.ErrataFormat; } }

        private HashSet<Int32> _universalIds = new HashSet<int>();

        protected override void ConvertCard(DataRow sourceRow, DataRowCollection destinationRows)
        {
            Int32 universalId;
            FormattedValue<string> name, traits, keywords, text, set, originalName, militaryCE, intrigueCE, powerCE;
            FormattedValue<int> type, house;
            FormattedValue<bool?> unique, doomed, endless, military, intrigue, power, war, holy, noble, learned, shadow, multiplayer, banned, restrictedJoust, restrictedMelee;
            FormattedValue<XInt> cost, strength, income, initiative, claim, influence;

            universalId = Int32.Parse(GetRowValue(sourceRow, "UniversalId"), CultureInfo.InvariantCulture);

            if(_universalIds.Contains(universalId))
                throw new Exception("Universal id already used: " + universalId);
            _universalIds.Add(universalId);

            var errataBoundFormat = new BoundFormat('{', '}', AgotCard.ErrataFormat);
            var traitsBoundFormat = new BoundFormat('~', AgotCard.TraitsFormat);

            name = ExtractFormattedStringValueFromRow(sourceRow, "Name", errataBoundFormat, traitsBoundFormat);
            traits = ExtractFormattedStringValueFromRow(sourceRow, "Traits", errataBoundFormat, traitsBoundFormat);
            keywords = ExtractFormattedStringValueFromRow(sourceRow, "Keywords", errataBoundFormat, traitsBoundFormat);
            text = ExtractFormattedStringValueFromRow(sourceRow, "Text", errataBoundFormat, traitsBoundFormat);
            set = ExtractFormattedStringValueFromRow(sourceRow, "Set", errataBoundFormat, traitsBoundFormat);
            originalName = ExtractFormattedStringValueFromRow(sourceRow, "OriginalName", errataBoundFormat, traitsBoundFormat);

            type = ExtractFormattedIntValueFromRow(sourceRow, "Type", TableNameType);
            house = ExtractFormattedIntValueFromRow(sourceRow, "House", TableNameHouse);

            unique = ExtractFormattedBoolValueFromRow(sourceRow, "Unique", errataBoundFormat);
            doomed = ExtractFormattedBoolValueFromRow(sourceRow, "Doomed", errataBoundFormat);
            endless = ExtractFormattedBoolValueFromRow(sourceRow, "Endless", errataBoundFormat);
            military = ExtractFormattedBoolValueFromRow(sourceRow, "Military", errataBoundFormat);
            intrigue = ExtractFormattedBoolValueFromRow(sourceRow, "Intrigue", errataBoundFormat);
            power = ExtractFormattedBoolValueFromRow(sourceRow, "Power", errataBoundFormat);
            militaryCE = ExtractFormattedStringValueFromRow(sourceRow, "MilitaryCE", errataBoundFormat, traitsBoundFormat);
            intrigueCE = ExtractFormattedStringValueFromRow(sourceRow, "IntrigueCE", errataBoundFormat, traitsBoundFormat);
            powerCE = ExtractFormattedStringValueFromRow(sourceRow, "PowerCE", errataBoundFormat, traitsBoundFormat);
            war = ExtractFormattedBoolValueFromRow(sourceRow, "War", errataBoundFormat);
            holy = ExtractFormattedBoolValueFromRow(sourceRow, "Holy", errataBoundFormat);
            noble = ExtractFormattedBoolValueFromRow(sourceRow, "Noble", errataBoundFormat);
            learned = ExtractFormattedBoolValueFromRow(sourceRow, "Learned", errataBoundFormat);
            shadow = ExtractFormattedBoolValueFromRow(sourceRow, "Shadow", errataBoundFormat);
            multiplayer = ExtractFormattedBoolValueFromRow(sourceRow, "Multiplayer", errataBoundFormat);
            banned = ExtractFormattedBoolValueFromRow(sourceRow, "Banned", errataBoundFormat);
            restrictedJoust = ExtractFormattedBoolValueFromRow(sourceRow, "RestrictedJoust", errataBoundFormat);
            restrictedMelee = ExtractFormattedBoolValueFromRow(sourceRow, "RestrictedMelee", errataBoundFormat);

            cost = ExtractFormattedXIntValueFromRow(sourceRow, "Cost", errataBoundFormat);
            strength = ExtractFormattedXIntValueFromRow(sourceRow, "Strength", errataBoundFormat);
            income = ExtractFormattedXIntValueFromRow(sourceRow, "Income", errataBoundFormat);
            initiative = ExtractFormattedXIntValueFromRow(sourceRow, "Initiative", errataBoundFormat);
            claim = ExtractFormattedXIntValueFromRow(sourceRow, "Claim", errataBoundFormat);
            influence = ExtractFormattedXIntValueFromRow(sourceRow, "Influence", errataBoundFormat);

#if DEBUG
			// we compare the case of the name of the card in its title and its cardtext.
			int index;
			string textValue = text.Value;
			do
			{
				index = textValue.IndexOf(name.Value, StringComparison.CurrentCultureIgnoreCase);
				if (index != -1)
				{
					if (string.Compare(textValue.Substring(index, name.Value.Length), name.Value, StringComparison.CurrentCulture) != 0)
						System.Diagnostics.Debug.WriteLine(CultureInfo.CurrentCulture, String.Format("{0} {1}", name.Value, universalId));
					textValue = textValue.Substring(index + name.Value.Length);
				}
			} while (index != -1);
#endif

            destinationRows.Add(
                universalId,
                name.Value, name.FormatsToString(),
                type.Value, (type.Formats.Count > 0),
                (house.Value == 0),
                (house.Value & (Int32)AgotCard.CardHouse.Stark) != 0,
                (house.Value & (Int32)AgotCard.CardHouse.Lannister) != 0,
                (house.Value & (Int32)AgotCard.CardHouse.Baratheon) != 0,
                (house.Value & (Int32)AgotCard.CardHouse.Greyjoy) != 0,
                (house.Value & (Int32)AgotCard.CardHouse.Martell) != 0,
                (house.Value & (Int32)AgotCard.CardHouse.Targaryen) != 0,
                house.Formats.Count > 0,
                unique.Value, unique.Formats.Count > 0,
                traits.Value, traits.FormatsToString(),
                keywords.Value, keywords.FormatsToString(),
                text.Value, text.FormatsToString(),
                doomed.Value, doomed.Formats.Count > 0,
                endless.Value, endless.Formats.Count > 0,
                XIntToString(cost.Value), cost.Formats.Count > 0,
                XIntToString(strength.Value), strength.Formats.Count > 0,
                military.Value, military.Formats.Count > 0,
                intrigue.Value, intrigue.Formats.Count > 0,
                power.Value, power.Formats.Count > 0,
                militaryCE.Value, militaryCE.FormatsToString(),
                intrigueCE.Value, intrigueCE.FormatsToString(),
                powerCE.Value, powerCE.FormatsToString(),
                war.Value, war.Formats.Count > 0,
                holy.Value, holy.Formats.Count > 0,
                noble.Value, noble.Formats.Count > 0,
                learned.Value, learned.Formats.Count > 0,
                shadow.Value, shadow.Formats.Count > 0,
                XIntToString(income.Value), income.Formats.Count > 0,
                XIntToString(initiative.Value), initiative.Formats.Count > 0,
                XIntToString(claim.Value), claim.Formats.Count > 0,
                XIntToString(influence.Value), influence.Formats.Count > 0,
                multiplayer.Value.Value, multiplayer.Formats.Count > 0, // Multiplayer shouldn't be null
                set.Value, set.FormatsToString(),
                originalName.Value, originalName.FormatsToString(),
                banned.Value, banned.Formats.Count > 0,
                restrictedJoust.Value, restrictedJoust.Formats.Count > 0,
                restrictedMelee.Value, restrictedMelee.Formats.Count > 0
            );
        }

        public DataTable GetAgendas()
        {
            return GetResultFromRequest(
                string.Format("SELECT * FROM [{0}] WHERE Type = :type ORDER BY Name", ApplicationSettings.Instance.DatabaseManager.TableNameMain),
                new CommandParameters().Add("type", (Int32)AgotCard.CardType.Agenda));
        }

        public DataTable GetCardTypeNames()
        {
            return GetResultFromRequest(
                string.Format("SELECT * FROM [{0}]", TableNameType));
        }

        public DataTable GetCardHouseNames()
        {
            return GetResultFromRequest(
                string.Format("SELECT * FROM [{0}]", TableNameHouse));
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

        public bool HasOctgnData()
        {
            var table = GetResultFromRequest(
                string.Format("SELECT TOP 1 * FROM [{0}]  WHERE [OctgnId] IS NOT NULL", TableNameMain));
            return table.Rows.Count > 0;
        }
    }
}
