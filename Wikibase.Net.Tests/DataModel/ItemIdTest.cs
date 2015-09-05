using System;
using Wikibase.DataModel;
using Xunit;

namespace Wikibase.Tests.DataModel
{
    public class ItemIdTest
    {
        [Fact]
        public void TestValidSerialization()
        {
            ItemId itemId = new ItemId("Q42");
            Assert.Equal("Q42", itemId.Serialization);
        }

        [Fact]
        public void TestInvalidSerialization()
        {
            Assert.Throws<ArgumentException>(() => new ItemId("FooBar"));
        }

        [Fact]
        public void TestPropertyIdSerialization()
        {
            Assert.Throws<ArgumentException>(() => new ItemId("P123"));
        }

        [Fact]
        public void TestPrefixedSerialization()
        {
            Assert.Throws<ArgumentException>(() => new ItemId("Foo Q123"));
        }

        [Fact]
        public void TestSuffixedSerialization()
        {
            Assert.Throws<ArgumentException>(() => new ItemId("Q123foo"));
        }

        [Fact]
        public void TestZeroSerialization()
        {
            Assert.Throws<ArgumentException>(() => new ItemId("Q0123"));
        }

        [Fact]
        public void TestNullSerialization()
        {
            Assert.Throws<ArgumentNullException>(() => new ItemId(null));
        }

        [Fact]
        public void TestEquals()
        {
            ItemId itemId = new ItemId("Q42");

            Assert.True(itemId.Equals(new ItemId("Q42")));
            Assert.False(itemId.Equals(new ItemId("Q123")));
        }
    }
}
