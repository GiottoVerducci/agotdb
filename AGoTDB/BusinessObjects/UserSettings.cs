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
using GenericDB.BusinessObjects;

namespace AGoTDB.BusinessObjects
{
	public sealed class UserSettings
	{
		public const string SettingsFilename = "AGoTDB.xml";
		private static readonly Settings _settings = LoadSettings();

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
			get { return _settings; }
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

		#region General
		public static string DatabaseLanguage
		{
			get { return Singleton.ReadString("General", "DatabaseLanguage", "fr"); }
			set { Singleton.WriteString("General", "DatabaseLanguage", value); }
		}
		
		public static bool DisplayImages
		{
			get { return Singleton.ReadBool("General", "DisplayImages", true); }
			set { Singleton.WriteBool("General", "DisplayImages", value); }
		}

		public static int ImagePreviewSize
		{
			get { return Math.Max(0, Math.Min(100, Singleton.ReadInt("General", "ImagePreviewSize", 100))); }
			set { Singleton.WriteInt("General", "ImagePreviewSize", Math.Max(0, Math.Min(100, value))); }
		}

		public static string ImageRepositoryUrl
		{
			get { return Singleton.ReadString("General", "ImageRepositoryUrl", ""); }
			set { Singleton.WriteString("General", "ImageRepositoryUrl", value); }
		}
		#endregion

		#region Startup
		public static bool CheckForUpdatesOnStartup
		{
			get { return Singleton.ReadBool("Startup", "CheckForUpdatesOnStartup", true); }
			set { Singleton.WriteBool("Startup", "CheckForUpdatesOnStartup", value); }
		}

		public static bool CreateExtendedDB
		{
			get { return Singleton.ReadBool("Startup", "CreateExtendedDB", true); }
			set { Singleton.WriteBool("Startup", "CreateExtendedDB", value); }
		}

		public static string UpdateInformationsUrl
		{
			get { return Singleton.ReadString("Startup", "UpdateInformationsUrl", ""); }
			set { Singleton.WriteString("Startup", "UpdateInformationsUrl", value); }
		}
		#endregion

		#region Deckbuilder
		public static string[] TypeOrder
		{
			get { return Singleton.ReadString("DeckBuilder", "TypeOrder", "").Split(','); }
			set { Singleton.WriteString("DeckBuilder", "TypeOrder", String.Join(",", value)); }
		}

		public static bool ShowNewVersionMessage
		{
			get { return Singleton.ReadBool("Deckbuilder", "ShowNewVersionMessage", true); }
			set { Singleton.WriteBool("Deckbuilder", "ShowNewVersionMessage", value); }
		}
		#endregion

		#region
		public static bool LcgSetsOnly
		{
			get { return Singleton.ReadBool("SearchForm", "LcgSetsOnly", false); }
			set { Singleton.WriteBool("SearchForm", "LcgSetsOnly", value); }
		}
		#endregion

		public static void Save()
		{
			Singleton.Save();
		}
	}
}
