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
using NRADB.DataAccess;
using GenericDB.BusinessObjects;

namespace NRADB.BusinessObjects
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

        public readonly string ApplicationName = "NRADB";
        public readonly SoftwareVersion ApplicationVersion = new SoftwareVersion(0, 11, 0);
        public NraDatabaseManager DatabaseManager { get; set; }
        public bool ImagesFolderExists { get; set; }
        public string ImagesFolder { get; set; }
        public bool IsOctgnReady { get; set; }
    }
}
