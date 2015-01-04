// <copyright file="StyleCopSession.cs" company="Brad Hess">
//   See LICENSE in the repo root for copyright and licensing information.
// </copyright>

using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using StyleCop;

namespace BrowningStyle.Tests
{
    /// <summary>
    /// A virtual session with the StyleCop analyzer.
    /// </summary>
    internal class StyleCopSession : IDisposable
    {
        /// <summary>
        /// The temporary working directory for this session.
        /// </summary>
        private readonly string tempDirectory;

        /// <summary>
        /// Initializes a new instance of the <see cref="StyleCopSession"/> class.
        /// </summary>
        /// <param name="sourceCode">The test code source file as a string.</param>
        public StyleCopSession(string sourceCode)
        {
            this.tempDirectory = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());
            Directory.CreateDirectory(this.tempDirectory);

            string settings = this.PushCollateral(TestCollateral.Settings, "TestSettings.stylecop");
            Console.WriteLine("Extracting settings to: {0}", settings);

            string[] addInPaths = new[] { AppDomain.CurrentDomain.BaseDirectory };
            Console.WriteLine(addInPaths[0]);
            foreach (string file in Directory.GetFiles(addInPaths[0]))
            {
                Console.WriteLine(file);
            }
            
            StyleCopConsole console = new StyleCopConsole(settings, false, null, addInPaths, true);

            foreach (var parser in console.Core.Parsers)
            {
                foreach (var analyzer in parser.Analyzers)
                {
                    Console.WriteLine("Found analyzer: {0}::{1}", parser.Name, analyzer.Name);
                }
            }

            this.Violations = new List<Violation>();
            console.ViolationEncountered += (sender, args) => this.Violations.Add(args.Violation);

            Configuration configuration = new Configuration(new string[0]);
            CodeProject project = new CodeProject(Guid.NewGuid().GetHashCode(), settings, configuration);

            string resourcePath = this.PushCollateral(sourceCode, "TestCase.cs");
            Console.WriteLine("Extracting resource to: {0}", resourcePath);
            console.Core.Environment.AddSourceCode(project, resourcePath, null);

            console.Start(new[] { project }, true);
        }

        /// <summary>
        /// Gets a list of violations from the StyleCop session.
        /// </summary>
        public List<Violation> Violations { get; private set; }

        /// <inheritdoc />
        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Dispose managed resources. Classes derived from this class should
        /// take care to call <code>base.Dispose(disposing)</code> within an overridden
        /// implementation.
        /// </summary>
        /// <param name="disposing">Whether a dispose in progress.</param>
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                Directory.Delete(this.tempDirectory, true);
            }
        }

        /// <summary>
        /// Push a collateral string into a temporary directory
        /// </summary>
        /// <param name="contents">The data to write.</param>
        /// <param name="filename">The name of the file to create.</param>
        /// <returns>The path to the extracted file.</returns>
        private string PushCollateral(string contents, string filename)
        {
            string targetPath = Path.Combine(this.tempDirectory, filename);
            File.WriteAllText(targetPath, contents);
            return targetPath;
        }
    }
}
