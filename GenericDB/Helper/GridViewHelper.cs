// GenericDB - A generic card searcher and deck builder library for CCGs
// Copyright © 2007, 2008, 2009, 2010, 2011, 2012, 2013, 2014 Vincent Ripoll
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
using System.Data;
using System.Linq;
using System.Windows.Forms;

namespace GenericDB.Helper
{
    public static class GridViewHelper
    {
        public static void SetDataTableColumnsSettings(DataTable dataTable, string columnsSettings)
        {
            if (String.IsNullOrWhiteSpace(columnsSettings))
                return;
            var columnSettings = columnsSettings.Split('|').Select(s => new ColumnSetting(s)).OrderByDescending(cs => cs.Index);
            foreach (var columnSetting in columnSettings)
            {
                var dataTableColumn = dataTable.Columns[columnSetting.Name];
                if (dataTableColumn != null)
                    dataTableColumn.SetOrdinal(Convert.ToInt32(columnSetting.Index));
            }
        }

        public static void SetDataGridViewColumnsSettings(DataGridView dataGridView, string columnsSettings)
        {
            if (columnsSettings == null)
                return;
            var columnSettings = columnsSettings.Split('|').Select(s => new ColumnSetting(s)).OrderByDescending(cs => cs.Index);
            foreach (var columnSetting in columnSettings)
            {
                var column = dataGridView.Columns[columnSetting.Name];
                if (column != null)
                    column.Width = Convert.ToInt32(columnSetting.Width);
            }
        }

        public static string GetDataGridViewColumnsSettings(DataGridView dataGridView)
        {
            var columnsSettings =
                from DataGridViewColumn column in dataGridView.Columns
                select new ColumnSetting(column.Name, column.DisplayIndex, column.Width) into setting
                select setting.ToString();
            return string.Join("|", columnsSettings);
        }
    }

    public class ColumnSetting
    {
        public string Name { get; set; }
        public int Index { get; set; }
        public int Width { get; set; }

        public override string ToString()
        {
            return string.Format("{0};{1};{2}", Name, Index, Width);
        }

        public ColumnSetting(string setting)
        {
            var values = setting.Split(';');
            Name = values[0];
            Index = Convert.ToInt32(values[1]);
            Width = Convert.ToInt32(values[2]);
        }

        public ColumnSetting(string name, int index, int width)
        {
            Name = name;
            Index = index;
            Width = width;
        }
    }

}
