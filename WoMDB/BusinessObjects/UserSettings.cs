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
using GenericDB.BusinessObjects;

namespace WoMDB.BusinessObjects
{
	public sealed class UserSettings
	{
		public const string SettingsFilename = "WoMDB.xml";
		private static readonly Settings fSettings = LoadSettings();

		// Explicit static constructor to tell C# compiler
		// not to mark type as beforefieldinit (for singleton template implementation)
		static UserSettings()
		{
		}

		private UserSettings()
		{
		}

		/// <summary>
		/// Gets the unique shared singleton instance of this class.
		/// </summary>
		public static Settings Singleton
		{
			get { return fSettings; }
		}

		/// <summary>
		/// Load the application settings stored in the xml configuration file.
		/// </summary>
		private static Settings LoadSettings()
		{
			try
			{
				return new Settings(SettingsFilename);
			}
			catch (Exception)
			{
				return null;
			}
		}
	}
}
