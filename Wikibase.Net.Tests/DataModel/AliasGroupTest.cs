using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Wikibase.DataModel;
using System.Collections.Generic;

namespace Wikibase.Tests.DataModel
{
    [TestClass]
    public class AliasGroupTest
    {
        [TestMethod]
        public void TestAliasGroup()
        {
            AliasGroup aliasGroup = new AliasGroup("en", new HashSet<string> { "foo", "bar" });

            Assert.AreEqual("en", aliasGroup.Language);
            Assert.AreEqual(2, aliasGroup.Aliases.Count);
            Assert.IsTrue(aliasGroup.Aliases.Contains("foo"));
            Assert.IsTrue(aliasGroup.Aliases.Contains("bar"));
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TestNullLanguage()
        {
            new AliasGroup(null, new HashSet<string>());
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TestNullAliases()
        {
            new AliasGroup("en", null);
        }

        [TestMethod]
        public void TestAddAlias()
        {
            AliasGroup aliasGroup = new AliasGroup("en", new HashSet<string> { "foo", "bar" });
            aliasGroup.Aliases.Add("baz");

            Assert.AreEqual(3, aliasGroup.Aliases.Count);
            Assert.IsTrue(aliasGroup.Aliases.Contains("baz"));
        }

        [TestMethod]
        public void TestRemoveAlias()
        {
            AliasGroup aliasGroup = new AliasGroup("en", new HashSet<string> { "foo", "bar" });
            aliasGroup.Aliases.Remove("bar");

            Assert.AreEqual(1, aliasGroup.Aliases.Count);
            Assert.IsFalse(aliasGroup.Aliases.Contains("bar"));
        }
    }
}
