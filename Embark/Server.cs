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
        /// <param name="directory">Path server will save data to. Default set to C:\MyTemp\Embark\Server\</param>
        /// <param name="port">port to use, default set to 8080</param>
        public Server(string directory = @"C:\MyTemp\Embark\Server\", int port = 8080)
        {
            ITextConverter textConverter = new JavascriptSerializerTextConverter();

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
