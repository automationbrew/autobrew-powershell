namespace AutoBrew.PowerShell.Cache
{
    using System;
    using System.Threading.Tasks;
    using Azure.Identity;

    /// <summary>
    /// Options that are used to control the in-memory token cache.
    /// </summary>
    public class InMemoryTokenCacheOptions : UnsafeTokenCacheOptions
    {
        /// <summary>
        /// Provides a lock that is used to manage access to a resource, allowing multiple threads for reading or exclusive access for writing.
        /// </summary>
        private static readonly ReaderWriterLockSlim resourceLock = new();

        /// <summary>
        /// Gets the bytes that represent the token cache.
        /// </summary>
        internal ReadOnlyMemory<byte> TokenCache { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="InMemoryTokenCacheOptions" /> class.
        /// </summary>
        public InMemoryTokenCacheOptions() : this(new ReadOnlyMemory<byte>())
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="InMemoryTokenCacheOptions" /> class.
        /// </summary>
        /// <param name="tokenCache">The bytes used to initialize the token cache.</param>
        /// <exception cref="ArgumentNullException">
        /// The tokenCache parameter is null.
        /// </exception>
        public InMemoryTokenCacheOptions(ReadOnlyMemory<byte> tokenCache)
        {
            tokenCache.AssertNotNull(nameof(tokenCache));

            TokenCache = tokenCache;
        }

        /// <summary>
        /// Gets the current state of the token cache.
        /// </summary>
        /// <returns>The bytes that represent the token cache.</returns>
        protected override async Task<ReadOnlyMemory<byte>> RefreshCacheAsync()
        {
            try
            {
                resourceLock.EnterReadLock();

                return await Task.FromResult(TokenCache).ConfigureAwait(false);
            }
            finally
            {
                resourceLock.ExitReadLock();
            }
        }

        /// <summary>
        /// Updates the token cache after a successful authentication request.
        /// </summary>
        /// <param name="tokenCacheUpdatedArgs">The arguments that represent the update for the token cache.</param>
        /// <returns>An instance of the <see cref="Task" /> class that represents the asynchronous operation.</returns>
        protected override async Task TokenCacheUpdatedAsync(TokenCacheUpdatedArgs tokenCacheUpdatedArgs)
        {
            try
            {
                resourceLock.EnterWriteLock();
                TokenCache = tokenCacheUpdatedArgs.UnsafeCacheData;

                await Task.CompletedTask.ConfigureAwait(false);
            }
            finally
            {
                resourceLock.ExitWriteLock();
            }
        }
    }
}