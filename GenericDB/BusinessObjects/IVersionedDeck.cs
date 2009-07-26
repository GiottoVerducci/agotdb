// GenericDB - A generic card searcher and deck builder library for CCGs
// Copyright © 2007, 2008, 2009 Vincent Ripoll
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
using GenericDB.BusinessObjects;

namespace GenericDB.BusinessObjects
{
	public interface IVersionedDeck<TD, TCL, TC> : IXmlizable
		where TC : class, ICard, new()
		where TCL : class, ICardList<TC>, new()
		where TD : class, IDeck<TCL, TC>, new()
	{
		/// <summary>
		/// Name of the deck author.
		/// </summary>
		String Author { get; set; }

		/// <summary>
		/// Description of the deck, by its author.
		/// </summary>
		String Description { get; set; }

		/// <summary>
		/// Name of the deck, by its author.
		/// </summary>
		String Name { get; set; }

		/// <summary>
		/// Gets the latest version of the deck.
		/// </summary>
		/// <returns>The lastest version of the deck.</returns>
		TD LastVersion { get; }

		/// <summary>
		/// Gets the count of versions of the deck.
		/// </summary>
		/// <returns>The count of versions of the deck.</returns>
		int Count { get; }

		/// <summary>
		/// Gets the version of the deck of given index.
		/// </summary>
		/// <remarks>The deck of index 0 is the initial deck version. 
		/// The deck of index equal to GetVersionsCount - 1 is the latest deck version.</remarks>
		/// <param name="versionIndex">The index of the version of the deck to get.</param>
		/// <returns>The <paramref name="versionIndex"/> version of the deck.</returns>
		TD this[int versionIndex] { get; }

		/// <summary>
		/// Adds a new version to the versioned deck.
		/// </summary>
		/// <param name="commentsForLastVersion">The comment associated to the former latest version.</param>
		void AddNewVersion(String commentsForLastVersion);

		/// <summary>
		/// Initializes this instance with data from an xml file.
		/// </summary>
		/// <param name="filename">The full path and name of the xml file.</param>
		/// <returns>A value indicating if the load was successful or if not, why it wasn't.</returns>
		DeckLoadResult LoadFromXmlFile(string filename);

		/// <summary>
		/// Saves the deck using an xml representation to a file.
		/// </summary>
		/// <param name="filename">The full path and name of the file.</param>
		/// <param name="applicationName">The name of the application creating this file.</param>
		/// <param name="applicationVersion">The version of the application creating this file.</param>
		/// <param name="databaseVersion">The version of the database used by the application creating this file.</param>
		/// <returns>True if the save was successful, False otherwise.</returns>
		DeckSaveResult SaveToXmlFile(string filename, string applicationName, SoftwareVersion applicationVersion, int? databaseVersion);

		bool Equals(object obj);

		int GetHashCode();
		IVersionedDeck<TD, TCL, TC> Clone();
	}

	/// <summary>
	/// Indicates the result of the LoadFromXMLFile method.
	/// </summary>
	public enum DeckLoadResult
	{
		Undefined = 0,
		Success,
		FileNotFound,
		InvalidContent
	}

	public enum DeckSaveResult
	{
		Undefined = 0,
		Success,
		Failure
	}
}