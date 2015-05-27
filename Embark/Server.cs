using System;
using System.ServiceModel.Web;
using Embark.Storage;
using Embark.DataChannel;
using Embark.TextConversion;

namespace Embark
{
    /// <summary>
    /// Server that shares a local database hosted over WCF HTTP
    /// </summary>
    public sealed class Server : IDisposable
    {   
        /// <summary>
        /// Host a new network server
        /// </summary>
        /// <param name="directory">Path server will save data to
        /// <para>Example:  @"C:\MyTemp\Embark\Server\"</para></param>
        /// <param name="port">port to use, default set to 8080</param>
        public Server(string directory, int port = 8080)
            : this(directory, port, new JavascriptSerializerTextConverter())
        { }

        /// <summary>
        /// Host a new network server
        /// </summary>
        /// <param name="directory">Path server will save data to
        /// <para>Example:  @"C:\MyTemp\Embark\Server\"</para></param>
        /// <param name="port">port to use, default set to 8080</param>
        /// <param name="textConverter">Custom converter between objects and text.</param>
        public Server(string directory, int port, ITextConverter textConverter )
        {
            var store = new FileDataStore(directory);
            var textRepository = new LocalRepository(store, textConverter);

            Uri url = new Uri("http://localhost:" + port + "/embark/");
            webHost = new WebServiceHost(textRepository, url);
        }

        private WebServiceHost webHost;

        /// <summary>
        /// Open the server web host
        /// </summary>
        public void Start()
        {
            webHost.Open();
        }

        /// <summary>
        /// Close the server web host
        /// </summary>
        public void Stop()
        {
            webHost.Close();
        }

        /// <summary>
        /// Dispose the web host
        /// </summary>
        public void Dispose()
        {
            ((IDisposable)webHost).Dispose();
        }
    }
}
