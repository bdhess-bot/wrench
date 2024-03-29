﻿// <copyright file="UnitTests.cs" company="Brad Hess">
//   See LICENSE in the repo root for copyright and licensing information.
// </copyright>

using System;
using System.Collections.Generic;
using System.Linq;
using StyleCop;
using Xunit;

namespace Wrench.BrowningStyle.Tests
{
    /// <summary>
    /// Unit tests for BrowningStyle
    /// </summary>
    public static class UnitTests
    {
        /// <summary>
        /// Ensure that all success cases pass with no violations.
        /// </summary>
        /// <param name="path">Path to a collateral success case.</param>
        [Theory]
        [InlineData("EndingNewLineSuccessCase.cs")]
        [InlineData("TrailingWhiteSpaceSuccessCase.cs")]
        [InlineData("UseOfVarSuccessCase.cs")]
        public static void Success(string path)
        {
            using (StyleCopSession session = new StyleCopSession(path))
            {
                Assert.Empty(session.Violations);
            }
        }

        /// <summary>
        /// Ensure that a file that doesn't end with a newline is detected.
        /// </summary>
        [Fact]
        public static void EndingNewLineFailure()
        {
            using (StyleCopSession session = new StyleCopSession("EndingNewLineFailureCase.cs"))
            {
                Assert.NotEmpty(session.Violations);
                Assert.Equal(1, session.Violations.Count);

                Violation v = session.Violations.Single();
                Assert.Equal(12, v.Line);
                Assert.Equal("The last character in a source file must be a new line.", v.Message);
                Assert.Equal("BS1001", v.Rule.CheckId);
                Assert.Equal("FileMustEndWithNewLine", v.Rule.Name);
            }
        }

        /// <summary>
        /// Ensure that a file with trailing white space is detected.
        /// </summary>
        [Fact]
        public static void TrailingWhiteSpaceFailure()
        {
            using (StyleCopSession session = new StyleCopSession("TrailingWhiteSpaceFailureCase.cs"))
            {
                Assert.NotEmpty(session.Violations);
                Assert.Equal(3, session.Violations.Count);

                Assert.Equal(9, session.Violations.Skip(0).First().Line);
                Assert.Equal(11, session.Violations.Skip(1).First().Line);
                Assert.Equal(13, session.Violations.Skip(2).First().Line);

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
        public static void UseOfVarFailure()
        {
            using (StyleCopSession session = new StyleCopSession("UseOfVarFailureCase.cs"))
            {
                Assert.NotEmpty(session.Violations);
                Assert.Equal(2, session.Violations.Count);

                Assert.Equal(13, session.Violations.Skip(0).First().Line);
                Assert.Equal(20, session.Violations.Skip(1).First().Line);

                foreach (Violation v in session.Violations)
                {
                    Assert.Equal("Use of the var keyword is disallowed.", v.Message);
                    Assert.Equal("BS1101", v.Rule.CheckId);
                    Assert.Equal("DoNotUseVarKeyword", v.Rule.Name);
                }
            }
        }
    }
}
