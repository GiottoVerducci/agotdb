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
using System.IO;

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

		public static string GetImageFileName(string imagesFolder, int universalId)
		{
			return String.Format("{0}{1}{2}.jpg",
				imagesFolder,
				Path.DirectorySeparatorChar,
				universalId);
		}

		public static ImageAvailability GetImageAvailability(string imagesFolder, int universalId)
		{
			var imageFileName = GetImageFileName(imagesFolder, universalId);
			return GetImageAvailability(imageFileName);
		}

		public static ImageAvailability GetImageAvailability(string imageFileName)
		{
			return File.Exists(imageFileName)
				? ImageAvailability.Available
				: ImageAvailability.NotAvailable;
		}
	}
}
