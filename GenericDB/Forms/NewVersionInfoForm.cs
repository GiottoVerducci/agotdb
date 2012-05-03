// GenericDB - A generic card searcher and deck builder library for CCGs
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

using System.Windows.Forms;

namespace GenericDB.Forms
{
	/// <summary>
	/// Information forms about how the versioning works.
	/// </summary>
	public partial class FormNewVersionInfo : Form
	{
		/// <summary>
		/// Default form constructor.
		/// </summary>
		public FormNewVersionInfo()
		{
			InitializeComponent();
		}

		/// <summary>
		/// Indicates whether this form must be displayed the next time it should be.
		/// </summary>
		/// <returns>True if the form should be displayed next time, False otherwise.</returns>
		public bool DisplayFormNextTime()
		{
			return !cbDoNotDisplay.Checked;
		}
	}
}