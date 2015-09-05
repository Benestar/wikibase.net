using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Wikibase.DataModel;
using Wikibase.DataModel.DataValues;

namespace Wikibase.Tests.DataModel
{
    [TestClass]
    public class PropertyValueSnakTest
    {
        [TestMethod]
        public void TestType()
        {
            PropertyValueSnak snak = new PropertyValueSnak(new PropertyId("P42"), new StringValue("foo"));
            Assert.AreEqual(SnakType.ValueSnak, snak.Type);
        }

        [TestMethod]
        public void TestPropertyId()
        {
            PropertyValueSnak snak = new PropertyValueSnak(new PropertyId("P42"), new StringValue("foo"));
            Assert.AreEqual("P42", snak.PropertyId.Serialization);
        }

        [TestMethod]
        public void TestPropertyIdSerialization()
        {
            PropertyValueSnak snak = new PropertyValueSnak(new PropertyId("P42"), new StringValue("foo"));
            Assert.AreEqual("P42", snak.PropertyId.Serialization);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TestPropertyIdNull()
        {
            new PropertyValueSnak((PropertyId)null, new StringValue("foo"));
        }

        [TestMethod]
        public void TestValue()
        {
            PropertyValueSnak snak = new PropertyValueSnak(new PropertyId("P42"), new StringValue("foo"));
            Assert.AreEqual("foo", ((StringValue)snak.Value).Value);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TestNullValue()
        {
            new PropertyValueSnak(new PropertyId("P42"), null);
        }

        [TestMethod]
        public void TestIsSnak()
        {
            Snak snak = new PropertyValueSnak(new PropertyId("P42"), new StringValue("foo"));
            Assert.IsInstanceOfType(snak, typeof(Snak));
        }
    }
}
