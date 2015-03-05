using Embark.Interfaces;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Embark.Cache
{
    public class Repository 
    {
        public Repository(string dataDirectory)
        {
            var collectionsFolder = dataDirectory + @"\Collections\";
            var keysFolder = dataDirectory + @"\Keys\";
            
            this.keyProvider = new KeyProvider(keysFolder);
            this.tagPaths = new CollectionPaths(collectionsFolder);
        }
        
        private KeyProvider keyProvider;
        private CollectionPaths tagPaths;
        private object syncRoot = new object();

        // Basic
        public long Insert(string tag, string jsonText)
        {
            // Get ID from IDGen
            var key = keyProvider.GetKey(tag);
                
            // TODO 3 offload to queue that gets processed by task
            var savePath = tagPaths.GetJsonPath(tag, key);

            // TODO 1 NB get a document only lock, instead of all repositories lock
            lock (syncRoot)
            {
                // Save object to tag dir
                File.WriteAllText(savePath, jsonText);

                //Return ID to client
                return key;
            }
        }
        
        public string Get(string tag, long id)
        {
            var savePath = tagPaths.GetJsonPath(tag, id);

            string jsonText;
            // TODO lock row only
            lock (syncRoot)
            {
                if (!File.Exists(savePath))
                    return null;

                jsonText = File.ReadAllText(savePath);
            }
            return jsonText;
        }

        public bool Update(string tag, long id, string jsonText)
        {
            var savePath = tagPaths.GetJsonPath(tag, id);
            
            lock(syncRoot)
            {
                if (!File.Exists(savePath))
                    return false;
                else
                {
                    File.WriteAllText(savePath, jsonText);
                    return true;
                }
            }
        }

        public bool Delete(string tag, long id)
        {
            var savePath = tagPaths.GetJsonPath(tag, id);

            lock (syncRoot)
            {
                if (File.Exists(savePath))
                {
                    File.Delete(savePath);
                    return true;
                }
                else return false;
            }
        }


        // Range

        public IEnumerable<string> GetWhere(string tag, object queryObject, object optionalEndrange = null)
        {
            lock(syncRoot)
            {
                var tagDir = tagPaths.GetTagDir(tag);
                var allItems = Directory.GetFiles(tagDir);

                var query = JObject.FromObject(queryObject);
                foreach (var item in allItems)
                {
                    var json = File.ReadAllText(item);
                    var target = (JObject)JsonConvert.DeserializeObject(json);
                    if (IsMatch(query, target))
                        yield return json;
                }
            }
        }

        public int UpdateWhere(string tag, object newValue, object oldValue, object optionalEndrange = null) 
        {
            throw new NotImplementedException();
        }
                
        public int DeleteWhere(string tag, object newValue, object oldValue, object optionalEndrange = null)
        {
            throw new NotImplementedException();
        }

        public bool IsMatch(JObject query, JObject target)
        {
            var tProp = target.Properties();
            return query
                .Properties()
                .All(tp => tProp.Any(qp =>
                    tp.Name == qp.Name &&
                    tp.Value.ToString() == qp.Value.ToString()));
        }
    }
}
