using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Wikibase.DataModel;

namespace Wikibase.Tests.DataModel
{
    [TestClass]
    public class TermTest
    {
        [TestMethod]
        public void TestTerm()
        {
            Term term = new Term("en", "Test");

            Assert.AreEqual("en", term.Language);
            Assert.AreEqual("Test", term.Text);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TestNullLanguage()
        {
            new Term(null, "Test");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TestNullText()
        {
            new Term("en", null);
        }
    }
}
