using System;
using System.Collections.Generic;
using Wikibase.DataModel;
using Xunit;

namespace Wikibase.Tests.DataModel
{
    public class FingerprintTest
    {
        [Fact]
        public void TestLabels()
        {
            Fingerprint fingerprint = new Fingerprint(
                new List<Term> { new Term("en", "foo") },
                null,
                null
            );

            Assert.False(fingerprint.Empty);
            Assert.Equal("en", fingerprint.Labels["en"].Language);
            Assert.Equal("foo", fingerprint.Labels["en"].Text);
        }

        [Fact]
        public void TestDescriptions()
        {
            Fingerprint fingerprint = new Fingerprint(
                null,
                new List<Term> { new Term("en", "this is a foo") },
                null
            );

            Assert.False(fingerprint.Empty);
            Assert.Equal("en", fingerprint.Descriptions["en"].Language);
            Assert.Equal("this is a foo", fingerprint.Descriptions["en"].Text);
        }

        [Fact]
        public void TestAliases()
        {
            Fingerprint fingerprint = new Fingerprint(
                null,
                null,
                new List<AliasGroup> { new AliasGroup("en", new HashSet<string> { "bar", "baz" }) }
            );

            Assert.False(fingerprint.Empty);
            Assert.Equal("en", fingerprint.Aliases["en"].Language);
            Assert.Equal(2, fingerprint.Aliases["en"].Aliases.Count);
            Assert.True(fingerprint.Aliases["en"].Aliases.Contains("bar"));
            Assert.True(fingerprint.Aliases["en"].Aliases.Contains("baz"));
        }

        [Fact]
        public void TestFingerprintEmpty()
        {
            Fingerprint fingerprint = new Fingerprint();

            Assert.True(fingerprint.Empty);
        }

        [Fact]
        public void TestNullLanguage()
        {
            Assert.Throws<ArgumentNullException>(
                () => new Fingerprint(
                    new List<Term> { new Term() },
                    null,
                    null
                )
            );
        }
    }
}
