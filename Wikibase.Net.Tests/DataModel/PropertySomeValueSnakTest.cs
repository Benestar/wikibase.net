using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Wikibase.DataModel;

namespace Wikibase.Tests.DataModel
{
    [TestClass]
    public class PropertySomeValueSnakTest
    {
        [TestMethod]
        public void TestType()
        {
            PropertySomeValueSnak snak = new PropertySomeValueSnak(new PropertyId("P42"));
            Assert.AreEqual(SnakType.SomeValueSnak, snak.Type);
        }

        [TestMethod]
        public void TestPropertyId()
        {
            PropertySomeValueSnak snak = new PropertySomeValueSnak(new PropertyId("P42"));
            Assert.AreEqual("P42", snak.PropertyId.Serialization);
        }

        [TestMethod]
        public void TestPropertyIdSerialization()
        {
            PropertySomeValueSnak snak = new PropertySomeValueSnak("P42");
            Assert.AreEqual("P42", snak.PropertyId.Serialization);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TestPropertyIdNull()
        {
            new PropertySomeValueSnak((PropertyId)null);
        }

        [TestMethod]
        public void TestIsSnak()
        {
            Snak snak = new PropertySomeValueSnak("P42");
            Assert.IsInstanceOfType(snak, typeof(Snak));
        }
    }
}
