namespace AutoBrew.PowerShell.AssemblyLoadContext
{
    using System;
    using System.Collections.Concurrent;
    using System.IO;
    using System.Reflection;
    using System.Runtime.Loader;

    /// <summary>
    /// Provides a scope for binding assemblies.
    /// </summary>
    internal class SharedAssemblyLoadContext : AssemblyLoadContext
    {
        /// <summary>
        /// Name for the directory that contains the assemblies.
        /// </summary>
        private const string assemblyDirectoryName = "NetCorePreloadAssemblies";

        /// <summary>
        /// The cache for any resolved assemblies used to simplify resolution.
        /// </summary>
        private static readonly ConcurrentDictionary<string, Assembly> assemblyCache = new(StringComparer.OrdinalIgnoreCase);

        /// <summary>
        /// The singleton instance of the <see cref="SharedAssemblyLoadContext" /> class.
        /// </summary>
        private static readonly Lazy<SharedAssemblyLoadContext> instance = new();

        /// <summary>
        /// Gets the instance of the <see cref="SharedAssemblyLoadContext" /> class.
        /// </summary>
        public static SharedAssemblyLoadContext Instance => instance.Value;

        /// <summary>
        /// Intiailizes a new instance of the <see cref="SharedAssemblyLoadContext" /> class.
        /// </summary>
        public SharedAssemblyLoadContext()
        { }

        /// <summary>
        /// Provides the ability to resolve and load assemblies based on an instance of <see cref="AssemblyName" /> class.
        /// </summary>
        /// <param name="assemblyName">An instance of the <see cref="AssemblyName" /> class that describes the assembly to be loaded.</param>
        /// <returns>An instance of the <see cref="Assembly" /> class that represents the assembly or null.</returns>
        protected override Assembly Load(AssemblyName assemblyName)
        {
            if (assemblyCache.TryGetValue(assemblyName.Name, out Assembly assembly))
            {
                if (IsAssemblyMatching(assemblyName, assembly.GetName()))
                {
                    return assembly;
                }
            }

            string dependencyAsmPath = Path.Join(
                Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location),
                assemblyDirectoryName,
                $"{assemblyName.Name}.dll");

            if (File.Exists(dependencyAsmPath))
            {
                Assembly loadedAssembly = LoadFromAssemblyPath(dependencyAsmPath);
                AssemblyName loadedAssemblyName = loadedAssembly.GetName();

                if (IsAssemblyMatching(assemblyName, loadedAssemblyName))
                {
                    assemblyCache.TryAdd(loadedAssemblyName.Name, loadedAssembly);
                }

                return loadedAssembly;
            }

            return null;
        }

        private static bool IsAssemblyMatching(AssemblyName requestedAssembly, AssemblyName loadedAssembly)
        {
            // We use the same rules as CoreCLR loader to compare the requested assembly and loaded assembly:
            //  1. If 'Version' of the requested assembly is specified, then the requested version should be less or equal to the loaded version;
            //  2. If 'CultureName' of the requested assembly is specified (not NullOrEmpty), then the CultureName of the loaded assembly should be the same;
            //  3. If 'PublicKeyToken' of the requested assembly is specified (not Null or EmptyArray), then the PublicKenToken of the loaded assembly should be the same.

            // Version of the requested assembly should be the same or before the version of loaded assembly
            if (requestedAssembly.Version != null && requestedAssembly.Version.CompareTo(loadedAssembly.Version) > 0)
            {
                return false;
            }

            // CultureName of requested assembly and loaded assembly should be the same
            string requestedCultureName = requestedAssembly.CultureName;
            if (!string.IsNullOrEmpty(requestedCultureName) && !requestedCultureName.Equals(loadedAssembly.CultureName, StringComparison.OrdinalIgnoreCase))
            {
                return false;
            }

            // PublicKeyToken should be the same, unless it's not specified in the requested assembly
            byte[] requestedPublicKeyToken = requestedAssembly.GetPublicKeyToken();
            byte[] loadedPublicKeyToken = loadedAssembly.GetPublicKeyToken();

            if (requestedPublicKeyToken != null && requestedPublicKeyToken.Length > 0)
            {
                if (loadedPublicKeyToken == null || requestedPublicKeyToken.Length != loadedPublicKeyToken.Length)
                {
                    return false;
                }

                for (int i = 0; i < requestedPublicKeyToken.Length; i++)
                {
                    if (requestedPublicKeyToken[i] != loadedPublicKeyToken[i])
                    {
                        return false;
                    }
                }
            }

            return true;
        }
    }
}