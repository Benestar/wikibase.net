using System;
using Wikibase.DataModel;
using Wikibase.DataModel.DataValues;
using Xunit;

namespace Wikibase.Tests.DataModel
{
    public class PropertyValueSnakTest
    {
        [Fact]
        public void TestType()
        {
            PropertyValueSnak snak = new PropertyValueSnak(new PropertyId("P42"), new StringValue("foo"));
            Assert.Equal(SnakType.ValueSnak, snak.Type);
        }

        [Fact]
        public void TestPropertyId()
        {
            PropertyValueSnak snak = new PropertyValueSnak(new PropertyId("P42"), new StringValue("foo"));
            Assert.Equal("P42", snak.PropertyId.Serialization);
        }

        [Fact]
        public void TestPropertyIdSerialization()
        {
            PropertyValueSnak snak = new PropertyValueSnak(new PropertyId("P42"), new StringValue("foo"));
            Assert.Equal("P42", snak.PropertyId.Serialization);
        }

        [Fact]
        public void TestPropertyIdNull()
        {
            Assert.Throws<ArgumentNullException>(() => new PropertyValueSnak((PropertyId)null, new StringValue("foo")));
        }

        [Fact]
        public void TestValue()
        {
            PropertyValueSnak snak = new PropertyValueSnak(new PropertyId("P42"), new StringValue("foo"));
            Assert.Equal("foo", ((StringValue)snak.Value).Value);
        }

        [Fact]
        public void TestNullValue()
        {
            Assert.Throws<ArgumentNullException>(() => new PropertyValueSnak(new PropertyId("P42"), null));
        }

        [Fact]
        public void TestIsSnak()
        {
            PropertyValueSnak snak = new PropertyValueSnak(new PropertyId("P42"), new StringValue("foo"));
            Assert.IsAssignableFrom<Snak>(snak);
        }
    }
}
