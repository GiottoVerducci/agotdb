// AGoTDB - A card searcher and deck builder tool for the CCG "A Game of Thrones"
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
// along with this program.  If not, see <http://www.gnu.org/licenses/>.
// You can contact me at v.ripoll@gmail.com
// © A Game of Thrones 2005 George R. R. Martin
// © A Game of Thrones CCG 2005 Fantasy Flight Games Inc.
// © Le Trône de Fer JCC 2005-2007 Stratagèmes éditions / Xénomorphe Sàrl

using System.Xml;

namespace AGoT.AGoTDB.BusinessObjects
{
  /// <summary>
  /// Static class providing convenient methods for handling xml documents.
  /// </summary>
  public static class XmlToolbox
  {
    public static void AddElementValue(XmlDocument doc, XmlElement parent, string elementName, string elementValue)
    {
      XmlNode element = doc.CreateElement(elementName);
      element.AppendChild(doc.CreateTextNode(elementValue));
      parent.AppendChild(element);
    }

    public static string GetElementValue(XmlDocument doc, XmlNode parent, string elementName)
    {
      XmlNode element = FindNode(parent, elementName);
      if ((element != null) && (element.FirstChild != null))
        return element.FirstChild.Value;
      return "";
    }

    public static XmlNode FindNode(XmlNode parent, string nodeName)
    {
      foreach (XmlNode element in parent.ChildNodes)
        if (element.Name == nodeName)
          return element;
      return null;
    }
  }
}