// <copyright file="CommandLineExecutor.cs" company="Brad Hess">
//   See LICENSE in the repo root for copyright and licensing information.
// </copyright>

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;

namespace Wrench.Tasks
{
    /// <summary>
    /// Perform common handling for executing a command given a <see cref="ProcessStartInfo"/>.
    /// </summary>
    internal static class CommandLineExecutor
    {
        /// <summary>
        /// Execute a command.
        /// </summary>
        /// <param name="startInfo">Start information primed with process path and arguments.</param>
        /// <param name="buildEngine">The build engine yielding purposes.</param>
        /// <param name="log">The task's logger.</param>
        /// <returns>The process's exit code.</returns>
        internal static int Execute(ProcessStartInfo startInfo, IBuildEngine3 buildEngine, TaskLoggingHelper log)
        {
            startInfo.RedirectStandardError = true;
            startInfo.RedirectStandardOutput = true;
            startInfo.UseShellExecute = false;

            using (Process process = new Process())
            {
                process.StartInfo = startInfo;
                log.LogMessage(Strings.CallingCommand, process.StartInfo.FileName, process.StartInfo.Arguments);

                process.Start();

                buildEngine.Yield();

                while (!process.HasExited)
                {
                    Thread.Sleep(TimeSpan.FromMilliseconds(100));
                }

                buildEngine.Reacquire();

                string line;
                while ((line = process.StandardOutput.ReadLine()) != null)
                {
                    log.LogMessage(line);
                }

                while ((line = process.StandardError.ReadLine()) != null)
                {
                    log.LogWarning(line);
                }

                return process.ExitCode;
            }
        }
    }
}
