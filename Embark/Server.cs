using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel.Web;
using System.Text;
using System.Threading.Tasks;
using Embark.Storage;
using Embark.Conversion;
using Embark.Interfaces;

namespace Embark
{
    /// <summary>
    /// Server that has a TextFileRepository who's commands get invoked by remote Clients
    /// </summary>
    public sealed class Server : IDisposable
    {   
        /// <summary>
        /// Host a new network server
        /// </summary>
        /// <param name="directory">directory server will save data to. If null default set to C:\MyTemp\Embark\Server\</param>
        /// <param name="port">port to use, default set to 8080</param>
        public Server(string directory = null, int port = 8080)
        {
            if (directory == null)
                directory = @"C:\MyTemp\Embark\Server\";
            
            ITextConverter textConverter = new JavascriptSerializerConverter();

            var textRepository = new TextFileRepository(directory, textConverter);

            Uri url = new Uri("http://localhost:" + port + "/embark/");
            webHost = new WebServiceHost(textRepository, url);
        }

        private WebServiceHost webHost;

        /// <summary>
        /// Open the web host of the server
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
