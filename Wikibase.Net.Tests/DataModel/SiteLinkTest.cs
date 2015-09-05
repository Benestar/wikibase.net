using System;
using Wikibase.DataModel;
using System.Collections.Generic;
using Xunit;

namespace Wikibase.Tests.DataModel
{
    public class SiteLinkTest
    {
        [Fact]
        public void TestSiteLink()
        {
            SiteLink siteLink = new SiteLink("enwiki", "Test");

            Assert.Equal("enwiki", siteLink.SiteId);
            Assert.Equal("Test", siteLink.PageName);
            Assert.Equal(0, siteLink.Badges.Count);
        }

        [Fact]
        public void TestBadges()
        {
            SiteLink siteLink = new SiteLink(
                "enwiki",
                "Test",
                new HashSet<ItemId> { new ItemId("Q12"), new ItemId("Q13") }
            );

            Assert.Equal(2, siteLink.Badges.Count);
            Assert.True(siteLink.Badges.Contains(new ItemId("Q12")));
            Assert.True(siteLink.Badges.Contains(new ItemId("Q13")));
        }

        [Fact]
        public void TestNullSiteId()
        {
            Assert.Throws<ArgumentNullException>(() => new SiteLink(null, "Test"));
        }

        [Fact]
        public void TestNullPageName()
        {
            Assert.Throws<ArgumentNullException>(() => new SiteLink("enwiki", null));
        }

        [Fact]
        public void TestAddBadge()
        {
            SiteLink siteLink = new SiteLink(
                "enwiki",
                "Test",
                new HashSet<ItemId> { new ItemId("Q12"), new ItemId("Q13") }
            );
            siteLink.Badges.Add(new ItemId("Q14"));

            Assert.Equal(3, siteLink.Badges.Count);
            Assert.True(siteLink.Badges.Contains(new ItemId("Q14")));
        }

        [Fact]
        public void TestRemoveBadge()
        {
            SiteLink siteLink = new SiteLink(
                "enwiki",
                "Test",
                new HashSet<ItemId> { new ItemId("Q12"), new ItemId("Q13") }
            );
            siteLink.Badges.Remove(new ItemId("Q13"));

            Assert.Equal(1, siteLink.Badges.Count);
            Assert.False(siteLink.Badges.Contains(new ItemId("Q13")));
        }
    }
}
