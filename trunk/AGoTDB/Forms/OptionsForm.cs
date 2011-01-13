// AGoTDB - A card searcher and deck builder tool for the CCG "A Game of Thrones"
// Copyright © 2007, 2008, 2009, 2010 Vincent Ripoll
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
using System.Linq;
using System.Windows.Forms;
using AGoTDB.BusinessObjects;

namespace AGoTDB.Forms
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
			var displayMessageSettings = new bool[] { UserSettings.ShowNewVersionMessage };
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
			if (cbDisplayMessages.CheckState == CheckState.Checked || cbDisplayMessages.CheckState == CheckState.Unchecked)
				UserSettings.ShowNewVersionMessage = cbDisplayMessages.CheckState == CheckState.Checked;
			UserSettings.Save();
		}

		private void EnsureControlsCoherence()
		{
			pnlCardPreviewSize.Enabled = cbDisplayImages.Checked;
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
