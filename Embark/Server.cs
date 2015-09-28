using System;
using System.ServiceModel.Web;
using Embark.Storage;
using Embark.DataChannel;
using Embark.TextConversion;
using System.Linq;

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
        /// <param name="port">port to use, default set to 8030</param>
        /// <param name="textConverter">Custom converter between objects and text.
        /// <para>If parameter is NULL, the textConverter is set to default json converter.</para>
        /// </param>
        public Server(string directory, int port = 8030, ITextConverter textConverter = null)
        {
            if (textConverter == null)
                textConverter = new JavascriptSerializerTextConverter();

            var store = new DiskDataStore(directory);
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
            webHost.Opening += ConfigureEnpointBinding;

            webHost.Open();
        }

        private void ConfigureEnpointBinding(object sender, EventArgs e)
        {
            var endpointBinding = (System.ServiceModel.WebHttpBinding)
                ((WebServiceHost)sender)
                .Description
                .Endpoints
                .Single(endpoint => endpoint.Contract.ContractType == typeof(ITextRepository))
                .Binding;

            endpointBinding.MaxReceivedMessageSize = int.MaxValue;
            endpointBinding.MaxBufferSize = int.MaxValue;
        }

        /// <summary>
        /// Close the server web host
        /// </summary>
        public void Stop() => webHost.Close();

        /// <summary>
        /// Dispose the web host
        /// </summary>
        public void Dispose() => ((IDisposable)webHost)?.Dispose();
    }
}
