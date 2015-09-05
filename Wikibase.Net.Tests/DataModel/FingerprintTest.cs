using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using Wikibase.DataModel;

namespace Wikibase.Tests.DataModel
{
    [TestClass]
    public class FingerprintTest
    {
        [TestMethod]
        public void TestLabels()
        {
            Fingerprint fingerprint = new Fingerprint(
                new List<Term> { new Term("en", "foo") },
                null,
                null
            );

            Assert.IsFalse(fingerprint.Empty);
            Assert.AreEqual("en", fingerprint.Labels["en"].Language);
            Assert.AreEqual("foo", fingerprint.Labels["en"].Text);
        }

        [TestMethod]
        public void TestDescriptions()
        {
            Fingerprint fingerprint = new Fingerprint(
                null,
                new List<Term> { new Term("en", "this is a foo") },
                null
            );

            Assert.IsFalse(fingerprint.Empty);
            Assert.AreEqual("en", fingerprint.Descriptions["en"].Language);
            Assert.AreEqual("this is a foo", fingerprint.Descriptions["en"].Text);
        }

        [TestMethod]
        public void TestAliases()
        {
            Fingerprint fingerprint = new Fingerprint(
                null,
                null,
                new List<AliasGroup> { new AliasGroup("en", new HashSet<string> { "bar", "baz" }) }
            );

            Assert.IsFalse(fingerprint.Empty);
            Assert.AreEqual("en", fingerprint.Aliases["en"].Language);
            Assert.AreEqual(2, fingerprint.Aliases["en"].Aliases.Count);
            Assert.IsTrue(fingerprint.Aliases["en"].Aliases.Contains("bar"));
            Assert.IsTrue(fingerprint.Aliases["en"].Aliases.Contains("baz"));
        }

        [TestMethod]
        public void TestFingerprintEmpty()
        {
            Fingerprint fingerprint = new Fingerprint();

            Assert.IsTrue(fingerprint.Empty);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TestNullLanguage()
        {
            Fingerprint fingerprint = new Fingerprint(
                new List<Term> { new Term() },
                null,
                null
            );
        }
    }
}
