using System;
using System.Collections.Generic;
using Wikibase.DataModel;
using Xunit;

namespace Wikibase.Tests.DataModel
{
    public class ItemTest
    {
        [Fact]
        public void TestId()
        {
            Item item = new Item(new ItemId("Q42"));
            Assert.Equal("Q42", item.Id.Serialization);
        }

        [Fact]
        public void TestItemId()
        {
            Item item = new Item(new ItemId("Q42"));
            Assert.Equal("Q42", item.ItemId.Serialization);
        }

        [Fact]
        public void TestFingerprint()
        {
            Item item = new Item(
                new ItemId("Q42"),
                new Fingerprint( new List<Term> { new Term("en", "foo") }, null, null ),
                null,
                null
            );

            Assert.False(item.Empty);
            Assert.Equal("foo", item.Fingerprint.Labels["en"].Text);
        }

        [Fact]
        public void TestStatements()
        {
            Item item = new Item(
                new ItemId("Q42"),
                null,
                new List<Statement> { new Statement(new PropertyNoValueSnak("P42")) },
                null
            );

            Assert.False(item.Empty);
            Assert.Equal(1, item.Statements.Count);
            Assert.Equal("P42", item.Statements[0].PropertyId.Serialization);
        }

        [Fact]
        public void TestSitelinks()
        {
            Item item = new Item(
                new ItemId("Q42"),
                null,
                null,
                new List<SiteLink> { new SiteLink("enwiki", "Foo"), new SiteLink("dewiki", "Bar") }
            );

            Assert.False(item.Empty);
            Assert.Equal(2, item.Sitelinks.Count);
            Assert.Equal(item.Sitelinks["enwiki"].PageName, "Foo");
            Assert.Equal(item.Sitelinks["dewiki"].PageName, "Bar");
        }

        [Fact]
        public void TestEmpty()
        {
            Item item = new Item();
            Assert.True(item.Empty);
        }

        [Fact]
        public void TestEmptyWithId()
        {
            Item item = new Item(new ItemId("Q42"));
            Assert.True(item.Empty);
        }
    }
}
