// <copyright file="FindNuGet.cs" company="Brad Hess">
//   See LICENSE in the repo root for copyright and licensing information.
// </copyright>

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;

namespace Wrench.Tasks
{
    /// <summary>
    /// Find the NuGet executable in this solution.
    /// </summary>
    [SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly",
        MessageId = "Nu", Justification = "NuGet is the correctly cased name of the product.")]
    public class FindNuGet : Task
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FindNuGet"/> class.
        /// </summary>
        public FindNuGet()
        {
            this.ExecutableName = "NuGet.exe";
        }

        /// <summary>
        /// Gets or sets the NuGet packages directory for the solution.
        /// </summary>
        [Required]
        public string PackagesDirectory { get; set; }

        /// <summary>
        /// Gets or sets the directory where the project exists.
        /// </summary>
        [Required]
        public string ProjectDirectory { get; set; }

        /// <summary>
        /// Gets or sets the name of the target executable (by
        /// default, "NuGet.exe").
        /// </summary>
        public string ExecutableName { get; set; }

        /// <summary>
        /// Gets the best match found by this task.
        /// </summary>
        [Output]
        public string Result { get; private set; }

        /// <inheritdoc />
        public override bool Execute()
        {
            this.Result = this.FindExecutable(this.PackagesDirectory, true);

            if (this.Result == null)
            {
                this.Result = this.FindExecutable(this.ProjectDirectory, true);
            }

            if (this.Result == null)
            {
                this.Result = this.FindExecutable(
                    Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), true);
            }

            if (this.Result == null)
            {
                this.Log.LogError(Strings.CouldNotFindNuGet);
                return false;
            }

            return true;
        }

        /// <summary>
        /// Find an executable with the name specified in <see cref="ExecutableName"/>
        /// by checking some well-known directory patterns.
        /// </summary>
        /// <param name="startDirectory">The starting point of the search.</param>
        /// <param name="recurse">Whether or not to traverse parent directories.</param>
        /// <returns>A path to a found executable, or null if none was found.</returns>
        private string FindExecutable(string startDirectory, bool recurse)
        {
            string localPath = Path.Combine(startDirectory, this.ExecutableName);
            if (File.Exists(localPath))
            {
                return localPath;
            }

            foreach (string directory in Directory.EnumerateDirectories(startDirectory, "NuGet.CommandLine.*")
                .OrderBy(x => Version.Parse(x.Substring("NuGet.CommandLine.".Length))))
            {
                string searchResult = this.FindExecutable(directory, false);
                if (searchResult != null)
                {
                    return searchResult;
                }
            }

            foreach (string directory in Directory.EnumerateDirectories(startDirectory, "*nuget*"))
            {
                string searchResult = this.FindExecutable(directory, false);
                if (searchResult != null)
                {
                    return searchResult;
                }
            }

            if (!recurse || Directory.GetParent(startDirectory) == null)
            {
                return null;
            }

            return this.FindExecutable(Directory.GetParent(startDirectory).FullName, true);
        }
    }
}
