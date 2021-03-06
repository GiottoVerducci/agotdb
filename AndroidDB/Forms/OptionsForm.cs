﻿// AndroidDB - A card searcher and deck builder tool for the LCG "Netrunner Android"
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
using System.Linq;
using System.Windows.Forms;
using NRADB.BusinessObjects;

namespace NRADB.Forms
{
	public partial class OptionsForm : Form
	{
		public OptionsForm()
		{
			InitializeComponent();
		}

		private void OptionsForm_Load(object sender, EventArgs e)
		{
			LoadOptions();
		}

		private void btnOK_Click(object sender, EventArgs e)
		{
			SaveOptions();
		}

		private void LoadOptions()
		{
			cbDisplayImages.Checked = UserSettings.DisplayImages;
			nudCardPreviewSize.Value = UserSettings.ImagePreviewSize;
			cbRebuildDatabase.Checked = UserSettings.CreateExtendedDB;
			cbCheckForDatabaseUpdateOnStartup.Checked = UserSettings.CheckForUpdatesOnStartup;
			var displayMessageSettings = new [] { UserSettings.ShowNewVersionMessage };
			cbDisplayMessages.CheckState = displayMessageSettings.All(b => b)
				? CheckState.Checked
				: (displayMessageSettings.All(b => !b)
					? CheckState.Unchecked
					: CheckState.Indeterminate);
		}

		private void SaveOptions()
		{
			UserSettings.DisplayImages = cbDisplayImages.Checked;
			UserSettings.ImagePreviewSize = Convert.ToInt32(nudCardPreviewSize.Value);
			UserSettings.CreateExtendedDB = cbRebuildDatabase.Checked;
			UserSettings.CheckForUpdatesOnStartup = cbCheckForDatabaseUpdateOnStartup.Checked;
			if (cbDisplayMessages.CheckState == CheckState.Checked || cbDisplayMessages.CheckState == CheckState.Unchecked)
				UserSettings.ShowNewVersionMessage = cbDisplayMessages.CheckState == CheckState.Checked;
			UserSettings.Save();
		}

		private void EnsureControlsCoherence()
		{
			lbCardPreviewSize.Enabled = nudCardPreviewSize.Enabled = cbDisplayImages.Checked;
		}

		private void cbDisplayImages_CheckedChanged(object sender, EventArgs e)
		{
			EnsureControlsCoherence();
		}

		private void OptionsForm_Shown(object sender, EventArgs e)
		{
			EnsureControlsCoherence();
		}
	}
}
