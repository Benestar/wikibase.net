using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Wikibase.DataModel;

namespace Wikibase.Tests.DataModel
{
    [TestClass]
    public class PropertyNoValueSnakTest
    {
        [TestMethod]
        public void TestType()
        {
            PropertyNoValueSnak snak = new PropertyNoValueSnak(new PropertyId("P42"));
            Assert.AreEqual(SnakType.NoValueSnak, snak.Type);
        }

        [TestMethod]
        public void TestPropertyId()
        {
            PropertyNoValueSnak snak = new PropertyNoValueSnak(new PropertyId("P42"));
            Assert.AreEqual("P42", snak.PropertyId.Serialization);
        }

        [TestMethod]
        public void TestPropertyIdSerialization()
        {
            PropertyNoValueSnak snak = new PropertyNoValueSnak("P42");
            Assert.AreEqual("P42", snak.PropertyId.Serialization);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TestPropertyIdNull()
        {
            new PropertyNoValueSnak((PropertyId) null);
        }

        [TestMethod]
        public void TestIsSnak()
        {
            Snak snak = new PropertyNoValueSnak("P42");
            Assert.IsInstanceOfType(snak, typeof(Snak));
        }
    }
}
