// WoMDB - A card searcher and deck builder tool for the CCG "Wizards of Mickey"
// Copyright © 2009 Vincent Ripoll
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
// © Wizards of Mickey CCG ??? ???

using System;
using System.Globalization;
using System.Threading;
using System.Windows.Forms;
using WoMDB.Forms;

namespace WoMDB
{
	static class Program
	{
		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main()
		{
			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(false);
			// The culture change must be done before InitializeComponent()
			// 1/ get the current culture (eg. ci.name="fr-FR")
			// 2/ change the application thread current culture
			Thread.CurrentThread.CurrentUICulture = CultureInfo.CurrentCulture;

			using (var form = new MainForm())
			{
				form.InitializeMainForm();
				Application.Run(form);
			}
		}
	}
}
