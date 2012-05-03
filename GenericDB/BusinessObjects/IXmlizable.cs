// GenericDB - A generic card searcher and deck builder library for CCGs
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

using System.Xml;

namespace GenericDB.BusinessObjects
{
	public interface IXmlizable
	{
		/// <summary>
		/// Gets the XML representation of this object.
		/// </summary>
		/// <param name="doc">The XML document for which the XML representation is generated.</param>
		/// <returns>A XML node representing this card.</returns>
		XmlNode ToXml(XmlDocument doc);

		/// <summary>
		/// Initializes the properties of this object from an XML node that was generated 
		/// using the ToXml method.
		/// </summary>
		/// <param name="doc">The XML document containing the XML node.</param>
		/// <param name="root">The XML node containing the XML data representing the object.</param>
		void InitializeFromXml(XmlDocument doc, XmlNode root);
	}
}
