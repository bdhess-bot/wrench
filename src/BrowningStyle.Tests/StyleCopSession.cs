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
        /// <param name="resourceName">The embedded resource to inspect</param>
        public StyleCopSession(string resourceName)
        {
            this.tempDirectory = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());
            Directory.CreateDirectory(this.tempDirectory);

            string settings = this.ExtractResource("TestSettings.stylecop");
            Console.WriteLine("Extracting settings to: {0}", settings);

            string[] addInPaths = new[] { AppDomain.CurrentDomain.BaseDirectory };
            StyleCopConsole console = new StyleCopConsole(settings, false, null, addInPaths, true);

            this.Violations = new List<Violation>();
            console.ViolationEncountered += (sender, args) => this.Violations.Add(args.Violation);

            Configuration configuration = new Configuration(new string[0]);
            CodeProject project = new CodeProject(Guid.NewGuid().GetHashCode(), settings, configuration);

            string resourcePath = this.ExtractResource(resourceName);
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
        /// Extract an embedded resource into the temporary directory
        /// </summary>
        /// <param name="filename">The relative name of the resource to extract.</param>
        /// <returns>The path to the extracted file.</returns>
        private string ExtractResource(string filename)
        {
            string collateralNamespace = typeof(StyleCopSession).Namespace + ".Collateral";
            Assembly currentAssembly = typeof(StyleCopSession).Assembly;

            string sourceResourceName = collateralNamespace + "." + filename;
            string targetPath = Path.Combine(this.tempDirectory, filename);

            using (Stream contents = currentAssembly.GetManifestResourceStream(sourceResourceName))
            {
                using (FileStream file = File.Create(targetPath))
                {
                    contents.CopyTo(file);
                }
            }

            return targetPath;
        }
    }
}
