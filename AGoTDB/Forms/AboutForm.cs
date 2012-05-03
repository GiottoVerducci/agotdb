// AGoTDB - A card searcher and deck builder tool for the CCG "A Game of Thrones"
// Copyright © 2007, 2008, 2009, 2010, 2011, 2012 Vincent Ripoll
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
// © A Game of Thrones 2005 George R. R. Martin
// © A Game of Thrones CCG 2005 Fantasy Flight Publishing, Inc.
// © A Game of Thrones LCG 2008 Fantasy Flight Publishing, Inc.
// © Le Trône de Fer JCC 2005-2007 Stratagèmes éditions / Xénomorphe Sàrl
// © Le Trône de Fer JCE 2008 Edge Entertainment


using System;
using System.Globalization;
using System.Windows.Forms;
using AGoTDB.BusinessObjects;

namespace AGoTDB.Forms
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
			lblVersion.Text = ApplicationSettings.ApplicationVersion.ToString();
			var databaseInfo = ApplicationSettings.DatabaseManager.DatabaseInfos.Count > 0
				? ApplicationSettings.DatabaseManager.DatabaseInfos[0]
				: null;
			if (databaseInfo != null)
				lblDbVersion.Text = string.Format(CultureInfo.InvariantCulture, "DB version: {0} ({1})",
				databaseInfo.VersionId, databaseInfo.DateCreation.HasValue ? databaseInfo.DateCreation.Value.ToShortDateString() : "");
		}

		private void btnOk_Click(object sender, EventArgs e)
		{
			Close();
		}
	}
}