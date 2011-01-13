// AGoTDB - A card searcher and deck builder tool for the CCG "A Game of Thrones"
// Copyright © 2007, 2008, 2009, 2010, 2011 Vincent Ripoll
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
using GenericDB.BusinessObjects;

namespace AGoTDB.BusinessObjects
{
	public sealed class UserSettings
	{
		public const string SettingsFilename = "AGoTDB.xml";
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
		private static Settings Singleton
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

		public static bool IsAvailable()
		{
			return Singleton != null;
		}

		public static bool DisplayImages
		{
			get { return UserSettings.Singleton.ReadBool("General", "DisplayImages", true); }
			set { UserSettings.Singleton.WriteBool("General", "DisplayImages", value); }
		}

		public static int ImagePreviewSize
		{
			get { return Math.Max(0, Math.Min(100, UserSettings.Singleton.ReadInt("General", "ImagePreviewSize", 100))); }
			set { UserSettings.Singleton.WriteInt("General", "ImagePreviewSize", Math.Max(0, Math.Min(100, value))); }
		}

		public static bool CreateExtendedDB
		{
			get { return UserSettings.Singleton.ReadBool("Startup", "CreateExtendedDB", true); }
			set { UserSettings.Singleton.WriteBool("Startup", "CreateExtendedDB", value); }
		}

		public static string[] TypeOrder
		{
			get { return UserSettings.Singleton.ReadString("DeckBuilder", "TypeOrder", "").Split(','); }
			set { UserSettings.Singleton.WriteString("DeckBuilder", "TypeOrder", String.Join(",", value)); }
		}

		public static bool ShowNewVersionMessage
		{
			get { return UserSettings.Singleton.ReadBool("Deckbuilder", "ShowNewVersionMessage", true); }
			set { UserSettings.Singleton.WriteBool("Deckbuilder", "ShowNewVersionMessage", value); }
		}

		public static bool LcgSetsOnly
		{
			get { return UserSettings.Singleton.ReadBool("SearchForm", "LcgSetsOnly", false); }
			set { UserSettings.Singleton.WriteBool("SearchForm", "LcgSetsOnly", value); }
		}

		public static string ImageRepositoryUrl
		{
			get { return UserSettings.Singleton.ReadString("General", "ImageRepositoryUrl", ""); }
			set { UserSettings.Singleton.WriteString("General", "ImageRepositoryUrl", value); }
		}

		public static void Save()
		{
			UserSettings.Singleton.Save();
		}
	}
}
