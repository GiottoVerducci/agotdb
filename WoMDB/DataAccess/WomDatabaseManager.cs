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
using System.Data;
using System.Globalization;
using GenericDB.BusinessObjects;
using GenericDB.DataAccess;
using WoMDB.BusinessObjects;

namespace WoMDB.DataAccess
{
	public class WomDatabaseManager : DatabaseManager
	{
		public WomDatabaseManager(string dbFileName, string exDbFileName)
			: base(dbFileName, exDbFileName)
		{
		}

		public override string TableNameMain { get { return "WoM-db"; } }
		public override string TableNameVersion { get { return "TableVersion"; } }
		public string TableNameType { get { return "TableType"; } }
		public string TableNameColor { get { return "TableColor"; } }
		public string TableNameSet { get { return "TableSet"; } }
		public string TableNameFilterText { get { return "TableFilterText"; } }
		public override TextFormat ErrataFormat { get { return WomCard.ErrataFormat; } }

		protected override void ConvertCard(DataRow sourceRow, DataRowCollection destinationRows)
		{
			Int32 universalId;
			FormattedValue<string> name, title, team, text, set;
			FormattedValue<int> type, color;
			FormattedValue<XInt> power;

			universalId = Int32.Parse(GetRowValue(sourceRow, "UniversalId"), CultureInfo.InvariantCulture);

			var errataBoundFormat = new BoundFormat('{', '}', WomCard.ErrataFormat);
			var specialBoundFormat = new BoundFormat('~', WomCard.SpecialFormat);

			name = ExtractFormattedStringValueFromRow(sourceRow, "Name", errataBoundFormat, specialBoundFormat);
			title = ExtractFormattedStringValueFromRow(sourceRow, "Title", errataBoundFormat, specialBoundFormat);
			team = ExtractFormattedStringValueFromRow(sourceRow, "Team", errataBoundFormat, specialBoundFormat);
			text = ExtractFormattedStringValueFromRow(sourceRow, "Text", errataBoundFormat, specialBoundFormat);
			set = ExtractFormattedStringValueFromRow(sourceRow, "Set", errataBoundFormat, specialBoundFormat);

			type = ExtractFormattedIntValueFromRow(sourceRow, "Type", TableNameType);
			color = ExtractFormattedIntValueFromRow(sourceRow, "Color", TableNameColor);

			power = ExtractFormattedXIntValueFromRow(sourceRow, "Power");

			destinationRows.Add(
				universalId,
				name.Value, name.FormatsToString(),
				title.Value, title.FormatsToString(),
				team.Value, team.FormatsToString(),
				type.Value, (type.Formats.Count > 0),
				(color.Value == 0),
				(color.Value & (int)WomCard.CardColor.Blue) != 0,
				(color.Value & (int)WomCard.CardColor.Green) != 0,
				(color.Value & (int)WomCard.CardColor.Red) != 0,
				(color.Value & (int)WomCard.CardColor.Yellow) != 0,
				(color.Value & (int)WomCard.CardColor.Black) != 0,
				color.Formats.Count > 0,
				text.Value, text.FormatsToString(),
				XIntToString(power.Value), power.Formats.Count > 0,
				set.Value, set.FormatsToString()
			);
		}

		public DataTable GetCardTypeNames()
		{
			return GetResultFromRequest(
				string.Format("SELECT * FROM {0}", TableNameType));
		}

		public DataTable GetCardColorNames()
		{
			return GetResultFromRequest(
				string.Format("SELECT * FROM {0}", TableNameColor));
		}
	}
}
