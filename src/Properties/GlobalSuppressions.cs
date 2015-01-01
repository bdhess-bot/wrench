// <copyright file="GlobalSuppressions.cs" company="Brad Hess">
//   Copyright (c) 2014 Brad Hess, and licensed under the MIT License.
//   See 'LICENSE' in the repo root for more information.
// </copyright>

using System.Diagnostics.CodeAnalysis;

[assembly: SuppressMessage("Microsoft.Design", "CA1016:MarkAssembliesWithAssemblyVersion",
    Justification = "Versioning is provided by Appveyor build.")]
[assembly: SuppressMessage("Microsoft.Design", "CA2210:AssembliesShouldHaveValidStrongNames",
    Justification = "Strong name signing is no longer suggested for new projects.")]
