using System;
using System.Drawing;
using GenericDB.DataAccess;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Data;
using GenericDB.BusinessObjects;

namespace GenericDBTestProject
{
	/// <summary>
	///This is a test class for DatabaseManagerTest and is intended
	///to contain all DatabaseManagerTest Unit Tests
	///</summary>
	[TestClass()]
	public class DatabaseManagerTest
	{
		/// <summary>
		///Gets or sets the test context which provides
		///information about and functionality for the current test run.
		///</summary>
		public TestContext TestContext { get; set; }

		#region Additional test attributes
		// 
		//You can use the following additional attributes as you write your tests:
		//
		//Use ClassInitialize to run code before running the first test in the class
		//[ClassInitialize()]
		//public static void MyClassInitialize(TestContext testContext)
		//{
		//}
		//
		//Use ClassCleanup to run code after all tests in a class have run
		//[ClassCleanup()]
		//public static void MyClassCleanup()
		//{
		//}
		//
		//Use TestInitialize to run code before running each test
		//[TestInitialize()]
		//public void MyTestInitialize()
		//{
		//}
		//
		//Use TestCleanup to run code after each test has run
		//[TestCleanup()]
		//public void MyTestCleanup()
		//{
		//}
		//
		#endregion


		/// <summary>
		///A test for ExtractFormattedStringValueFromRow
		///</summary>
		[TestMethod()]
		public void ExtractFormattedStringValueFromRowTest_NoFormat()
		{
			string columnName = "col";
			var table = new DataTable("test");
			var textColumn = new DataColumn(columnName);
			textColumn.DataType = typeof(string);
			table.Columns.Add(textColumn);

			var row = table.NewRow();
			row[columnName] = "Hello world";

			var actual = DatabaseManager_Accessor.ExtractFormattedStringValueFromRow(row, columnName);

			Assert.IsTrue(actual.Formats.Count == 0);
			Assert.IsTrue(actual.Value == "Hello world");

			actual = DatabaseManager_Accessor.ExtractFormattedStringValueFromRow(row, columnName,
				new BoundFormat('{', '}', new TextFormat("errata", Color.Red)));

			Assert.IsTrue(actual.Formats.Count == 0);
			Assert.IsTrue(actual.Value == "Hello world");
		}

		/// <summary>
		///A test for ExtractFormattedStringValueFromRow
		///</summary>
		[TestMethod()]
		public void ExtractFormattedStringValueFromRowTest_WithFormat()
		{
			string columnName = "col";
			var table = new DataTable("test");
			var textColumn = new DataColumn(columnName);
			textColumn.DataType = typeof(string);
			table.Columns.Add(textColumn);

			var row = table.NewRow();
			row[columnName] = "Hello {world}, the ~sun~ is shinning";

			var actual = DatabaseManager_Accessor.ExtractFormattedStringValueFromRow(row, columnName);

			Assert.IsTrue(actual.Formats.Count == 0);
			Assert.IsTrue(actual.Value == "Hello {world}, the ~sun~ is shinning");

			var errataFormat = new TextFormat("errata", Color.Red);
			var specialFormat = new TextFormat("special", Color.Green);

			actual = DatabaseManager_Accessor.ExtractFormattedStringValueFromRow(row, columnName,
				new BoundFormat('{', '}', errataFormat),
				new BoundFormat('~', specialFormat));

			Assert.IsTrue(actual.Formats.Count == 2);
			Assert.IsTrue(actual.Value == "Hello world, the sun is shinning");
			Assert.IsTrue(actual.Formats[0].Begin == 6);
			Assert.IsTrue(actual.Formats[0].End == 11);
			Assert.IsTrue(actual.Formats[0].Format == errataFormat);
			Assert.IsTrue(actual.Formats[1].Begin == 17);
			Assert.IsTrue(actual.Formats[1].End == 20);
			Assert.IsTrue(actual.Formats[1].Format == specialFormat);
		}

		/// <summary>
		///A test for ExtractFormattedStringValueFromRow
		///</summary>
		[TestMethod()]
		public void ExtractFormattedStringValueFromRowTest_WithImbricatedFormat()
		{
			string columnName = "col";
			var table = new DataTable("test");
			var textColumn = new DataColumn(columnName);
			textColumn.DataType = typeof(string);
			table.Columns.Add(textColumn);

			var row = table.NewRow();
			row[columnName] = "Hello {world, the ~s{u}n~ is} shinning";

			var errataFormat = new TextFormat("errata", Color.Red);
			var specialFormat = new TextFormat("special", Color.Green);

			var actual = DatabaseManager_Accessor.ExtractFormattedStringValueFromRow(row, columnName,
				new BoundFormat('{', '}', errataFormat),
				new BoundFormat('~', specialFormat));

			Assert.IsTrue(actual.Formats.Count == 3);
			Assert.IsTrue(actual.Value == "Hello world, the sun is shinning");
			Assert.IsTrue(actual.Formats[0].Begin == 6);
			Assert.IsTrue(actual.Formats[0].End == 23);
			Assert.IsTrue(actual.Formats[0].Format == errataFormat);
			Assert.IsTrue(actual.Formats[1].Begin == 17);
			Assert.IsTrue(actual.Formats[1].End == 20);
			Assert.IsTrue(actual.Formats[1].Format == specialFormat);
			Assert.IsTrue(actual.Formats[2].Begin == 18);
			Assert.IsTrue(actual.Formats[2].End == 19);
			Assert.IsTrue(actual.Formats[2].Format == errataFormat);
		}
	}
}
