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
        public static Client GetLocalDB(string directory = @"C:\MyTemp\Embark\Local\")
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
            this.dataStore = knownConnections.GetOrAdd(directory, (dir) => new TextFileRepository(dir, textConverter));
        }

        private Client(string address, int port)
        {
            // TODO Test connection

            Uri uri = new Uri("http://" + address + ":" + port + "/embark/");

            this.dataStore = knownConnections.GetOrAdd(uri.AbsoluteUri, (server) => new WebServiceRepository(server));
        }

        private static ConcurrentDictionary<string, ITextDataStore> knownConnections = new ConcurrentDictionary<string, ITextDataStore>();

        private ITextDataStore dataStore;

        //private ITextConverter textConverter = new JsonNetConverter();
        private ITextConverter textConverter = new JavascriptSerializerConverter();

        /// <summary>
        /// Basic collection named "Basic"
        /// </summary>
        public Collection Basic { get { return this["Basic"]; } }

        /// <summary>
        /// Indexer to return collection with name of index
        /// </summary>
        /// <param name="index">Name of the collection</param>
        /// <returns>Calls <see cref="Client.GetCollection"/> to return a collection with possible DB commands.</returns>
        public Collection this[string index]
        {
            get { return GetCollection(index); }
        }

        /// <summary>
        /// Get a collection associated with the Client datastore and text converter.
        /// </summary>
        /// <param name="collectionName">Name of the collection</param>
        /// <returns>Collection class with commands to perform against the collection</returns>
        public Collection GetCollection(string collectionName)
        {
            ValidateCollectionName(collectionName);

            return new Collection(collectionName, dataStore, textConverter);
        }

        //public CollectionT<T> GetCollection<T>(string collectionName)
        //{
        //    ValidateCollectionName(collectionName);

        //    return new CollectionT<T>(collectionName, dataStore, textConverter);
        //}

        private static void ValidateCollectionName(string collectionName)
        {
            if (collectionName == null || collectionName.Length < 1)
                throw new ArgumentException("Collection name should be at least one alphanumerical or underscore character.");

            if (!Regex.IsMatch(collectionName, "^[A-Za-z0-9_]+?$"))
                throw new NotSupportedException("Only alphanumerical & underscore characters supported in collection names.");
        }
    }
}
