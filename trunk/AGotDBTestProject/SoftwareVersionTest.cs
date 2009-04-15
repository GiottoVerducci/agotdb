using AGoT.AGoTDB.BusinessObjects;
using Microsoft.VisualStudio.TestTools.UnitTesting;
namespace AGotDBTestProject
{
  /// <summary>
  ///This is a test class for SoftwareVersionTest and is intended
  ///to contain all SoftwareVersionTest Unit Tests
  ///</summary>
  [TestClass()]
  public class SoftwareVersionTest
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
    ///A test for CompareTo
    ///</summary>
    [TestMethod()]
    public void CompareToTest()
    {
      Assert.IsTrue(0 == new SoftwareVersion(1, 2, 16000).CompareTo(new SoftwareVersion(1, 2, 16000)));
      Assert.IsTrue(0 < new SoftwareVersion(1, 2, 16001).CompareTo(new SoftwareVersion(1, 2, 16000)));
      Assert.IsTrue(0 < new SoftwareVersion(2, 2, 0).CompareTo(new SoftwareVersion(1, 2, 16000)));
      Assert.IsTrue(0 < new SoftwareVersion(1, 3, 0).CompareTo(new SoftwareVersion(1, 2, 16000)));
      Assert.IsTrue(0 > new SoftwareVersion(0, 16, 16000).CompareTo(new SoftwareVersion(1, 2, 16000)));
      Assert.IsTrue(0 > new SoftwareVersion(0, 750, 0).CompareTo(new SoftwareVersion(0, 765, 0)));
    }
  }
}
