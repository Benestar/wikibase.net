using System;
using System.Collections.Generic;
using Wikibase.DataModel;
using Xunit;

namespace Wikibase.Tests.DataModel
{
    public class AliasGroupTest
    {
        [Fact]
        public void TestAliasGroup()
        {
            AliasGroup aliasGroup = new AliasGroup("en", new HashSet<string> { "foo", "bar" });

            Assert.Equal("en", aliasGroup.Language);
            Assert.Equal(2, aliasGroup.Aliases.Count);
            Assert.True(aliasGroup.Aliases.Contains("foo"));
            Assert.True(aliasGroup.Aliases.Contains("bar"));
        }

        [Fact]
        public void TestNullLanguage()
        {
            Assert.Throws<ArgumentNullException>(() => new AliasGroup(null, new HashSet<string>()));
        }

        [Fact]
        public void TestNullAliases()
        {
            Assert.Throws<ArgumentNullException>(() => new AliasGroup("en", null));
        }

        [Fact]
        public void TestAddAlias()
        {
            AliasGroup aliasGroup = new AliasGroup("en", new HashSet<string> { "foo", "bar" });
            aliasGroup.Aliases.Add("baz");

            Assert.Equal(3, aliasGroup.Aliases.Count);
            Assert.True(aliasGroup.Aliases.Contains("baz"));
        }

        [Fact]
        public void TestRemoveAlias()
        {
            AliasGroup aliasGroup = new AliasGroup("en", new HashSet<string> { "foo", "bar" });
            aliasGroup.Aliases.Remove("bar");

            Assert.Equal(1, aliasGroup.Aliases.Count);
            Assert.False(aliasGroup.Aliases.Contains("bar"));
        }
    }
}
