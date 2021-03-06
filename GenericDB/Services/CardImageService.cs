﻿// GenericDB - A generic card searcher and deck builder library for CCGs
// Copyright © 2007, 2008, 2009, 2010, 2011, 2012, 2013, 2014 Vincent Ripoll
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

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace GenericDB.Services
{
    public enum ImageAvailability
    {
        Undefined,
        Available,
        InProgress,
        NotAvailable
    }

    public sealed class CardImageService
    {
        private static readonly CardImageService _cardImageService = new CardImageService();
        // Explicit static constructor to tell C# compiler
        // not to mark type as beforefieldinit (for singleton template implementation)
        static CardImageService()
        {
        }

        private CardImageService()
        {
        }

        /// <summary>
        /// Gets the unique shared singleton instance of this class.
        /// </summary>
        private static CardImageService Singleton
        {
            get { return _cardImageService; }
        }

        public static ICollection<string> GetImageFileNames(string imagesFolder, int universalId, Guid[] octgnIds)
        {
            var result = new List<string>
            {
                String.Format("{0}{1}{2}.jpg",
                    imagesFolder,
                    Path.DirectorySeparatorChar,
                    universalId),
            };
            result.AddRange(octgnIds.Select(octgnId => String.Format("{0}{1}{2}.jpg",
                    imagesFolder,
                    Path.DirectorySeparatorChar,
                    octgnId)));
            return result;
        }

        public static ImageAvailability GetImageAvailability(string imagesFolder, int universalId, Guid[] octgnIds)
        {
            var imageFileNames = GetImageFileNames(imagesFolder, universalId, octgnIds);
            int dummy;
            return GetImageAvailability(imageFileNames, out dummy);
        }

        public static ImageAvailability GetImageAvailability(ICollection<string> imageFileNames, out int index)
        {
            for (int i = 0; i < imageFileNames.Count; ++i)
                if (File.Exists(imageFileNames.ElementAt(i)))
                {
                    index = i;
                    return ImageAvailability.Available;
                }

            index = -1;
            return ImageAvailability.NotAvailable;
        }
    }
}
