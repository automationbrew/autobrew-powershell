namespace AutoBrew.PowerShell.Factories
{
    using System.Collections.Concurrent;
    using System.Net;
    using System.Net.Http;
    using Microsoft.Identity.Client;
    using Models.Authentication;
    using Network;

    /// <summary>
    /// The factory used to create instance of the <see cref="HttpClient" /> class for use by the Microsoft Authentication Library (MSAL).
    /// </summary>
    public class MsalHttpClientFactory : IMsalHttpClientFactory
    {
        /// <summary>
        /// The collection of factories used to create instances of the <see cref="HttpClient" /> class.
        /// </summary>
        private static readonly IDictionary<MsalHttpClientFactoryType, MsalHttpClientFactory> factories = new ConcurrentDictionary<MsalHttpClientFactoryType, MsalHttpClientFactory>();

        /// <summary>
        /// An instance of the <see cref="HttpClient" /> class used to perform HTTP operations.
        /// </summary>
        private readonly HttpClient httpClient;

        /// <summary>
        /// Initializes a new instance of the <see cref="MsalHttpClientFactory" /> class.
        /// </summary>
        /// <param name="handler">The handler responsible for processing the HTTP response messages.</param>
        public MsalHttpClientFactory(DelegatingHandler handler)
        {
            handler.AssertNotNull(nameof(handler));

            httpClient = new HttpClient(handler, false);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MsalHttpClientFactory" /> class.
        /// </summary>
        /// <param name="handler">The handler responsible for processing the HTTP response messages.</param>
        public MsalHttpClientFactory(HttpClientHandler handler)
        {
            handler.AssertNotNull(nameof(handler));

            httpClient = new HttpClient(handler, false);
        }

        /// <summary>
        /// Gets an instance of the <see cref="HttpClient" /> class that will be used to communicate with Azure Active Directory.
        /// </summary>
        /// <returns>An instance of the <see cref="HttpClient" /> class that will be used to communicate with Azure Active Directory.</returns>
        public HttpClient GetHttpClient()
        {
            return httpClient;
        }

        /// <summary>
        /// Gets an instance of the <see cref="MsalHttpClientFactory" /> class associated with the specified <see cref="MsalHttpClientFactoryType" />.
        /// </summary>
        /// <param name="type">The type of HTTP client factory.</param>
        /// <returns>
        /// An instance of the <see cref="MsalHttpClientFactory" /> class associated with the specified <see cref="MsalHttpClientFactoryType" />.
        /// </returns>
        public static MsalHttpClientFactory GetInstance(MsalHttpClientFactoryType type)
        {
            return factories[type];
        }

        /// <summary>
        /// Initializes the collection of MSAL HTTP client factories.
        /// </summary>
        public static void Initialize()
        {
            if (factories.ContainsKey(MsalHttpClientFactoryType.Proxy) == false)
            {
                factories[MsalHttpClientFactoryType.Proxy] = new MsalHttpClientFactory(new HttpClientHandler
                {
                    Proxy = new WebProxy
                    {
                        Address = new Uri("socks5://127.0.0.1:9050"),
                        BypassProxyOnLocal = false,
                        UseDefaultCredentials = true
                    }
                });
            }

            if (factories.ContainsKey(MsalHttpClientFactoryType.RefreshToken) == false)
            {
                factories[MsalHttpClientFactoryType.RefreshToken] = new MsalHttpClientFactory(new RefreshTokenDelegatingHandler()
                {
                    InnerHandler = new HttpClientHandler()
                });
            }
        }
    }
}