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
using System.Globalization;
using System.Xml;
using System.Windows.Forms;

namespace GenericDB.BusinessObjects
{
	// Rewritten from the following by Vincent Ripoll
	/*
		The Settings class provides an easy way to store
		and load application settings/options in an ini-file
		like structure. The class uses an XML file as its
		underlying database but hides all the XML details
		from the user. All data are written/read with a simple
		interface of Read/Write functions.

		Like ini-files the Settings class stores settings in
		sections which contain name-value pairs, f.e.:

		<Settings>
			 <LastDirectories>
				<Dir1 value="c:\" />
				<Dir2 value="c:\somedirectory" />
			 </LastDirectories>
		</Settings>

		To use the class create an instance passing the
		file name. Use the Read/Write methods as needed.
		After writing to the file call the Save() method
		to flush the changes to disk.

		Note that the section and name strings must follow
		the XML rules! Don't use spaces in section and
		name strings and don't begin with numbers.

		For comments/questions email to mail@heelana.com or
		visit http://wwww.heelana.com
	*/

	public class Settings
	{
		protected readonly XmlDocument fDoc;
		protected readonly XmlNode fDocRoot;
		protected readonly string fFileName;
		protected readonly string fRootName;

		public Settings(string fileName, string rootName)
		{
			if (fileName == null) throw new ArgumentNullException("fileName");
			if (rootName == null) throw new ArgumentNullException("rootName");

			fFileName = fileName;
			fRootName = rootName;

			fDoc = new XmlDocument();

			bool loadingSettings = true;
			try
			{
				fDoc.Load(fileName);
			}
			catch (System.IO.FileNotFoundException)
			{
				loadingSettings = false;
				CreateSettingsDocument();
			}
			
			fDocRoot = fDoc.DocumentElement;
			if(fDocRoot == null)
				throw new ApplicationException(string.Format("Error while {0} file {1}", loadingSettings ? "loading" : "creating", fileName));
		}

		public Settings(string fileName)
			: this(fileName, "Settings")
		{
		}

		/// <summary>
		/// Deletes all entries of a section.
		/// </summary>
		/// <param name="sectionName">The section name.</param>
		public void ClearSection(string sectionName)
		{
			XmlNode s = GetSectionNode(sectionName);

			if (s == null)
				return; //not found

			s.RemoveAll();
		}

		/// <summary>
		/// Initializes a new settings file with the XML version
		/// and the root node.
		/// </summary>
		protected void CreateSettingsDocument()
		{
			fDoc.AppendChild(fDoc.CreateXmlDeclaration("1.0", null, null));
			fDoc.AppendChild(fDoc.CreateElement(fRootName));
		}

		/// <summary>
		/// Saves the pending changes.
		/// </summary>
		public void Flush()
		{
			try
			{
				fDoc.Save(fFileName);
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message);
			}
		}

		/// <summary>
		/// Removes a section and all its entries.
		/// </summary>
		/// <param name="sectionName">The section name.</param>
		public void RemoveSection(string sectionName)
		{
			XmlNode s = GetSectionNode(sectionName);

			if (s != null)
				fDocRoot.RemoveChild(s);
		}

		/// <summary>
		/// Gets the node that represents a section.
		/// </summary>
		/// <param name="sectionName">The section name.</param>
		/// <returns>The node representing the section, null if not found.</returns>
		protected virtual XmlNode GetSectionNode(string sectionName)
		{
			return fDocRoot.SelectSingleNode(string.Format("/{0}/{1}", fRootName, sectionName));
		}

		public void Save()
		{
			Flush();
		}

		#region Read methods

		protected delegate T ValueConverter<T>(string value);

		protected T ReadValue<T>(string sectionName, string propertyName, T defaultValue, ValueConverter<T> valueConverter)
		{
			string s = ReadString(sectionName, propertyName, "");

			if (string.IsNullOrEmpty(s))
				return defaultValue;
			try
			{
				return valueConverter(s);
			}
			catch (FormatException)
			{
				return defaultValue;
			}
		}

		public bool ReadBool(string sectionName, string propertyName, bool defaultValue)
		{
			return ReadValue(sectionName, propertyName, defaultValue, v => Convert.ToBoolean(v, CultureInfo.InvariantCulture));
		}

		public DateTime ReadDateTime(string sectionName, string propertyName, DateTime defaultValue)
		{
			return ReadValue(sectionName, propertyName, defaultValue, v => Convert.ToDateTime(v, CultureInfo.InvariantCulture));
		}

		public double ReadDouble(string sectionName, string propertyName, double defaultValue)
		{
			return ReadValue(sectionName, propertyName, defaultValue, v => Convert.ToDouble(v, CultureInfo.InvariantCulture));
		}

		public float ReadFloat(string sectionName, string propertyName, float defaultValue)
		{
			return ReadValue(sectionName, propertyName, defaultValue, v => Convert.ToSingle(v, CultureInfo.InvariantCulture));
		}

		public int ReadInt(string sectionName, string propertyName, int defaultValue)
		{
			return ReadValue(sectionName, propertyName, defaultValue, v => Convert.ToInt32(v, CultureInfo.InvariantCulture));
		}

		public long ReadLong(string sectionName, string propertyName, long defaultValue)
		{
			return ReadValue(sectionName, propertyName, defaultValue, v => Convert.ToInt64(v, CultureInfo.InvariantCulture));
		}

		public short ReadShort(string sectionName, string propertyName, short defaultValue)
		{
			return ReadValue(sectionName, propertyName, defaultValue, v => Convert.ToInt16(v, CultureInfo.InvariantCulture));
		}

		public string ReadString(string sectionName, string propertyName, string defaultValue)
		{
			XmlNode s = GetSectionNode(sectionName);

			if (s == null)
				return defaultValue; //not found

			XmlNode n = s.SelectSingleNode(propertyName);

			if (n == null)
				return defaultValue;  //not found

			XmlAttributeCollection attrs = n.Attributes;

			foreach (XmlAttribute attr in attrs)
			{
				if (attr.Name == "value")
					return attr.Value;
			}

			return defaultValue;
		}

		public uint ReadUInt(string sectionName, string propertyName, uint defaultValue)
		{
			return ReadValue(sectionName, propertyName, defaultValue, v => Convert.ToUInt32(v, CultureInfo.InvariantCulture));
		}

		public ulong ReadULong(string sectionName, string propertyName, ulong defaultValue)
		{
			return ReadValue(sectionName, propertyName, defaultValue, v => Convert.ToUInt64(v, CultureInfo.InvariantCulture));
		}

		public ushort ReadUShort(string sectionName, string propertyName, ushort defaultValue)
		{
			return ReadValue(sectionName, propertyName, defaultValue, v => Convert.ToUInt16(v, CultureInfo.InvariantCulture));
		}

		#endregion

		#region Write methods

		public void WriteBool(string sectionName, string propertyName, bool value)
		{
			WriteString(sectionName, propertyName, value.ToString());
		}

		public void WriteDateTime(string sectionName, string propertyName, DateTime value)
		{
			WriteString(sectionName, propertyName, value.ToString(CultureInfo.InvariantCulture));
		}

		public void WriteDouble(string sectionName, string propertyName, double value)
		{
			WriteString(sectionName, propertyName, value.ToString(CultureInfo.InvariantCulture));
		}

		public void WriteFloat(string sectionName, string propertyName, float value)
		{
			WriteString(sectionName, propertyName, value.ToString(CultureInfo.InvariantCulture));
		}

		public void WriteInt(string sectionName, string propertyName, int value)
		{
			WriteString(sectionName, propertyName, value.ToString(CultureInfo.InvariantCulture));
		}

		public void WriteLong(string sectionName, string propertyName, long value)
		{
			WriteString(sectionName, propertyName, value.ToString(CultureInfo.InvariantCulture));
		}

		public void WriteShort(string sectionName, string propertyName, short value)
		{
			WriteString(sectionName, propertyName, value.ToString(CultureInfo.InvariantCulture));
		}

		public void WriteString(string sectionName, string propertyName, string value)
		{
			XmlNode s = GetSectionNode(sectionName);

			if (s == null)
				s = fDocRoot.AppendChild(fDoc.CreateElement(sectionName));

			XmlNode n = s.SelectSingleNode(propertyName);

			if (n == null)
				n = s.AppendChild(fDoc.CreateElement(propertyName));

			XmlAttribute attr = ((XmlElement)n).SetAttributeNode("value", "");
			attr.Value = value;
		}

		public void WriteUInt(string sectionName, string propertyName, uint value)
		{
			WriteString(sectionName, propertyName, value.ToString(CultureInfo.InvariantCulture));
		}

		public void WriteULong(string sectionName, string propertyName, ulong value)
		{
			WriteString(sectionName, propertyName, value.ToString(CultureInfo.InvariantCulture));
		}

		public void WriteUShort(string sectionName, string propertyName, ushort value)
		{
			WriteString(sectionName, propertyName, value.ToString(CultureInfo.InvariantCulture));
		}

		#endregion
	}
}
