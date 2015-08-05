using System;
using System.Text.RegularExpressions;
using Embark.Storage;
using Embark.DataChannel;
using Embark.Interaction;
using Embark.TextConversion;

namespace Embark
{
    /// <summary>
    /// Connection to use/consume a local or server database
    /// </summary>
    public class Client
    {
        /// <summary>
        /// Get a connection to a local database
        /// </summary>
        /// <param name="directory">The path of where to save data</param>
        /// <returns>Client with db commands</returns>
        public static Client GetLocalDB(string directory) => new Client(directory);

        /// <summary>
        /// Get a connection to a server database
        /// </summary>
        /// <param name="address">IP Address / DNS Name of server. Example: "220.114.0.12" or "srv-embark-live"</param>
        /// <param name="port">Port used by server</param>
        /// <returns>Client with db commands</returns>
        public static Client GetNetworkDB(string address, int port = 8080) => new Client(address, port);

        /// <summary>
        /// Modify a local databas,
        /// </summary>
        /// <param name="directory">The path of where to save data
        /// <para>Example: @"C:\MyTemp\Embark\Local\"</para>
        /// </param>
        /// <param name="textConverter">Custom converter between objects and text.
        /// <para>If parameter is NULL, the textConverter is set to default json converter.</para>
        /// </param>
        /// <returns>Client with db commands</returns>>
        public Client(string directory, ITextConverter textConverter = null)
        {
            var store = new FileDataStore(directory);

            this.textConverter = textConverter ?? new JavascriptSerializerTextConverter();

            dataStore = new LocalRepository(store, this.textConverter);
        }
       
        /// <summary>
        /// Get a connection to a server database
        /// </summary>
        /// <param name="address">IP Address / DNS Name of server. Example: "220.114.0.12" or "srv-embark-live"</param>
        /// <param name="port">Port used by server</param>
        /// <param name="textConverter">Custom converter between objects and text used on the server
        /// <para>If parameter is NULL, the textConverter is set to default json converter.</para>
        /// </param>
        /// <returns>Client with db commands</returns>>
        public Client(string address, int port, ITextConverter textConverter = null)
        {
            // TODO Test connection

            Uri uri = new Uri("http://" + address + ":" + port + "/embark/");

            this.textConverter = textConverter ?? new JavascriptSerializerTextConverter();

            dataStore = new WebServiceRepository(uri.AbsoluteUri);
        }

        private ITextRepository dataStore;
        private ITextConverter textConverter;

        /// <summary>
        /// Basic collection named "Basic"
        /// </summary>
        public Collection Basic => this["Basic"];

        /// <summary>
        /// Indexer to return collection with same Name as lookup
        /// </summary>
        /// <param name="index">Name of the collection</param>
        /// <returns>Calls <see cref="GetCollection"/> to return a collection with possible DB commands.</returns>
        public Collection this[string index] => GetCollection(index);

        /// <summary>
        /// Get a collection to read/write documents to/from
        /// </summary>
        /// <param name="collectionName">Name of the collection</param>
        /// <returns>Collection class with commands to perform against the collection</returns>
        public Collection GetCollection(string collectionName)
        {
            ValidateCollectionName(collectionName);

            return new Collection(collectionName, dataStore, textConverter);
        }

        /// <summary>
        /// Get a type-specific collection to read/write documents to/from
        /// </summary>
        /// <typeparam name="T">The POCO or DTO class represented by the documents</typeparam>
        /// <param name="collectionName">Name of the collection</param>
        /// <returns>CollectionT class with commands to perform against the collection</returns>
        public Collection<T> GetCollection<T>(string collectionName) where T : class
        {
            var basic = GetCollection(collectionName);

            return new Collection<T>(basic);
        }

        /// <summary>
        /// Get a <see cref="IDataEntry"/> based Collection 
        /// </summary>
        /// <typeparam name="T">The POCO or DTO class represented by the documents</typeparam>
        /// <param name="collectionName">Name of the collection</param>
        /// <returns>DataEntryCollection class with commands to perform against the <see cref="IDataEntry"/> based collection</returns>
        public DataEntryCollection<T> GetDataEntryCollection<T>(string collectionName) where T : class, IDataEntry
        {
            var basic = GetCollection(collectionName);

            return new DataEntryCollection<T>(basic);
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
