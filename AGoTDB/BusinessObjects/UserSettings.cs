// AGoTDB - A card searcher and deck builder tool for the CCG "A Game of Thrones"
// Copyright � 2007, 2008, 2009, 2010, 2011, 2012, 2013, 2014 Vincent Ripoll
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
// � A Game of Thrones 2005 George R. R. Martin
// � A Game of Thrones CCG 2005 Fantasy Flight Publishing, Inc.
// � A Game of Thrones LCG 2008 Fantasy Flight Publishing, Inc.
// � Le Tr�ne de Fer JCC 2005-2007 Stratag�mes �ditions / X�nomorphe S�rl
// � Le Tr�ne de Fer JCE 2008 Edge Entertainment

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
        
        #region Main form
        private const string MainFormSectionName = "MainForm";
        public static string ColumnsSettings
        {
            get { return Singleton.ReadString(MainFormSectionName, "ColumnsSettings", null); }
            set { Singleton.WriteString(MainFormSectionName, "ColumnsSettings", value); }
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

        #region OCTGN
        private const string OctgnSectionName = "OCTGN";
        public static Guid OctgnGameId
        {
            get { return Singleton.ReadGuid(OctgnSectionName, "OctgnGameId", new Guid("a12af4e8-be4b-4cda-a6b6-534f9717391f")); }
            set { Singleton.WriteGuid(OctgnSectionName, "OctgnGameId", value); }
        }

        public static string OctgnSetsDownloadUrl
        {
            get { return Singleton.ReadString(OctgnSectionName, "OctgnSetsDownloadUrl", string.Format("http://www.myget.org/F/octgngamedirectory/api/v2/package/{0}/", OctgnGameId)); }
            set { Singleton.WriteString(OctgnSectionName, "OctgnSetsDownloadUrl", value); }
        }

        public static Guid OctgnHouseBaratheonId
        {
            get { return Singleton.ReadGuid(OctgnSectionName, "OctgnHouseBaratheonId", new Guid("a12af4e8-be4b-4cda-a6b6-534f97001001")); }
            set { Singleton.WriteGuid(OctgnSectionName, "OctgnHouseBaratheonId", value); }
        }

        public static Guid OctgnHouseGreyjoyId
        {
            get { return Singleton.ReadGuid(OctgnSectionName, "OctgnHouseGreyjoyId", new Guid("a12af4e8-be4b-4cda-a6b6-534f97001002")); }
            set { Singleton.WriteGuid(OctgnSectionName, "OctgnHouseGreyjoyId", value); }
        }

        public static Guid OctgnHouseLannisterId
        {
            get { return Singleton.ReadGuid(OctgnSectionName, "OctgnHouseLannisterId", new Guid("a12af4e8-be4b-4cda-a6b6-534f97001003")); }
            set { Singleton.WriteGuid(OctgnSectionName, "OctgnHouseLannisterId", value); }
        }

        public static Guid OctgnHouseMartellId
        {
            get { return Singleton.ReadGuid(OctgnSectionName, "OctgnHouseMartellId", new Guid("a12af4e8-be4b-4cda-a6b6-534f97001004")); }
            set { Singleton.WriteGuid(OctgnSectionName, "OctgnHouseMartellId", value); }
        }

        public static Guid OctgnHouseStarkId
        {
            get { return Singleton.ReadGuid(OctgnSectionName, "OctgnHouseStarkId", new Guid("a12af4e8-be4b-4cda-a6b6-534f97001005")); }
            set { Singleton.WriteGuid(OctgnSectionName, "OctgnHouseStarkId", value); }
        }

        public static Guid OctgnHouseTargaryenId
        {
            get { return Singleton.ReadGuid(OctgnSectionName, "OctgnHouseTargaryenId", new Guid("a12af4e8-be4b-4cda-a6b6-534f97001006")); }
            set { Singleton.WriteGuid(OctgnSectionName, "OctgnHouseTargaryenId", value); }
        }
        #endregion


        public static void Save()
        {
            Singleton.Save();
        }
    }
}
