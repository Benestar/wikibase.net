using System;
using Wikibase.DataModel;
using Xunit;

namespace Wikibase.Tests.DataModel
{
    public class TermTest
    {
        [Fact]
        public void TestTerm()
        {
            Term term = new Term("en", "Test");

            Assert.Equal("en", term.Language);
            Assert.Equal("Test", term.Text);
        }

        [Fact]
        public void TestNullLanguage()
        {
            Assert.Throws<ArgumentNullException>(() => new Term(null, "Test"));
        }

        [Fact]
        public void TestNullText()
        {
            Assert.Throws<ArgumentNullException>(() => new Term("en", null));
        }
    }
}
