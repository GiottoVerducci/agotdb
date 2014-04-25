// AGoTDB - A card searcher and deck builder tool for the CCG "A Game of Thrones"
// Copyright © 2007, 2008, 2009, 2010, 2011, 2012, 2013 Vincent Ripoll
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

using AGoTDB.DataAccess;
using GenericDB.BusinessObjects;

namespace AGoTDB.BusinessObjects
{
    public class ApplicationSettings : IApplicationSettings
    {
        private static readonly ApplicationSettings _instance = new ApplicationSettings();

        // Explicit static constructor to tell C# compiler
        // not to mark type as beforefieldinit
        static ApplicationSettings() { }

        private ApplicationSettings() { }

        public static ApplicationSettings Instance
        {
            get { return _instance; }
        }

        public readonly string ApplicationName = "AGoTDB";
        public readonly SoftwareVersion ApplicationVersion = new SoftwareVersion(0, 749 , 0);
        public AgotDatabaseManager DatabaseManager { get; set; }
        public bool ImagesFolderExists { get; set; }
        public string ImagesFolder { get; set; }
        public bool IsOctgnReady { get; set; }
    }
}
