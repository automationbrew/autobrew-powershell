﻿namespace AutoBrew.PowerShell.Utilities
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Reflection;

    /// <summary>
    /// Provides the ability to resolve assemblies before the PowerShell module is loaded.
    /// </summary>
    public static class AssemblyResolver
    {
        /// <summary>
        /// Name for the directory that contains the assemblies.
        /// </summary>
        private const string AssemblyDirectoryName = "NetFxPreloadAssemblies";

        /// <summary>
        /// Provides a list of assemblies, including the version, that should be redirected when loading.
        /// </summary>
        private static readonly Dictionary<string, Version> NetFxPreloadAssemblies = new(StringComparer.InvariantCultureIgnoreCase)
        {
            { "Microsoft.ApplicationInsights", new Version("2.21.0.429") },
            { "Microsoft.Bcl.AsyncInterfaces", new Version("7.0.0.0") },
            { "Microsoft.Extensions.Primitives", new Version("7.0.0.0") },
            { "Microsoft.Graph.Beta", new Version("4.68.0.0") },
            { "Microsoft.Graph.Core", new Version("2.0.14.0") },
            { "Microsoft.Identity.Client", new Version("4.48.0.0") },
            { "Microsoft.Identity.Client.Extensions.Msal", new Version("2.25.0.0") },
            { "Microsoft.IdentityModel.Abstractions", new Version("6.22.0.0") },
            { "System.Buffers", new Version("4.0.3.0") },
            { "System.Diagnostics.DiagnosticSource", new Version("5.0.0.0") },
            { "System.Memory", new Version("4.6.31308.1") },
            { "System.Numerics.Vectors", new Version("4.6.26515.06") },
            { "System.Runtime.CompilerServices.Unsafe", new Version("6.0.0.0") },
            { "System.Text.Encodings.Web", new Version("7.0.0.0") },
            { "System.Text.Json", new Version("7.0.0.0") },
            { "System.Threading.Tasks.Extensions", new Version("4.6.28619.01") }
        };

        /// <summary>
        /// The path for the preload assembly directory.
        /// </summary>
        private static string PreloadAssemblyDirectory;

        /// <summary>
        /// Initializes the assembly resolver to ensure redirects are applied when loading assemblies.
        /// </summary>
        public static void Initialize()
        {
            PreloadAssemblyDirectory = Path.Combine(
                Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location),
                AssemblyDirectoryName);

            AppDomain.CurrentDomain.AssemblyResolve += CurrentDomain_AssemblyResolve;
        }

        /// <summary>
        /// Handles the <see cref="AppDomain.AssemblyResolve" />, <see cref="AppDomain.ResourceResolve" />, or <see cref="AppDomain.AssemblyResolve" /> events for app domain.
        /// </summary>
        /// <param name="sender">The source for the event.</param>
        /// <param name="args">The arguments for the event.</param>
        /// <returns>The assembly that resolves the type, assembly, or resource; or null if the assembly cannot be resolved.</returns>
        private static Assembly CurrentDomain_AssemblyResolve(object sender, ResolveEventArgs args)
        {
            AssemblyName assemblyName;

            try
            {
                assemblyName = new AssemblyName(args.Name);

                if (NetFxPreloadAssemblies.TryGetValue(assemblyName.Name, out Version version))
                {
                    if (IsVersionMatching(assemblyName, version))
                    {
                        return Assembly.LoadFrom(Path.Combine(PreloadAssemblyDirectory, $"{assemblyName.Name}.dll"));
                    }
                }
            }
            catch
            {
                // Exceptions can be safely ignored for this operation
            }

            return null;
        }

        /// <summary>
        /// Determines if the major version for the specified assembly matches the specified version.
        /// </summary>
        /// <param name="assemblyName">An instance of the <see cref="AssemblyName" /> class that represents the assembly to be loaded.</param>
        /// <param name="version">An instance of the <see cref="Version" /> class that represents the version of the assembly in the NetFxPreloadAssemblies directory.</param>
        /// <returns><c>true</c> if the major version matches or the assembly is part of the allowed list; otherwise, <c>false</c>.</returns>
        private static bool IsVersionMatching(AssemblyName assemblyName, Version version)
        {
            string[] versionMismatchAllowed = { "System.Diagnostics.DiagnosticSource", "System.Runtime.CompilerServices.Unsafe", "System.Text.Json" };

            assemblyName.AssertNotNull(nameof(assemblyName));
            version.AssertNotNull(nameof(version));

            if (versionMismatchAllowed.SingleOrDefault(a => a.Equals(assemblyName.Name, StringComparison.OrdinalIgnoreCase)) != null)
            {
                return true;
            }
            else if (version >= assemblyName.Version && version.Major == assemblyName.Version.Major)
            {
                return true;
            }

            return false;
        }
    }
}