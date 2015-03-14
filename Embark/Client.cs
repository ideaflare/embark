using Embark.Cache;
using Embark.Interfaces;
using Newtonsoft.Json;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        /// <param name="ip">IP Address of server</param>
        /// <param name="port">Port data is sent/received</param>
        /// <returns>Client with db commands</returns>
        public static Client GetNetworkDB(string ip, int port)
        {
            return new Client(ip, port);
        }

        private Client(string directory = null)
        {
            if (directory == null)
                directory = Directory.GetCurrentDirectory();            
            
            this.localDB = localDBs.GetOrAdd(directory, (dir) => new Repository(dir));
        }

        private Client(string ip, int port)
        {
            // Test connection

            // Return network client

            throw new NotImplementedException();
        }

        private static ConcurrentDictionary<string, Repository> localDBs = new ConcurrentDictionary<string, Repository>();

        private Repository localDB;

        public IDataStore Generic { get { return this["Generic"]; } }

        public IDataStore this[string index]
        {
            get
            {
                return new Collection(index, localDB);
            }
        }
    }
}
