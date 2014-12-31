using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using StyleCop;
using StyleCop.CSharp;

namespace BrowningStyle
{
    [SourceAnalyzer(typeof(CsParser))]
    public class BrowningRules : SourceAnalyzer
    {
        public const string WhitespacePattern = "\\s+$";
        private static readonly Regex WhitespaceRegex = new Regex(WhitespacePattern);

        public override void AnalyzeDocument(CodeDocument document)
        {
            Param.RequireNotNull(document, "document");
            CsDocument csDocument = (CsDocument)document;

            if (csDocument.RootElement != null && !csDocument.RootElement.Generated)
            {
                this.CheckLayout(csDocument);

                csDocument.WalkDocument(this.ProcessElement, this.ProcessStatement);
            }
        }

        private void CheckLayout(CsDocument document)
        {
            int currentLineNumber = 0;

            using (TextReader reader = document.SourceCode.Read())
            {
                string lineContents = reader.ReadLine();
                while (lineContents != null)
                {
                    currentLineNumber++;

                    if (WhitespaceRegex.IsMatch(lineContents))
                    {
                        this.AddViolation(document.RootElement, currentLineNumber, Rules.NoTrailingWhitespace);
                    }

                    if (lineContents.Length > 120)
                    {
                        this.AddViolation(document.RootElement, currentLineNumber, Rules.MaximumLineLengthExceeded);
                    }

                    lineContents = reader.ReadLine();
                }
            }

            using (TextReader reader = document.SourceCode.Read())
            {
                if (!reader.ReadToEnd().EndsWith("\r\n", StringComparison.Ordinal))
                {
                    this.AddViolation(document.RootElement, currentLineNumber, Rules.FileMustEndWithNewline);
                }
            }
        }

        private bool ProcessElement(CsElement element, CsElement parentElement, object context)
        {
            return true;
        }

        private bool ProcessStatement(Statement statement, Expression parentExpression,
            Statement parentStatement, CsElement parentElement, object context)
        {
            Param.AssertNotNull(statement, "statement");
            Param.AssertNotNull(parentElement, "parentElement");

            if (!parentElement.Generated && statement.StatementType == StatementType.VariableDeclaration)
            {
                foreach (CsToken token in statement.Tokens.Where(
                    x => x.CsTokenType == CsTokenType.Other && x.Text == "var"))
                {
                    this.AddViolation(parentElement, token.Location, Rules.DoNotUseVarKeyword);
                }
            }

            return true;
        }
    }
}
