using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GenericDBTests
{
    using System.IO;
    using System.Linq;
    using System.Text;

    using GenericDB.DataAccess;

    [TestClass]
    public class CsvManagerTests
    {
        [TestMethod]
        public void RegularTest()
        {
            var filePath = System.IO.Path.GetTempFileName();

            try
            {
                using (var writer = new StreamWriter(filePath, false, Encoding.UTF8))
                {
                    writer.WriteLine("A;B b;\"C\"");
                    writer.WriteLine("Some text;\"Some\r\nmulti-line\ntext\";Some text with \"\" quotes inside");
                    writer.WriteLine("\"Some text\";\"Some\r\nmulti-line\ntext\";\"Some text with \"\" quotes inside\"");
                }

                var csvManager = new CsvManager(filePath, ';', '"', true);

                Assert.AreEqual(2, csvManager.Data.Count);
                Assert.IsTrue(csvManager.HasHeader);
                Assert.IsTrue(csvManager.Data.ColumnNames.SequenceEqual(new[] { "A", "B b", "C" }));

                var row = csvManager.Data[0];
                Assert.AreEqual("Some text", row[0]);
                Assert.AreEqual("Some text", row["A"]);
                Assert.AreEqual("Some\r\nmulti-line\ntext", row[1]);
                Assert.AreEqual("Some\r\nmulti-line\ntext", row["B b"]);
                Assert.AreEqual("Some text with \" quotes inside", row[2]);
                Assert.AreEqual("Some text with \" quotes inside", row["C"]);

                row = csvManager.Data[1];
                Assert.AreEqual("Some text", row[0]);
                Assert.AreEqual("Some text", row["A"]);
                Assert.AreEqual("Some\r\nmulti-line\ntext", row[1]);
                Assert.AreEqual("Some\r\nmulti-line\ntext", row["B b"]);
                Assert.AreEqual("Some text with \" quotes inside", row[2]);
                Assert.AreEqual("Some text with \" quotes inside", row["C"]);
            }
            finally
            {
                File.Delete(filePath);
            }
        }
    }
}
