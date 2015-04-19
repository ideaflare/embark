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
        /// Get a connection to a local database
        /// </summary>
        /// <param name="directory">A folder path in which to save data</param>
        /// <returns>Client with db commands</returns>
        public static Client GetLocalDB(string directory = @"C:\MyTemp\Embark\Local\")
        {
            return new Client(directory);
        }

        /// <summary>
        /// Get a connection to a network accesible database
        /// </summary>
        /// <param name="address">IP Address / DNS Name of server. Example: "220.114.0.12" or "srv-embark-live"</param>
        /// <param name="port">Port used by server</param>
        /// <returns>Client with db commands</returns>
        public static Client GetNetworkDB(string address, int port = 8080)
        {
            return new Client(address, port);
        }

        /// <summary>
        /// Get a connection to a local database
        /// </summary>
        /// <param name="directory">A folder path in which to save data</param>
        /// <returns>Client with db commands</returns>>
        public Client(string directory = @"C:\MyTemp\Embark\Local\")
        {      
            this.dataStore = knownConnections.GetOrAdd(directory, (dir) => new TextFileRepository(dir, textConverter));
        }

        /// <summary>
        /// Get a connection to a network accesible database
        /// </summary>
        /// <param name="address">IP Address / DNS Name of server. Example: "220.114.0.12" or "srv-embark-live"</param>
        /// <param name="port">Port used by server</param>
        /// <returns>Client with db commands</returns>>
        public Client(string address, int port)
        {
            // TODO Test connection

            Uri uri = new Uri("http://" + address + ":" + port + "/embark/");

            this.dataStore = knownConnections.GetOrAdd(uri.AbsoluteUri, (server) => new WebServiceRepository(server));
        }

        private static ConcurrentDictionary<string, ITextDataStore> knownConnections = new ConcurrentDictionary<string, ITextDataStore>();

        private ITextDataStore dataStore;

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

        /// <summary>
        /// Get a type specific collection associated with the Client datastore and text converter.
        /// </summary>
        /// <typeparam name="T">The POCO class represented by the documents</typeparam>
        /// <param name="collectionName">Name of the collection</param>
        /// <returns>CollectionT class with commands to perform against the collection</returns>
        public Collection<T> GetCollection<T>(string collectionName) where T : class
        {
            var basic = GetCollection(collectionName);

            return new Collection<T>(basic);
        }

        private static void ValidateCollectionName(string collectionName)
        {
            if (collectionName == null || collectionName.Length < 1)
                throw new ArgumentException("Collection name should be at least one alphanumerical or underscore character.");

            if (!Regex.IsMatch(collectionName, "^[A-Za-z0-9_]+?$"))
                throw new NotSupportedException("Only alphanumerical & underscore characters supported in collection names.");
        }
    }
}
