using System;
using Wikibase.DataModel;
using Xunit;

namespace Wikibase.Tests.DataModel
{
    public class PropertySomeValueSnakTest
    {
        [Fact]
        public void TestType()
        {
            PropertySomeValueSnak snak = new PropertySomeValueSnak(new PropertyId("P42"));
            Assert.Equal(SnakType.SomeValueSnak, snak.Type);
        }

        [Fact]
        public void TestPropertyId()
        {
            PropertySomeValueSnak snak = new PropertySomeValueSnak(new PropertyId("P42"));
            Assert.Equal("P42", snak.PropertyId.Serialization);
        }

        [Fact]
        public void TestPropertyIdSerialization()
        {
            PropertySomeValueSnak snak = new PropertySomeValueSnak("P42");
            Assert.Equal("P42", snak.PropertyId.Serialization);
        }

        [Fact]
        public void TestPropertyIdNull()
        {
            Assert.Throws<ArgumentNullException>(() => new PropertySomeValueSnak((PropertyId)null));
        }

        [Fact]
        public void TestIsSnak()
        {
            PropertySomeValueSnak snak = new PropertySomeValueSnak("P42");
            Assert.IsAssignableFrom<Snak>(snak);
        }
    }
}
