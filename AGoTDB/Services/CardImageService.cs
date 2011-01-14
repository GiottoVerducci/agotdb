using System;
using AGoTDB.BusinessObjects;
using System.IO;

namespace AGoTDB.Services
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
		private static readonly CardImageService fCardImageService = new CardImageService();
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
			get { return fCardImageService; }
		}


		public static string GetImageFileName(int universalId)
		{
			return String.Format("{0}\\{1}.jpg",
				ApplicationSettings.ImagesFolder,
				universalId);
		}

		public static ImageAvailability GetImageAvailability(int universalId)
		{
			var imageFileName = GetImageFileName(universalId);
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
