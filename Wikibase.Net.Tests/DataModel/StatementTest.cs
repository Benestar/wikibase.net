using System;
using System.Collections.Generic;
using Wikibase.DataModel;
using Xunit;

namespace Wikibase.Tests.DataModel
{
    public class StatementTest
    {
        [Fact]
        public void TestSnak()
        {
            Statement statement = new Statement(new PropertySomeValueSnak("P42"));
            Assert.Equal("P42", statement.Snak.PropertyId.Serialization);
            Assert.Equal(SnakType.SomeValueSnak, statement.Snak.Type);
        }

        [Fact]
        public void TestQualifiers()
        {
            Statement statement = new Statement(
                new PropertySomeValueSnak("P42"),
                new List<Snak> { new PropertyNoValueSnak("P42"), new PropertySomeValueSnak("P31") }
            );

            Assert.Equal(2, statement.Qualifiers.Count);

            Assert.Equal("P42", statement.Qualifiers[0].PropertyId.Serialization);
            Assert.Equal(SnakType.NoValueSnak, statement.Qualifiers[0].Type);

            Assert.Equal("P31", statement.Qualifiers[1].PropertyId.Serialization);
            Assert.Equal(SnakType.SomeValueSnak, statement.Qualifiers[1].Type);
        }

        [Fact]
        public void TestReferences()
        {
            Statement statement = new Statement(
                new PropertySomeValueSnak("P42"),
                new List<Snak>(),
                new List<Snak> { new PropertyNoValueSnak("P42"), new PropertySomeValueSnak("P31") }
            );

            Assert.Equal(2, statement.References.Count);

            Assert.Equal("P42", statement.References[0].PropertyId.Serialization);
            Assert.Equal(SnakType.NoValueSnak, statement.References[0].Type);

            Assert.Equal("P31", statement.References[1].PropertyId.Serialization);
            Assert.Equal(SnakType.SomeValueSnak, statement.References[1].Type);
        }

        [Fact]
        public void TestGuid()
        {
            Statement statement = new Statement(
                "statement-guid",
                new PropertySomeValueSnak("P42"),
                new List<Snak>(),
                new List<Snak>()
            );

            Assert.Equal("statement-guid", statement.Guid);
        }

        [Fact]
        public void TestPropertyId()
        {
            Statement statement = new Statement(new PropertySomeValueSnak("P42"));
            Assert.Equal("P42", statement.PropertyId.Serialization);
        }
    }
}
