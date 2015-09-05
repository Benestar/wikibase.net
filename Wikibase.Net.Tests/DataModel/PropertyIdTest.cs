using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Wikibase.DataModel;

namespace Wikibase.Tests.DataModel
{
    [TestClass]
    public class PropertyIdTest
    {
        [TestMethod]
        public void TestValidSerialization()
        {
            PropertyId PropertyId = new PropertyId("P42");
            Assert.AreEqual("P42", PropertyId.Serialization);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void TestInvalidSerialization()
        {
            new PropertyId("FooBar");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void TestItemIdSerialization()
        {
            new PropertyId("Q123");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void TestPrefixedSerialization()
        {
            new PropertyId("Foo P123");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void TestSuffixedSerialization()
        {
            new PropertyId("P123foo");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void TestZeroSerialization()
        {
            new PropertyId("P0123");
        }

        [TestMethod]
        public void TestEquals()
        {
            PropertyId propertyId = new PropertyId("P42");

            Assert.IsTrue(propertyId.Equals(new PropertyId("P42")));
            Assert.IsFalse(propertyId.Equals(new PropertyId("P123")));
        }
    }
}
