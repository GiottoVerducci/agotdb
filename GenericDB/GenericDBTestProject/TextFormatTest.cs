using GenericDB.BusinessObjects;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Drawing;

namespace GenericDBTestProject
{
	/// <summary>
	///This is a test class for TextFormatTest and is intended
	///to contain all TextFormatTest Unit Tests
	///</summary>
	[TestClass()]
	public class TextFormatTest
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
		///A test for TextFormat Constructor
		///</summary>
		[TestMethod()]
		public void TextFormatConstructorTest2()
		{
			string name = "test";
			FontStyle style = FontStyle.Bold | FontStyle.Italic;
			TextFormat target = new TextFormat(name, style);
			Assert.IsTrue(target.Color == TextFormat.DefaultColor);
			Assert.IsTrue(target.Name == name);
			Assert.IsTrue(target.Style == style);
		}

		/// <summary>
		///A test for TextFormat Constructor
		///</summary>
		[TestMethod()]
		public void TextFormatConstructorTest1()
		{
			string name = "test";
			Color color = Color.DarkSlateGray;
			TextFormat target = new TextFormat(name, color);
			Assert.IsTrue(target.Color == color);
			Assert.IsTrue(target.Name == name);
			Assert.IsTrue(target.Style == TextFormat.DefaultFontStyle);
		}

		/// <summary>
		///A test for TextFormat Constructor
		///</summary>
		[TestMethod()]
		public void TextFormatConstructorTest()
		{
			string name = "test";
			FontStyle style = FontStyle.Strikeout;
			Color color = Color.DarkSlateGray;
			TextFormat target = new TextFormat(name, style, color);
			Assert.IsTrue(target.Color == color);
			Assert.IsTrue(target.Name == name);
			Assert.IsTrue(target.Style == style);
		}

		/// <summary>
		///A test for Merge
		///</summary>
		[TestMethod()]
		public void MergeTest()
		{
			TextFormat format1 = new TextFormat("babar", FontStyle.Bold);
			TextFormat format2 = new TextFormat("roi", Color.Cyan);
			TextFormat actual;
			actual = TextFormat.Merge(format1, format2);
			Assert.IsTrue(actual.Name == "babar+roi");
			Assert.IsTrue(actual.Color == Color.Cyan);
			Assert.IsTrue(actual.Style == FontStyle.Bold);

			format1 = new TextFormat("bibi", Color.Red);
			actual = TextFormat.Merge(format2, format1);
			Assert.IsTrue(actual.Name == "roi+bibi");
			Assert.IsTrue(actual.Color == Color.Red);
			Assert.IsTrue(actual.Style == FontStyle.Regular);
		}
	}
}
