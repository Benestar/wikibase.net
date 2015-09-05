using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Wikibase.DataModel;
using System.Collections.Generic;

namespace Wikibase.Tests.DataModel
{
    [TestClass]
    public class SiteLinkTest
    {
        [TestMethod]
        public void TestSiteLink()
        {
            SiteLink siteLink = new SiteLink("enwiki", "Test");

            Assert.AreEqual("enwiki", siteLink.SiteId);
            Assert.AreEqual("Test", siteLink.PageName);
            Assert.AreEqual(0, siteLink.Badges.Count);
        }

        [TestMethod]
        public void TestBadges()
        {
            SiteLink siteLink = new SiteLink(
                "enwiki",
                "Test",
                new HashSet<ItemId> { new ItemId("Q12"), new ItemId("Q13") }
            );

            Assert.AreEqual(2, siteLink.Badges.Count);
            Assert.IsTrue(siteLink.Badges.Contains(new ItemId("Q12")));
            Assert.IsTrue(siteLink.Badges.Contains(new ItemId("Q13")));
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TestNullSiteId()
        {
            new SiteLink(null, "Test");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TestNullPageName()
        {
            new SiteLink("enwiki", null);
        }

        [TestMethod]
        public void TestAddBadge()
        {
            SiteLink siteLink = new SiteLink(
                "enwiki",
                "Test",
                new HashSet<ItemId> { new ItemId("Q12"), new ItemId("Q13") }
            );
            siteLink.Badges.Add(new ItemId("Q14"));

            Assert.AreEqual(3, siteLink.Badges.Count);
            Assert.IsTrue(siteLink.Badges.Contains(new ItemId("Q14")));
        }

        [TestMethod]
        public void TestRemoveBadge()
        {
            SiteLink siteLink = new SiteLink(
                "enwiki",
                "Test",
                new HashSet<ItemId> { new ItemId("Q12"), new ItemId("Q13") }
            );
            siteLink.Badges.Remove(new ItemId("Q13"));

            Assert.AreEqual(1, siteLink.Badges.Count);
            Assert.IsFalse(siteLink.Badges.Contains(new ItemId("Q13")));
        }
    }
}
