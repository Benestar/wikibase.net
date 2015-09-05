using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Wikibase.DataModel;
using System.Collections.Generic;

namespace Wikibase.Tests.DataModel
{
    [TestClass]
    public class StatementTest
    {
        [TestMethod]
        public void TestSnak()
        {
            Statement statement = new Statement(new PropertySomeValueSnak("P42"));
            Assert.AreEqual("P42", statement.Snak.PropertyId.Serialization);
            Assert.AreEqual(SnakType.SomeValueSnak, statement.Snak.Type);
        }

        [TestMethod]
        public void TestQualifiers()
        {
            Statement statement = new Statement(
                new PropertySomeValueSnak("P42"),
                new List<Snak> { new PropertyNoValueSnak("P42"), new PropertySomeValueSnak("P31") }
            );

            Assert.AreEqual(2, statement.Qualifiers.Count);

            Assert.AreEqual("P42", statement.Qualifiers[0].PropertyId.Serialization);
            Assert.AreEqual(SnakType.NoValueSnak, statement.Qualifiers[0].Type);

            Assert.AreEqual("P31", statement.Qualifiers[1].PropertyId.Serialization);
            Assert.AreEqual(SnakType.SomeValueSnak, statement.Qualifiers[1].Type);
        }

        [TestMethod]
        public void TestReferences()
        {
            Statement statement = new Statement(
                new PropertySomeValueSnak("P42"),
                new List<Snak>(),
                new List<Snak> { new PropertyNoValueSnak("P42"), new PropertySomeValueSnak("P31") }
            );

            Assert.AreEqual(2, statement.References.Count);

            Assert.AreEqual("P42", statement.References[0].PropertyId.Serialization);
            Assert.AreEqual(SnakType.NoValueSnak, statement.References[0].Type);

            Assert.AreEqual("P31", statement.References[1].PropertyId.Serialization);
            Assert.AreEqual(SnakType.SomeValueSnak, statement.References[1].Type);
        }

        [TestMethod]
        public void TestGuid()
        {
            Statement statement = new Statement(
                "statement-guid",
                new PropertySomeValueSnak("P42"),
                new List<Snak>(),
                new List<Snak>()
            );

            Assert.AreEqual("statement-guid", statement.Guid);
        }

        [TestMethod]
        public void TestPropertyId()
        {
            Statement statement = new Statement(new PropertySomeValueSnak("P42"));
            Assert.AreEqual("P42", statement.PropertyId.Serialization);
        }
    }
}
