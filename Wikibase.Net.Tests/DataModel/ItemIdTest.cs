using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Wikibase.DataModel;

namespace Wikibase.Tests.DataModel
{
    [TestClass]
    public class ItemIdTest
    {
        [TestMethod]
        public void TestValidSerialization()
        {
            ItemId itemId = new ItemId("Q42");
            Assert.AreEqual("Q42", itemId.Serialization);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void TestInvalidSerialization()
        {
            new ItemId("FooBar");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void TestPropertyIdSerialization()
        {
            new ItemId("P123");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void TestPrefixedSerialization()
        {
            new ItemId("Foo Q123");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void TestSuffixedSerialization()
        {
            new ItemId("Q123foo");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void TestZeroSerialization()
        {
            new ItemId("Q0123");
        }

        [TestMethod]
        public void TestEquals()
        {
            ItemId itemId = new ItemId("Q42");

            Assert.IsTrue(itemId.Equals(new ItemId("Q42")));
            Assert.IsFalse(itemId.Equals(new ItemId("Q123")));
        }
    }
}
