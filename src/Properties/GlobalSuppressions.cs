using System.Diagnostics.CodeAnalysis;

[assembly: SuppressMessage("Microsoft.Design", "CA1016:MarkAssembliesWithAssemblyVersion",
    Justification = "Versioning is provided by Appveyor build.")]
[assembly: SuppressMessage("Microsoft.Design", "CA2210:AssembliesShouldHaveValidStrongNames",
    Justification = "Strong name signing is no longer suggested for new projects.")]
