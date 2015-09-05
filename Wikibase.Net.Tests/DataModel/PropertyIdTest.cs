using System;
using Wikibase.DataModel;
using Xunit;

namespace Wikibase.Tests.DataModel
{
    public class PropertyIdTest
    {
        [Fact]
        public void TestValidSerialization()
        {
            PropertyId PropertyId = new PropertyId("P42");
            Assert.Equal("P42", PropertyId.Serialization);
        }

        [Fact]
        public void TestInvalidSerialization()
        {
            Assert.Throws<ArgumentException>(() => new PropertyId("FooBar"));
        }

        [Fact]
        public void TestItemIdSerialization()
        {
            Assert.Throws<ArgumentException>(() => new PropertyId("Q123"));
        }

        [Fact]
        public void TestPrefixedSerialization()
        {
            Assert.Throws<ArgumentException>(() => new PropertyId("Foo P123"));
        }

        [Fact]
        public void TestSuffixedSerialization()
        {
            Assert.Throws<ArgumentException>(() => new PropertyId("P123foo"));
        }

        [Fact]
        public void TestZeroSerialization()
        {
            Assert.Throws<ArgumentException>(() => new PropertyId("P0123"));
        }

        [Fact]
        public void TestNullSerialization()
        {
            Assert.Throws<ArgumentNullException>(() => new PropertyId(null));
        }

        [Fact]
        public void TestEquals()
        {
            PropertyId propertyId = new PropertyId("P42");

            Assert.True(propertyId.Equals(new PropertyId("P42")));
            Assert.False(propertyId.Equals(new PropertyId("P123")));
        }
    }
}
