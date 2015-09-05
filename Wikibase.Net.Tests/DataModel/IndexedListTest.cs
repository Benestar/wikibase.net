using System;
using System.Collections.Generic;
using Wikibase.DataModel;
using Xunit;

namespace Wikibase.Tests.DataModel
{
    public class IndexedListTest
    {
        [Fact]
        public void TestFromList()
        {
            IndexedList<string, SiteLink> list = new IndexedList<string, SiteLink> (
                siteLink => siteLink.SiteId,
                new List<SiteLink> { new SiteLink("enwiki", "Foo") }
            );

            Assert.Equal("Foo", list["enwiki"].PageName);
        }

        [Fact]
        public void TestDuplicateKey()
        {
            Assert.Throws<ArgumentException>(
                () => new IndexedList<string, SiteLink>(siteLink => siteLink.SiteId) {
                    new SiteLink("dewiki", "Foo"),
                    new SiteLink("dewiki", "Bar")
                }
            );
        }

        [Fact]
        public void TestNullKey()
        {
            Assert.Throws<ArgumentNullException>(
                () => new IndexedList<string, SiteLink>(siteLink => siteLink.SiteId) {
                    new SiteLink()
                }
            );
        }

        [Fact]
        public void TestAccess()
        {
            IndexedList<string, SiteLink> list = new IndexedList<string, SiteLink> (siteLink => siteLink.SiteId) {
                new SiteLink("enwiki", "Foo"),
                new SiteLink("dewiki", "Bar")
            };

            Assert.Equal("Foo", list["enwiki"].PageName);
            Assert.Equal("Bar", list["dewiki"].PageName);
        }

        [Fact]
        public void TestAccessNotExisting()
        {
            IndexedList<string, SiteLink> list = new IndexedList<string, SiteLink>(siteLink => siteLink.SiteId) {
                new SiteLink("enwiki", "Foo"),
                new SiteLink("dewiki", "Bar")
            };

            Assert.Throws<KeyNotFoundException>(() => list["xxwiki"]);
        }

        [Fact]
        public void TestAdd()
        {
            IndexedList<string, SiteLink> list = new IndexedList<string, SiteLink> (siteLink => siteLink.SiteId) {
                new SiteLink("enwiki", "Foo")
            };
            list.Add(new SiteLink("dewiki", "Bar"));

            Assert.Equal("Foo", list["enwiki"].PageName);
            Assert.Equal("Bar", list["dewiki"].PageName);
        }

        [Fact]
        public void TestAddNullKey()
        {
            IndexedList<string, SiteLink> list = new IndexedList<string, SiteLink>(siteLink => siteLink.SiteId) {
                new SiteLink("enwiki", "Foo")
            };

            Assert.Throws<ArgumentNullException>(() => list.Add(new SiteLink()));
        }

        [Fact]
        public void TestSet()
        {
            IndexedList<string, SiteLink> list = new IndexedList<string, SiteLink> (siteLink => siteLink.SiteId) {
                new SiteLink("enwiki", "Foo")
            };
            list.Set(new SiteLink("enwiki", "Bar"));

            Assert.Equal("Bar", list["enwiki"].PageName);
        }

        [Fact]
        public void TestSetNullKey()
        {
            IndexedList<string, SiteLink> list = new IndexedList<string, SiteLink>(siteLink => siteLink.SiteId) {
                new SiteLink("enwiki", "Foo")
            };

            Assert.Throws<ArgumentNullException>(() => list.Set(new SiteLink()));
        }

        [Fact]
        public void TestAddConflict()
        {
            IndexedList<string, SiteLink> list = new IndexedList<string, SiteLink> (siteLink => siteLink.SiteId) {
                new SiteLink("enwiki", "Foo")
            };

            Assert.Throws<ArgumentException>(() => list.Add(new SiteLink("enwiki", "Bar")));
        }

        [Fact]
        public void TestContainsKey()
        {
            IndexedList<string, SiteLink> list = new IndexedList<string, SiteLink>(siteLink => siteLink.SiteId) {
                new SiteLink("enwiki", "Foo")
            };

            Assert.True(list.ContainsKey("enwiki"));
            Assert.False(list.ContainsKey("xxwiki"));
        }

        [Fact]
        public void TestRemoveExisting()
        {
            IndexedList<string, SiteLink> list = new IndexedList<string, SiteLink>(siteLink => siteLink.SiteId) {
                new SiteLink("enwiki", "Foo")
            };

            Assert.True(list.Remove("enwiki"));
        }

        [Fact]
        public void TestRemoveNotExisting()
        {
            IndexedList<string, SiteLink> list = new IndexedList<string, SiteLink>(siteLink => siteLink.SiteId) {
                new SiteLink("enwiki", "Foo")
            };

            Assert.False(list.Remove("xxwiki"));
        }

        [Fact]
        public void TestCount()
        {
            IndexedList<string, SiteLink> list = new IndexedList<string, SiteLink> (siteLink => siteLink.SiteId) {
                new SiteLink("enwiki", "Foo"),
                new SiteLink("dewiki", "Bar")
            };
            Assert.Equal(2, list.Count);
        }

        [Fact]
        public void TestCountEmpty()
        {
            IndexedList<string, SiteLink> list = new IndexedList<string, SiteLink>(siteLink => siteLink.SiteId);
            Assert.Equal(0, list.Count);
        }

        [Fact]
        public void TestForeach()
        {
            IndexedList<string, SiteLink> list = new IndexedList<string, SiteLink> (siteLink => siteLink.SiteId) {
                new SiteLink("enwiki", "Foo")
            };

            foreach (SiteLink siteLink in list)
            {
                Assert.Equal("enwiki", siteLink.SiteId);
                Assert.Equal("Foo", siteLink.PageName);
            }
        }
    }
}

