using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Wikibase.DataModel;
using System.Collections.Generic;

namespace Wikibase.Tests.DataModel
{
    [TestClass]
    public class IndexedListTest
    {
        [TestMethod]
        public void TestFromList()
        {
            IndexedList<string, SiteLink> list = new IndexedList<string, SiteLink> (
                siteLink => siteLink.SiteId,
                new List<SiteLink> { new SiteLink("enwiki", "Foo") }
            );

            Assert.AreEqual("Foo", list["enwiki"].PageName);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void TestDuplicateKey()
        {
            new IndexedList<string, SiteLink>(siteLink => siteLink.SiteId) {
                new SiteLink("dewiki", "Foo"),
                new SiteLink("dewiki", "Bar")
            };
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TestNullKey()
        {
            new IndexedList<string, SiteLink>(siteLink => siteLink.SiteId) {
                new SiteLink()
            };
        }

        [TestMethod]
        public void TestAccess()
        {
            IndexedList<string, SiteLink> list = new IndexedList<string, SiteLink> (siteLink => siteLink.SiteId) {
                new SiteLink("enwiki", "Foo"),
                new SiteLink("dewiki", "Bar")
            };

            Assert.AreEqual("Foo", list["enwiki"].PageName);
            Assert.AreEqual("Bar", list["dewiki"].PageName);
        }

        [TestMethod]
        [ExpectedException(typeof(KeyNotFoundException))]
        public void TestAccessNotExisting()
        {
            IndexedList<string, SiteLink> list = new IndexedList<string, SiteLink>(siteLink => siteLink.SiteId) {
                new SiteLink("enwiki", "Foo"),
                new SiteLink("dewiki", "Bar")
            };

            SiteLink link = list["xxwiki"];
        }

        [TestMethod]
        public void TestAdd()
        {
            IndexedList<string, SiteLink> list = new IndexedList<string, SiteLink> (siteLink => siteLink.SiteId) {
                new SiteLink("enwiki", "Foo")
            };
            list.Add(new SiteLink("dewiki", "Bar"));

            Assert.AreEqual("Foo", list["enwiki"].PageName);
            Assert.AreEqual("Bar", list["dewiki"].PageName);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TestAddNullKey()
        {
            IndexedList<string, SiteLink> list = new IndexedList<string, SiteLink>(siteLink => siteLink.SiteId) {
                new SiteLink("enwiki", "Foo")
            };
            list.Add(new SiteLink());
        }

        [TestMethod]
        public void TestSet()
        {
            IndexedList<string, SiteLink> list = new IndexedList<string, SiteLink> (siteLink => siteLink.SiteId) {
                new SiteLink("enwiki", "Foo")
            };
            list.Set(new SiteLink("enwiki", "Bar"));

            Assert.AreEqual("Bar", list["enwiki"].PageName);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TestSetNullKey()
        {
            IndexedList<string, SiteLink> list = new IndexedList<string, SiteLink>(siteLink => siteLink.SiteId) {
                new SiteLink("enwiki", "Foo")
            };
            list.Set(new SiteLink());
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void TestAddConflict()
        {
            IndexedList<string, SiteLink> list = new IndexedList<string, SiteLink> (siteLink => siteLink.SiteId) {
                new SiteLink("enwiki", "Foo")
            };
            list.Add(new SiteLink("enwiki", "Bar"));
        }

        [TestMethod]
        public void TestContainsKey()
        {
            IndexedList<string, SiteLink> list = new IndexedList<string, SiteLink>(siteLink => siteLink.SiteId) {
                new SiteLink("enwiki", "Foo")
            };

            Assert.IsTrue(list.ContainsKey("enwiki"));
            Assert.IsFalse(list.ContainsKey("xxwiki"));
        }

        [TestMethod]
        public void TestRemoveExisting()
        {
            IndexedList<string, SiteLink> list = new IndexedList<string, SiteLink>(siteLink => siteLink.SiteId) {
                new SiteLink("enwiki", "Foo")
            };

            Assert.IsTrue(list.Remove("enwiki"));
        }

        [TestMethod]
        public void TestRemoveNotExisting()
        {
            IndexedList<string, SiteLink> list = new IndexedList<string, SiteLink>(siteLink => siteLink.SiteId) {
                new SiteLink("enwiki", "Foo")
            };

            Assert.IsFalse(list.Remove("xxwiki"));
        }

        [TestMethod]
        public void TestCount()
        {
            IndexedList<string, SiteLink> list = new IndexedList<string, SiteLink> (siteLink => siteLink.SiteId) {
                new SiteLink("enwiki", "Foo"),
                new SiteLink("dewiki", "Bar")
            };
            Assert.AreEqual(2, list.Count);
        }

        [TestMethod]
        public void TestCountEmpty()
        {
            IndexedList<string, SiteLink> list = new IndexedList<string, SiteLink>(siteLink => siteLink.SiteId);
            Assert.AreEqual(0, list.Count);
        }

        [TestMethod]
        public void TestForeach()
        {
            IndexedList<string, SiteLink> list = new IndexedList<string, SiteLink> (siteLink => siteLink.SiteId) {
                new SiteLink("enwiki", "Foo")
            };

            foreach (SiteLink siteLink in list)
            {
                Assert.AreEqual("enwiki", siteLink.SiteId);
                Assert.AreEqual("Foo", siteLink.PageName);
            }
        }
    }
}

