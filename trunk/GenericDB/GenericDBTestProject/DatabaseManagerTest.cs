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

			Assert.AreEqual(0, actual.Formats.Count);
			Assert.AreEqual("Hello world", actual.Value);

			actual = DatabaseManager_Accessor.ExtractFormattedStringValueFromRow(row, columnName,
				new BoundFormat('{', '}', new TextFormat("errata", Color.Red)));

			Assert.AreEqual(0, actual.Formats.Count);
			Assert.AreEqual("Hello world", actual.Value);
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

			Assert.AreEqual(0, actual.Formats.Count);
			Assert.AreEqual("Hello {world}, the ~sun~ is shinning", actual.Value);

			var errataFormat = new TextFormat("errata", Color.Red);
			var specialFormat = new TextFormat("special", Color.Green);

			actual = DatabaseManager_Accessor.ExtractFormattedStringValueFromRow(row, columnName,
				new BoundFormat('{', '}', errataFormat),
				new BoundFormat('~', specialFormat));

			Assert.AreEqual(2, actual.Formats.Count);
			Assert.AreEqual("Hello world, the sun is shinning", actual.Value);
			Assert.AreEqual(6, actual.Formats[0].Begin);
			Assert.AreEqual(11, actual.Formats[0].End);
			Assert.AreEqual(errataFormat, actual.Formats[0].Format);
			Assert.AreEqual(17, actual.Formats[1].Begin);
			Assert.AreEqual(20, actual.Formats[1].End);
			Assert.AreEqual(specialFormat, actual.Formats[1].Format);
		}

		/// <summary>
		///A test for ExtractFormattedStringValueFromRow
		///</summary>
		[TestMethod()]
		public void ExtractFormattedStringValueFromRowTest_WithFormatAndEmptySection()
		{
			string columnName = "col";
			var table = new DataTable("test");
			var textColumn = new DataColumn(columnName);
			textColumn.DataType = typeof(string);
			table.Columns.Add(textColumn);

			var row = table.NewRow();
			row[columnName] = "Hello {world}, {}the~~ ~sun~ is shinning";

			var errataFormat = new TextFormat("errata", Color.Red);
			var specialFormat = new TextFormat("special", Color.Green);

			var actual = DatabaseManager_Accessor.ExtractFormattedStringValueFromRow(row, columnName,
				new BoundFormat('{', '}', errataFormat),
				new BoundFormat('~', specialFormat));

			Assert.AreEqual(4, actual.Formats.Count);
			Assert.AreEqual("Hello world, {}the~~ sun is shinning", actual.Value);
			Assert.AreEqual(6, actual.Formats[0].Begin);
			Assert.AreEqual(11, actual.Formats[0].End);
			Assert.AreEqual(errataFormat, actual.Formats[0].Format);
			Assert.AreEqual(13, actual.Formats[1].Begin);
			Assert.AreEqual(15, actual.Formats[1].End);
			Assert.AreEqual(errataFormat, actual.Formats[1].Format);
			Assert.AreEqual(18, actual.Formats[2].Begin);
			Assert.AreEqual(20, actual.Formats[2].End);
			Assert.AreEqual(specialFormat, actual.Formats[2].Format);
			Assert.AreEqual(21, actual.Formats[3].Begin);
			Assert.AreEqual(24, actual.Formats[3].End);
			Assert.AreEqual(specialFormat, actual.Formats[3].Format);
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

			Assert.AreEqual(3, actual.Formats.Count);
			Assert.AreEqual("Hello world, the sun is shinning", actual.Value);
			Assert.AreEqual(6, actual.Formats[0].Begin);
			Assert.AreEqual(23, actual.Formats[0].End);
			Assert.AreEqual(errataFormat, actual.Formats[0].Format);
			Assert.AreEqual(17, actual.Formats[1].Begin);
			Assert.AreEqual(20, actual.Formats[1].End);
			Assert.AreEqual(specialFormat, actual.Formats[1].Format);
			Assert.AreEqual(18, actual.Formats[2].Begin);
			Assert.AreEqual(19, actual.Formats[2].End);
			Assert.AreEqual(errataFormat, actual.Formats[2].Format);
		}
	}
}
