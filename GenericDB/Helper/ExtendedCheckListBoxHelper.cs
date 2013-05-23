// GenericDB - A generic card searcher and deck builder library for CCGs
// Copyright © 2007, 2008, 2009, 2010, 2011, 2012, 2013 Vincent Ripoll
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
