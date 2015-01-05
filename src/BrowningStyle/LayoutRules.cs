// <copyright file="LayoutRules.cs" company="Brad Hess">
//   See LICENSE in the repo root for copyright and licensing information.
// </copyright>

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using StyleCop;
using StyleCop.CSharp;

namespace Wrench.BrowningStyle
{
    /// <summary>
    /// A <see cref="SourceAnalyzer" /> that applies our custom rules.
    /// </summary>
    [SourceAnalyzer(typeof(CsParser))]
    public class LayoutRules : SourceAnalyzer
    {
        /// <inheritdoc/>
        public override void AnalyzeDocument(CodeDocument document)
        {
            Param.RequireNotNull(document, "document");
            CsDocument csharpDocument = (CsDocument)document;

            if (csharpDocument.RootElement != null)
            {
                this.CheckLayout(csharpDocument);
                csharpDocument.WalkDocument(this.ProcessElement, this.ProcessStatement);
            }
        }

        /// <summary>
        /// Checks the correctness of text layout within a given document.
        /// </summary>
        /// <param name="document">The document to process.</param>
        private void CheckLayout(CsDocument document)
        {
            foreach (Node<CsToken> tokenNode in document.Tokens.ForwardNodeIterator())
            {
                CsToken token = tokenNode.Value;

                if (token.CsTokenType == CsTokenType.EndOfLine && 
                    tokenNode.Previous.Value.CsTokenType == CsTokenType.WhiteSpace)
                {
                    this.AddViolation(document.RootElement, tokenNode.Previous.Value.Location, "NoTrailingWhiteSpace");
                }

                if (tokenNode == document.Tokens.Last && token.CsTokenType != CsTokenType.EndOfLine)
                {
                    this.AddViolation(document.RootElement, token.Location, "FileMustEndWithNewLine");
                }
            }
        }

        /// <summary>
        /// Processes one element. This is a no-op implementation.
        /// </summary>
        /// <param name="element">The element to process.</param>
        /// <param name="parentElement">The parent element.</param>
        /// <param name="context">The processing context.</param>
        /// <returns>Returns false if the analyzer should quit.</returns>
        private bool ProcessElement(CsElement element, CsElement parentElement, object context)
        {
            Param.Ignore(element, parentElement, context);
            return true;
        }

        /// <summary>
        /// Processes one statement.
        /// </summary>
        /// <param name="statement">The statement to process.</param>
        /// <param name="parentExpression">The parent expression.</param>
        /// <param name="parentStatement">The parent statement.</param>
        /// <param name="parentElement">The parent element.</param>
        /// <param name="context">The processing context.</param>
        /// <returns>Returns false if the analyzer should quit.</returns>
        private bool ProcessStatement(
            Statement statement,
            Expression parentExpression,
            Statement parentStatement,
            CsElement parentElement,
            object context)
        {
            Param.AssertNotNull(statement, "statement");
            Param.AssertNotNull(parentElement, "parentElement");
            Param.Ignore(parentExpression, parentStatement, context);

            if (statement.StatementType == StatementType.VariableDeclaration)
            {
                foreach (CsToken token in statement.Tokens.Where(x =>
                    x.CsTokenClass == CsTokenClass.Type &&
                    x.CsTokenType == CsTokenType.Other && 
                    x.Text == "var"))
                {
                    this.AddViolation(parentElement, token.Location, "DoNotUseVarKeyword");
                }
            }

            return true;
        }
    }
}
