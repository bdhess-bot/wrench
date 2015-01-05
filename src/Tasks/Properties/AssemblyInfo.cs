// <copyright file="AssemblyInfo.cs" company="Brad Hess">
//   See LICENSE in the repo root for copyright and licensing information.
// </copyright>

using System;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using System.Resources;
using System.Runtime.InteropServices;

[assembly: AssemblyTitle("Wrench.NuGetTasks")]
[assembly: AssemblyDescription("Wrap NuGet functionality as MSBuild tasks.")]
[assembly: AssemblyProduct("Wrench")]
[assembly: AssemblyCopyright("Copyright ©  2014 Brad Hess")]
[assembly: AssemblyCompany("Brad Hess")]
[assembly: NeutralResourcesLanguage("en-US")]
[assembly: CLSCompliant(true)]
[assembly: ComVisible(false)]
[assembly: Guid("9f571c78-2c9e-4430-99f9-9d1d30b25ea2")]
[assembly: SuppressMessage("Microsoft.Design", "CA2210:AssembliesShouldHaveValidStrongNames",
    Justification = "This is a development-time dependency.")]
