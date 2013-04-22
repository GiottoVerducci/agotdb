using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace GenericDB.Helper
{
    public static class GridViewHelper
    {
        public static void SetDataTableColumnsSettings(DataTable dataTable, string columnsSettings)
        {
            if (columnsSettings == null)
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
