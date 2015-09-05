using System;
using Wikibase.DataModel;
using Xunit;

namespace Wikibase.Tests.DataModel
{
    public class PropertyNoValueSnakTest
    {
        [Fact]
        public void TestType()
        {
            PropertyNoValueSnak snak = new PropertyNoValueSnak(new PropertyId("P42"));
            Assert.Equal(SnakType.NoValueSnak, snak.Type);
        }

        [Fact]
        public void TestPropertyId()
        {
            PropertyNoValueSnak snak = new PropertyNoValueSnak(new PropertyId("P42"));
            Assert.Equal("P42", snak.PropertyId.Serialization);
        }

        [Fact]
        public void TestPropertyIdSerialization()
        {
            PropertyNoValueSnak snak = new PropertyNoValueSnak("P42");
            Assert.Equal("P42", snak.PropertyId.Serialization);
        }

        [Fact]
        public void TestPropertyIdNull()
        {
            Assert.Throws<ArgumentNullException>(() => new PropertyNoValueSnak((PropertyId)null));
        }

        [Fact]
        public void TestIsSnak()
        {
            PropertyNoValueSnak snak = new PropertyNoValueSnak("P42");
            Assert.IsAssignableFrom<Snak>(snak);
        }
    }
}
