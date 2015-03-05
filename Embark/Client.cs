using Embark.Cache;
using Embark.Interfaces;
using Newtonsoft.Json;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Embark
{
    /// <summary>
    /// Provider of a connection to a local or server database
    /// </summary>
    public class Client : IChannel
    {
        /// <summary>
        /// New client connection to a server
        /// </summary>
        /// <param name="ip">IP Address of server</param>
        /// <param name="port">Port data is sent/received</param>
        /// <returns>IChannel with db commands</returns>
        public Client(string ip, int port)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Starts a new embark db on a local directory,
        /// or connects to local current process db if one is already running
        /// </summary>
        /// <param name="directory">A folder path in which to save data</param>
        /// <returns>IChannel with db commands</returns>
        public Client(string directory)
        {
            if (!directory.EndsWith("\\"))
                directory += "\\";

            var db = localDataBases.GetOrAdd(directory, (dir) => new Repository(dir));

            this.channel = db;
        }

        private static ConcurrentDictionary<string, Repository> localDataBases = new ConcurrentDictionary<string, Repository>();

        private Repository channel;
        
        public long Insert<T>(string tag, T objectToInsert) where T : class
        {
            string jsonText = JsonConvert.SerializeObject(objectToInsert, Formatting.Indented);
            return channel.Insert(tag, jsonText);
        }

        public T Get<T>(string tag, long id) where T : class
        {
            var jsonText = channel.Get(tag, id);

            if (jsonText == null)
            {
                return null;
            }
            else return JsonConvert.DeserializeObject<T>(jsonText);
        }

        public bool Update(string tag, long id, object objectToUpdate)
        {
            string jsonText = JsonConvert.SerializeObject(objectToUpdate, Formatting.Indented);
            return channel.Update(tag, id, jsonText);
        }

        public bool Delete(string tag, long id)
        {
            return channel.Delete(tag, id);
        }
 
        public IEnumerable<T> GetWhere<T>(string tag, object searchObject, object optionalEndrange = null) where T : class
        {
            var jsonTextEnumerable = channel.GetWhere(tag, searchObject, optionalEndrange);
            foreach(var jsonText in jsonTextEnumerable)
            {
                yield return JsonConvert.DeserializeObject<T>(jsonText);
            }
        }

        public int UpdateWhere(string tag, object newValue, object searchObject, object optionalEndrange = null)
        {
            throw new NotImplementedException();
        }

        public int DeleteWhere(string tag, object searchObject, object optionalEndrange = null)
        {
            throw new NotImplementedException();
        }
    }
}
