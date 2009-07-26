using System.Drawing;
using GenericDB.BusinessObjects;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace GenericDBTestProject
{
	/// <summary>
	///This is a test class for CardTest and is intended
	///to contain all CardTest Unit Tests
	///</summary>
	[TestClass()]
	public class CardTest
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
		///A test for ApplyFormatsToFormattedText
		///</summary>
		[TestMethod()]
		[DeploymentItem("GenericDB.dll")]
		public void ApplyFormatsToFormattedTextTest()
		{
			var result = new List<FormattedText> { new FormattedText("Babar") };
			var boldFormat = new TextFormat("bold", FontStyle.Bold);
			var formats = new List<FormatSection> { new FormatSection(2, 4, boldFormat) };
			Card_Accessor.ApplyFormatsToFormattedText(result, formats);

			Assert.AreEqual(3, result.Count);
			Assert.AreEqual("Ba", result[0].Text);
			Assert.AreEqual(TextFormat.Regular, result[0].Format);
			Assert.AreEqual("ba", result[1].Text);
			Assert.AreEqual(boldFormat, result[1].Format);
			Assert.AreEqual("r", result[2].Text);
			Assert.AreEqual(TextFormat.Regular, result[2].Format);
		}

		/// <summary>
		///A test for ApplyFormatsToFormattedText
		///</summary>
		[TestMethod()]
		[DeploymentItem("GenericDB.dll")]
		public void ApplyFormatsToFormattedTextTest_NoFormat()
		{
			var result = new List<FormattedText> { new FormattedText("Babar") };
			var formats = new List<FormatSection>();
			Card_Accessor.ApplyFormatsToFormattedText(result, formats);

			Assert.AreEqual(1, result.Count);
			Assert.AreEqual("Babar", result[0].Text);
			Assert.AreEqual(TextFormat.Regular, result[0].Format);
		}

		/// <summary>
		///A test for ApplyFormatsToFormattedText
		///</summary>
		[TestMethod()]
		[DeploymentItem("GenericDB.dll")]
		public void ApplyFormatsToFormattedTextTest_Limits()
		{
			var result = new List<FormattedText> { new FormattedText("Babar") };
			var boldFormat = new TextFormat("bold", FontStyle.Bold);
			var formats = new List<FormatSection> { new FormatSection(0, 2, boldFormat), new FormatSection(3, 5, boldFormat) };
			Card_Accessor.ApplyFormatsToFormattedText(result, formats);

			Assert.AreEqual(3, result.Count);
			Assert.AreEqual("Ba", result[0].Text);
			Assert.AreEqual(boldFormat, result[0].Format);
			Assert.AreEqual("b", result[1].Text);
			Assert.AreEqual(TextFormat.Regular, result[1].Format);
			Assert.AreEqual("ar", result[2].Text);
			Assert.AreEqual(boldFormat, result[2].Format);
		}
	}
}
