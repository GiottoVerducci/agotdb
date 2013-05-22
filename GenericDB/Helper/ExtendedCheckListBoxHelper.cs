using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using Beyond.ExtendedControls;
using GenericDB.DataAccess;
using GenericDB.Extensions;

namespace GenericDB.Helper
{
    public static class ExtendedCheckListBoxHelper
    {
        public static void UpdateEclAccordingToDatabase(ExtendedCheckedListBox extendedCheckedListBox, DatabaseManager databaseManager, string tableName, string column, TableType tableType, Predicate<DbFilter> filter)
        {
            List<object> checkedItems = extendedCheckedListBox.GetItemsByState(CheckState.Checked);
            List<object> indeterminateItems = extendedCheckedListBox.GetItemsByState(CheckState.Indeterminate);

            databaseManager.UpdateExtendedCheckedListBox(extendedCheckedListBox, tableName, column, tableType, filter);
            
            extendedCheckedListBox.WorkOnExpandedItems(ecl =>
                {
                    for (int i = 0; i < ecl.Items.Count; ++i)
                    {
                        var item = (DbFilter)ecl.Items[i];
                        if (checkedItems.Any(o => ((DbFilter)o).ShortName == item.ShortName))
                            ecl.SetItemCheckState(i, CheckState.Checked);
                        else if (indeterminateItems.Any(o => ((DbFilter)o).ShortName == item.ShortName))
                            ecl.SetItemCheckState(i, CheckState.Indeterminate);
                    }
                });
        }
    }
}
