using Embark.Conversion;
using Embark.Interfaces;
using Embark.Storage;
using System;
using System.Collections.Concurrent;
using System.Text.RegularExpressions;

namespace Embark
{
    /// <summary>
    /// Provider of a connection to a local or server database
    /// </summary>
    public class Client
    {
        /// <summary>
        /// Starts a new embark db on a local directory,
        /// or connects to local current process db if one is already running
        /// </summary>
        /// <param name="directory">A folder path in which to save data</param>
        /// <returns>Client with db commands</returns>
        public static Client GetLocalDB(string directory = null)
        {
            return new Client(directory);
        }

        /// <summary>
        /// New client connection to a server
        /// </summary>
        /// <param name="address">IP Address of server</param>
        /// <param name="port">Port data is sent/received</param>
        /// <returns>Client with db commands</returns>
        public static Client GetNetworkDB(string address = null, int port = 8080)
        {
            return new Client(address, port);
        }

        private Client(string directory)
        {
            if (directory == null)
                directory = @"C:\MyTemp\Embark\Local\";          
            
            this.dataStore = knownConnections.GetOrAdd(directory, (dir) => new TextFileRepository(dir, textConverter));
        }

        private Client(string address, int port)
        {
            // TODO Test connection

            Uri uri = new Uri("http://" + address + ":" + port + "/embark/");
            this.dataStore = new WebServiceRepository(uri);
        }

        private static ConcurrentDictionary<string, ITextDataStore> knownConnections = new ConcurrentDictionary<string, ITextDataStore>();

        private ITextDataStore dataStore;
        //private ITextConverter textConverter = new JsonNetConverter();
        private ITextConverter textConverter = new JavascriptSerializerConverter();

        public Collection Generic { get { return this["Generic"]; } }

        public Collection this[string index]
        {
            get { return GetCollection(index); }
        }

        public Collection GetCollection(string collectionName)
        {
            if (collectionName == null || collectionName.Length < 1)
                throw new ArgumentException("Collection name should be at least one alphanumerical or underscore character.");

            if (!Regex.IsMatch(collectionName, "^[A-Za-z0-9_]+?$"))
                throw new NotSupportedException("Only alphanumerical & underscore characters supported in collection names.");

            return new Collection(collectionName, dataStore, textConverter);
        }
    }
}
