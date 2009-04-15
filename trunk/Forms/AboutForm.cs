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
using System.Windows.Forms;
using AGoT.AGoTDB.BusinessObjects;

namespace AGoT.AGoTDB.Forms
{
  /// <summary>
  /// About form, including version number and GPL license.
  /// </summary>
  public partial class AboutForm : Form
  {
    /// <summary>
    /// Default form constructor.
    /// </summary>
    public AboutForm()
    {
      InitializeComponent();
    }

    private void AboutForm_Shown(object sender, EventArgs e)
    {
      var dbInfos = DatabaseInterface.Singleton.DatabaseInfos.Count > 0
                      ? DatabaseInterface.Singleton.DatabaseInfos[0]
                      : null;
      lblVersion.Text = ApplicationSettings.ApplicationVersion.ToString();
      if (dbInfos != null)
        lblDbVersion.Text = string.Format("DB version: {0} ({1})", dbInfos.VersionId, dbInfos.DateCreation.HasValue ? dbInfos.DateCreation.Value.ToShortDateString() : "");
    }

    private void btnOk_Click(object sender, EventArgs e)
    {
      Close();
    }
  }
}