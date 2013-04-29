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
using GenericDB.BusinessObjects;

namespace NRADB.BusinessObjects
{
    public sealed class UserSettings
    {
        public const string SettingsFilename = "NRADB.xml";
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
        private const string GeneralSectionName = "General";
        public static string DatabaseLanguage
        {
            get { return Singleton.ReadString(GeneralSectionName, "DatabaseLanguage", "fr"); }
            set { Singleton.WriteString(GeneralSectionName, "DatabaseLanguage", value); }
        }

        public static bool DisplayImages
        {
            get { return Singleton.ReadBool(GeneralSectionName, "DisplayImages", true); }
            set { Singleton.WriteBool(GeneralSectionName, "DisplayImages", value); }
        }

        public static int ImagePreviewSize
        {
            get { return Math.Max(0, Math.Min(100, Singleton.ReadInt(GeneralSectionName, "ImagePreviewSize", 100))); }
            set { Singleton.WriteInt(GeneralSectionName, "ImagePreviewSize", Math.Max(0, Math.Min(100, value))); }
        }

        public static string ImageRepositoryUrl
        {
            get { return Singleton.ReadString(GeneralSectionName, "ImageRepositoryUrl", ""); }
            set { Singleton.WriteString(GeneralSectionName, "ImageRepositoryUrl", value); }
        }
        #endregion

        #region Startup
        private const string StartupSectionName = "Startup";
        public static bool CheckForUpdatesOnStartup
        {
            get { return Singleton.ReadBool(StartupSectionName, "CheckForUpdatesOnStartup", true); }
            set { Singleton.WriteBool(StartupSectionName, "CheckForUpdatesOnStartup", value); }
        }

        public static bool CreateExtendedDB
        {
            get { return Singleton.ReadBool(StartupSectionName, "CreateExtendedDB", true); }
            set { Singleton.WriteBool(StartupSectionName, "CreateExtendedDB", value); }
        }

        public static string UpdateInformationsUrl
        {
            get { return Singleton.ReadString(StartupSectionName, "UpdateInformationsUrl", ""); }
            set { Singleton.WriteString(StartupSectionName, "UpdateInformationsUrl", value); }
        }
        #endregion

        #region DeckBuilder
        private const string DeckBuilderSectionName = "DeckBuilder";
        public static string[] TypeOrder
        {
            get { return Singleton.ReadString(DeckBuilderSectionName, "TypeOrder", "").Split(','); }
            set { Singleton.WriteString(DeckBuilderSectionName, "TypeOrder", String.Join(",", value)); }
        }

        public static bool ShowNewVersionMessage
        {
            get { return Singleton.ReadBool(DeckBuilderSectionName, "ShowNewVersionMessage", true); }
            set { Singleton.WriteBool(DeckBuilderSectionName, "ShowNewVersionMessage", value); }
        }
        #endregion

        #region SearchForm
        private const string SearchFormSectionName = "SearchForm";
        public static bool LcgSetsOnly
        {
            get { return Singleton.ReadBool(SearchFormSectionName, "LcgSetsOnly", false); }
            set { Singleton.WriteBool(SearchFormSectionName, "LcgSetsOnly", value); }
        }
        #endregion

        #region Main form
        private const string MainFormSectionName = "MainForm";
        public static string ColumnsSettings
        {
            get { return Singleton.ReadString(MainFormSectionName, "ColumnsSettings", null); }
            set { Singleton.WriteString(MainFormSectionName, "ColumnsSettings", value); }
        }
        #endregion

        #region OCTGN
        private const string OctgnSectionName = "OCTGN";
        public static Guid OctgnGameId
        {
            get { return Singleton.ReadGuid(OctgnSectionName, "OctgnGameId", new Guid("a12af4e8-be4b-4cda-a6b6-534f9717391f")); }
            set { Singleton.WriteGuid(OctgnSectionName, "OctgnGameId", value); }
        }
        #endregion


        public static void Save()
        {
            Singleton.Save();
        }
    }
}
