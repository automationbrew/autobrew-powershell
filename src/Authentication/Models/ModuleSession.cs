namespace AutoBrew.PowerShell.Models
{
    using System.Collections.Concurrent;
    using Factories;

    /// <summary>
    /// Represents the current session for the module.
    /// </summary>
    public sealed class ModuleSession
    {
        /// <summary>
        /// Provides a singleton instance of the <see cref="ModuleSession" /> class.
        /// </summary>
        private static readonly Lazy<ModuleSession> session = new();

        /// <summary>
        /// Provides a lock that is used to manage access to a resource, allowing multiple threads for reading or exclusive access for writing.
        /// </summary>
        private static readonly ReaderWriterLockSlim resourceLock = new(LockRecursionPolicy.SupportsRecursion);

        /// <summary>
        /// Provides the components that have been registered.
        /// </summary>
        private readonly IDictionary<ComponentType, object> componentRegistry = new ConcurrentDictionary<ComponentType, object>();

        /// <summary>
        /// Gets or sets the factory used to perform authentication requests.
        /// </summary>
        public IAuthenticationFactory AuthenticationFactory { get; set; }

        /// <summary>
        /// Gets or sets the module context.
        /// </summary>
        public ModuleContext Context { get; set; }

        /// <summary>
        /// Gets the instance of the <see cref="ModuleSession" /> class.
        /// </summary>
        public static ModuleSession Instance
        {
            get
            {
                resourceLock.EnterReadLock();

                try
                {
                    return session.Value;
                }
                finally
                {
                    resourceLock.ExitReadLock();
                }
            }
        }

        /// <summary>
        /// Gets the component associated with the specified key.
        /// </summary>
        /// <typeparam name="T">The type of component in the registry.</typeparam>
        /// <param name="key">The key for the component.</param>
        /// <param name="component">When this method returns, contains the value associated with the specified key, if the key is found; otherwise, the default value for the type of the value parameter.</param>
        /// <returns><c>true</c> if the component registry contains an element with the specified key; otherwise, <c>false</c>.</returns>
        public bool TryGetComponent<T>(ComponentType key, out T component) where T : class
        {
            bool flag = componentRegistry.TryGetValue(key, out object value);

            component = value == null ? null : value as T;

            return flag;
        }

        /// <summary>
        /// Registers a component with the specified key.
        /// </summary>
        /// <typeparam name="T">The type of component in the registry.</typeparam>
        /// <param name="key">The key for the component.</param>
        /// <param name="initializer">The method used to initialize the component.</param>
        /// <param name="overwrite">A flag that indicates if the component should be overwrite an existing registration.</param>
        public void RegisterComponent<T>(ComponentType key, Func<T> initializer, bool overwrite = false) where T : class
        {
            initializer.AssertNotNull(nameof(initializer));

            if (!componentRegistry.ContainsKey(key) || overwrite)
            {
                componentRegistry[key] = initializer();
            }
        }

        /// <summary>
        /// Unregisters the component with the specified key.
        /// </summary>
        /// <param name="key">The key for the component.</param>
        public void UnregisterComponent(ComponentType key)
        {
            if (componentRegistry.ContainsKey(key))
            {
                componentRegistry.Remove(key);
            }
        }
    }
}