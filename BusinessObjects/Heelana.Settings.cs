using System;
using System.Globalization;
using System.Xml;
using System.Windows.Forms;

namespace Heelana
{
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
		XmlDocument doc;
		string fileName;
		string rootName;

		public Settings(string fileName, string rootName)
		{
			this.fileName = fileName;
			this.rootName = rootName;

			doc = new XmlDocument();

			try
			{
				doc.Load(fileName);
			}
			catch(System.IO.FileNotFoundException)
			{
				CreateSettingsDocument();
			}
		}

		public Settings(string fileName) : this(fileName, "Settings")
		{
		}

		//Deletes all entries of a section
		public void ClearSection(string section)
		{
			XmlNode root = doc.DocumentElement;
			XmlNode s = root.SelectSingleNode('/' + rootName + '/' + section);

			if(s == null)
				return;  //not found

			s.RemoveAll();
		}

		//initializes a new settings file with the XML version
		//and the root node
		protected void CreateSettingsDocument()
		{
			doc.AppendChild(doc.CreateXmlDeclaration("1.0", null, null));
			doc.AppendChild(doc.CreateElement(rootName));
		}

		public void Flush()
		{
			try
			{
				doc.Save(fileName);
			}
			catch(Exception ex)
			{
            	MessageBox.Show(ex.Message);
			}
		}

		//removes a section and all its entries
		public void RemoveSection(string section)
		{
			XmlNode root = doc.DocumentElement;
			XmlNode s = root.SelectSingleNode('/' + rootName + '/' + section);

			if(s != null)
				root.RemoveChild(s);
		}

		public void Save()
		{
			Flush();
		}


		#region Read methods

		protected delegate T ConverterDelegate<T>(string s);  

		protected T ReadValue<T>(string section, string name, T defaultValue, ConverterDelegate<T> converter)
		{
			string s = ReadString(section, name, "");

			if (string.IsNullOrEmpty(s))
				return defaultValue;
			try
			{
				return converter(s);
			}
			catch (FormatException)
			{
				return defaultValue;
			}
		}

		public bool ReadBool(string section, string name, bool defaultValue)
		{
			return ReadValue(section, name, defaultValue, v => Convert.ToBoolean(v, CultureInfo.InvariantCulture));
		}

		public DateTime ReadDateTime(string section, string name, DateTime defaultValue)
		{
			return ReadValue(section, name, defaultValue, v => Convert.ToDateTime(v, CultureInfo.InvariantCulture));
		}

		public double ReadDouble(string section, string name, double defaultValue)
		{
			return ReadValue(section, name, defaultValue, v => Convert.ToDouble(v, CultureInfo.InvariantCulture));
		}

		public float ReadFloat(string section, string name, float defaultValue)
		{
			return ReadValue(section, name, defaultValue, v => Convert.ToSingle(v, CultureInfo.InvariantCulture));
		}

		public int ReadInt(string section, string name, int defaultValue)
		{
			return ReadValue(section, name, defaultValue, v => Convert.ToInt32(v, CultureInfo.InvariantCulture));
		}

		public long ReadLong(string section, string name, long defaultValue)
		{
			return ReadValue(section, name, defaultValue, v => Convert.ToInt64(v, CultureInfo.InvariantCulture));
		}

		public short ReadShort(string section, string name, short defaultValue)
		{
			return ReadValue(section, name, defaultValue, v => Convert.ToInt16(v, CultureInfo.InvariantCulture));
		}

		public string ReadString(string section, string name, string defaultValue)
		{
			XmlNode root = doc.DocumentElement;
			XmlNode s = root.SelectSingleNode('/' + rootName + '/' + section);

			if(s == null)
				return defaultValue;  //not found

			XmlNode n = s.SelectSingleNode(name);

			if(n == null)
				return defaultValue;  //not found

			XmlAttributeCollection attrs = n.Attributes;

			foreach(XmlAttribute attr in attrs)
			{
				if(attr.Name == "value")
					return attr.Value;
			}

			return defaultValue;
		}

		public uint ReadUInt(string section, string name, uint defaultValue)
		{
			return ReadValue(section, name, defaultValue, v => Convert.ToUInt32(v, CultureInfo.InvariantCulture));
		}

		public ulong ReadULong(string section, string name, ulong defaultValue)
		{
			return ReadValue(section, name, defaultValue, v => Convert.ToUInt64(v, CultureInfo.InvariantCulture));
		}

		public ushort ReadUShort(string section, string name, ushort defaultValue)
		{
			return ReadValue(section, name, defaultValue, v => Convert.ToUInt16(v, CultureInfo.InvariantCulture));
		}

		#endregion


		#region Write methods

		public void WriteBool(string section, string name, bool value)
		{
			WriteString(section, name, value.ToString());
		}

		public void WriteDateTime(string section, string name, DateTime value)
		{
			WriteString(section, name, value.ToString(CultureInfo.InvariantCulture));
		}

		public void WriteDouble(string section, string name, double value)
		{
			WriteString(section, name, value.ToString(CultureInfo.InvariantCulture));
		}

		public void WriteFloat(string section, string name, float value)
		{
			WriteString(section, name, value.ToString(CultureInfo.InvariantCulture));
		}

		public void WriteInt(string section, string name, int value)
		{
			WriteString(section, name, value.ToString(CultureInfo.InvariantCulture));
		}

		public void WriteLong(string section, string name, long value)
		{
			WriteString(section, name, value.ToString(CultureInfo.InvariantCulture));
		}

		public void WriteShort(string section, string name, short value)
		{
			WriteString(section, name, value.ToString(CultureInfo.InvariantCulture));
		}

		public void WriteString(string section, string name, string value)
		{
			XmlNode root = doc.DocumentElement;
			XmlNode s = root.SelectSingleNode('/' + rootName + '/' + section);

			if(s == null)
				s =	root.AppendChild(doc.CreateElement(section));

			XmlNode n = s.SelectSingleNode(name);

			if(n == null)
				n = s.AppendChild(doc.CreateElement(name));

			XmlAttribute attr = ((XmlElement)n).SetAttributeNode("value", "");
			attr.Value = value;
		}

		public void WriteUInt(string section, string name, uint value)
		{
			WriteString(section, name, value.ToString(CultureInfo.InvariantCulture));
		}

        public void WriteULong(string section, string name, ulong value)
		{
			WriteString(section, name, value.ToString(CultureInfo.InvariantCulture));
		}

		public void WriteUShort(string section, string name, ushort value)
		{
			WriteString(section, name, value.ToString(CultureInfo.InvariantCulture));
		}

		#endregion

	}
}
