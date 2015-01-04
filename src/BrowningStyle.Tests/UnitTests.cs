// <copyright file="UnitTests.cs" company="Brad Hess">
//   See LICENSE in the repo root for copyright and licensing information.
// </copyright>

using System;
using System.Collections.Generic;
using System.Linq;
using StyleCop;
using Xunit;

namespace BrowningStyle.Tests
{
    /// <summary>
    /// Unit tests for BrowningStyle
    /// </summary>
    public class UnitTests
    {
        /// <summary>
        /// Ensure that the success case passes with no violations.
        /// </summary>
        [Fact]
        public void Success()
        {
            using (StyleCopSession session = new StyleCopSession(TestCollateral.Success))
            {
                this.Dump(session.Violations);
                Assert.Empty(session.Violations);
            }
        }

        /// <summary>
        /// Ensure that a file that doesn't end with a newline is detected.
        /// </summary>
        [Fact]
        public void EndingNewLineFailure()
        {
            using (StyleCopSession session = new StyleCopSession(TestCollateral.FailureEndingNewLine))
            {
                this.Dump(session.Violations);

                Assert.NotEmpty(session.Violations);
                Assert.Equal(1, session.Violations.Count);

                Violation v = session.Violations.Single();
                Assert.Equal(9, v.Line);
                Assert.Equal("The last character in a source file must be a new line.", v.Message);
                Assert.Equal("BS1001", v.Rule.CheckId);
                Assert.Equal("FileMustEndWithNewLine", v.Rule.Name);
            }
        }

        /// <summary>
        /// Ensure that a file with trailing white space is detected.
        /// </summary>
        [Fact]
        public void TrailingWhiteSpaceFailure()
        {
            using (StyleCopSession session = new StyleCopSession(TestCollateral.FailureTrailingWhiteSpace))
            {
                this.Dump(session.Violations);

                Assert.NotEmpty(session.Violations);
                Assert.Equal(3, session.Violations.Count);

                Assert.Equal(6, session.Violations.Skip(0).First().Line);
                Assert.Equal(8, session.Violations.Skip(1).First().Line);
                Assert.Equal(10, session.Violations.Skip(2).First().Line);

                foreach (Violation v in session.Violations)
                {
                    Assert.Equal("The last character of a line must not be white space.", v.Message);
                    Assert.Equal("BS1000", v.Rule.CheckId);
                    Assert.Equal("NoTrailingWhiteSpace", v.Rule.Name);
                }
            }
        }

        /// <summary>
        /// Ensure that a use of the <code>var</code> contextual keyword is detected.
        /// </summary>
        [Fact]
        public void UseOfVarFailure()
        {
            using (StyleCopSession session = new StyleCopSession(TestCollateral.FailureUseOfVar))
            {
                this.Dump(session.Violations);

                Assert.NotEmpty(session.Violations);
                Assert.Equal(1, session.Violations.Count);

                Violation v = session.Violations.Single();
                Assert.Equal(10, v.Line);
                Assert.Equal("Use of the var keyword is disallowed.", v.Message);
                Assert.Equal("BS1101", v.Rule.CheckId);
                Assert.Equal("DoNotUseVarKeyword", v.Rule.Name);
            }
        }

        /// <summary>
        /// Dump a set of violations to the console.
        /// </summary>
        /// <param name="violations">The violations to dump.</param>
        private void Dump(IEnumerable<Violation> violations)
        {
            foreach (Violation violation in violations)
            {
                Console.WriteLine(
                    "Found violation in {0}, line {1}: {2}.",
                    violation.SourceCode.Path,
                    violation.Line,
                    violation.Message);
            }
        }
    }
}
