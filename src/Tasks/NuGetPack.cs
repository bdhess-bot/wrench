// <copyright file="NuGetPack.cs" company="Brad Hess">
//   See LICENSE in the repo root for copyright and licensing information.
// </copyright>

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;

namespace Wrench.Tasks
{
    /// <summary>
    /// A task for packing a NuGet project.
    /// </summary>
    [SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly",
        MessageId = "Nu", Justification = "NuGet is the correctly cased name of the product.")]
    public class NuGetPack : Task
    {
        /// <summary>
        /// Gets or sets the path to NuGet.exe
        /// </summary>
        [Required]
        public ITaskItem Executable { get; set; }

        /// <summary>
        /// Gets or sets a list of appropriate target files
        /// (project or NuSpec) to be built.
        /// </summary>
        [Required]
        public ITaskItem[] TargetFiles { get; set; }

        /// <summary>
        /// Gets or sets the directory from which relative references
        /// should be resolved.
        /// </summary>
        [Required]
        public ITaskItem BaseDirectory { get; set; }

        /// <summary>
        /// Gets or sets the directory into which built NuGet packages
        /// should be dropped.
        /// </summary>
        public ITaskItem OutputDirectory { get; set; }

        /// <summary>
        /// Gets or sets properties to supply to the NuGet process.
        /// </summary>
        public ITaskItem[] Properties { get; set; }

        /// <inheritdoc />
        public override bool Execute()
        {
            foreach (ITaskItem target in this.TargetFiles)
            {
                CommandLineBuilder builder = new CommandLineBuilder();
                builder.AppendSwitch("pack");
                builder.AppendFileNameIfNotNull(Path.Combine(this.BaseDirectory.ItemSpec, target.ItemSpec));
                builder.AppendSwitchIfNotNull("-BasePath ", this.BaseDirectory);
                builder.AppendSwitchIfNotNull("-OutputDirectory ", this.OutputDirectory);
                builder.AppendSwitchUnquotedIfNotNull("-Properties ", this.Properties, ";");
                string arguments = builder.ToString();

                ProcessStartInfo startInfo = new ProcessStartInfo(this.Executable.ItemSpec, arguments);

                int result = CommandLineExecutor.Execute(startInfo, this.BuildEngine3, this.Log);

                if (result != 0)
                {
                    this.Log.LogError(Strings.FailureInNuGetPack);
                    return false;
                }
            }

            return true;
        }
    }
}
