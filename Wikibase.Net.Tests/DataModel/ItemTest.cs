using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using Wikibase.DataModel;

namespace Wikibase.Tests.DataModel
{
    [TestClass]
    public class ItemTest
    {
        [TestMethod]
        public void TestId()
        {
            Item item = new Item(new ItemId("Q42"));
            Assert.AreEqual("Q42", item.Id.Serialization);
        }

        [TestMethod]
        public void TestItemId()
        {
            Item item = new Item(new ItemId("Q42"));
            Assert.AreEqual("Q42", item.ItemId.Serialization);
        }

        [TestMethod]
        public void TestFingerprint()
        {
            Item item = new Item(
                new ItemId("Q42"),
                new Fingerprint( new List<Term> { new Term("en", "foo") }, null, null ),
                null,
                null
            );

            Assert.IsFalse(item.Empty);
            Assert.AreEqual("foo", item.Fingerprint.Labels["en"].Text);
        }

        [TestMethod]
        public void TestStatements()
        {
            Item item = new Item(
                new ItemId("Q42"),
                null,
                new List<Statement> { new Statement(new PropertyNoValueSnak("P42")) },
                null
            );

            Assert.IsFalse(item.Empty);
            Assert.AreEqual(1, item.Statements.Count);
            Assert.AreEqual("P42", item.Statements[0].PropertyId.Serialization);
        }

        [TestMethod]
        public void TestSitelinks()
        {
            Item item = new Item(
                new ItemId("Q42"),
                null,
                null,
                new List<SiteLink> { new SiteLink("enwiki", "Foo"), new SiteLink("dewiki", "Bar") }
            );

            Assert.IsFalse(item.Empty);
            Assert.AreEqual(2, item.Sitelinks.Count);
            Assert.AreEqual(item.Sitelinks["enwiki"].PageName, "Foo");
            Assert.AreEqual(item.Sitelinks["dewiki"].PageName, "Bar");
        }

        [TestMethod]
        public void TestEmpty()
        {
            Item item = new Item();
            Assert.IsTrue(item.Empty);
        }

        [TestMethod]
        public void TestEmptyWithId()
        {
            Item item = new Item(new ItemId("Q42"));
            Assert.IsTrue(item.Empty);
        }
    }
}
