using System;
using System.Collections.Generic;
using Wikibase.DataModel;
using Xunit;

namespace Wikibase.Tests.DataModel
{
    public class PropertyTest
    {
        [Fact]
        public void TestId()
        {
            Property property = new Property(new PropertyId("P42"));
            Assert.Equal("P42", property.Id.Serialization);
        }

        [Fact]
        public void TestPropertyId()
        {
            Property property = new Property(new PropertyId("P42"));
            Assert.Equal("P42", property.PropertyId.Serialization);
        }

        [Fact]
        public void TestFingerprint()
        {
            Property property = new Property(
                new PropertyId("P42"),
                new Fingerprint(new List<Term> { new Term("en", "foo") }, null, null),
                null
            );

            Assert.False(property.Empty);
            Assert.Equal("foo", property.Fingerprint.Labels["en"].Text);
        }

        [Fact]
        public void TestStatements()
        {
            Property property = new Property(
                new PropertyId("P42"),
                null,
                new List<Statement> { new Statement(new PropertyNoValueSnak("P42")) }
            );

            Assert.False(property.Empty);
            Assert.Equal(1, property.Statements.Count);
            Assert.Equal("P42", property.Statements[0].PropertyId.Serialization);
        }

        [Fact]
        public void TestEmpty()
        {
            Property property = new Property();
            Assert.True(property.Empty);
        }

        [Fact]
        public void TestEmptyWithId()
        {
            Property property = new Property(new PropertyId("P42"));
            Assert.True(property.Empty);
        }
    }
}
