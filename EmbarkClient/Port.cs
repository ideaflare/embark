using DataStore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace EmbarkClient
{
    public class Port : IChannel
    {
        public IChannel dataIO { get; private set; }

        //public Port(IPAddress ip, int port)
        //{
        //    throw new NotImplementedException();
        //}

        //public Port(string localFolder)
        //{
        //    dataIO = new Repository(localFolder);
        //}

        //public long Insert<T>(string tag, T something) where T : class
        //{
        //    return dataIO.Insert<T>(tag, something);
        //}

        //public T Get<T>(string tag, long id) where T : class
        //{
        //    return dataIO.Get<T>(tag, id);
        //}

        //public bool Update<T>(string tag, T something) where T : class
        //{
        //    return dataIO.Update<T>(tag, something);
        //}
        
        //public bool Delete(string tag, long id)
        //{
        //    return dataIO.Delete(tag, id);
        //}

        //public List<T> GetWhere<T>(string tag, T newValue, T oldValue, T optionalEndrange = null) where T : class
        //{
        //    return dataIO.GetWhere<T>(tag, newValue, oldValue, optionalEndrange);
        //}

        //public int UpdateWhere<T>(string tag, T newValue, T oldValue, T optionalEndrange = null) where T : class
        //{
        //    return dataIO.UpdateWhere<T>(tag, newValue, oldValue, optionalEndrange);
        //}

        //public int DeleteWhere<T>(string tag, T newValue, T oldValue, T optionalEndrange = null) where T : class
        //{
        //    return dataIO.DeleteWhere<T>(tag, newValue, oldValue, optionalEndrange);
        //}
    }
}
