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
using System.Globalization;
using System.Windows.Forms;
using NRADB.BusinessObjects;

namespace NRADB.Forms
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
				databaseInfo.VersionId, databaseInfo.DateCreation.HasValue ? databaseInfo.DateCreation.Value.ToShortDateString() : string.Empty);
		}

		private void btnOk_Click(object sender, EventArgs e)
		{
			Close();
		}
	}
}