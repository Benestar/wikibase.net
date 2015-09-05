using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using Wikibase.DataModel;

namespace Wikibase.Tests.DataModel
{
    [TestClass]
    public class PropertyTest
    {
        [TestMethod]
        public void TestId()
        {
            Property property = new Property(new PropertyId("P42"));
            Assert.AreEqual("P42", property.Id.Serialization);
        }

        [TestMethod]
        public void TestPropertyId()
        {
            Property property = new Property(new PropertyId("P42"));
            Assert.AreEqual("P42", property.PropertyId.Serialization);
        }

        [TestMethod]
        public void TestFingerprint()
        {
            Property property = new Property(
                new PropertyId("P42"),
                new Fingerprint(new List<Term> { new Term("en", "foo") }, null, null),
                null
            );

            Assert.IsFalse(property.Empty);
            Assert.AreEqual("foo", property.Fingerprint.Labels["en"].Text);
        }

        [TestMethod]
        public void TestStatements()
        {
            Property property = new Property(
                new PropertyId("P42"),
                null,
                new List<Statement> { new Statement(new PropertyNoValueSnak("P42")) }
            );

            Assert.IsFalse(property.Empty);
            Assert.AreEqual(1, property.Statements.Count);
            Assert.AreEqual("P42", property.Statements[0].PropertyId.Serialization);
        }

        [TestMethod]
        public void TestEmpty()
        {
            Property property = new Property();
            Assert.IsTrue(property.Empty);
        }

        [TestMethod]
        public void TestEmptyWithId()
        {
            Property property = new Property(new PropertyId("P42"));
            Assert.IsTrue(property.Empty);
        }
    }
}
