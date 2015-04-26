using Embark.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.ServiceModel;
using System.ServiceModel.Description;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;

namespace Embark.Storage
{
    internal class WebServiceRepository : ITextDataStore
    {
        public WebServiceRepository(string serviceAbsoluteUri)
        {
            this.serviceAbsoluteUri = serviceAbsoluteUri;
        }

        private T CallRemoteDatastore<T>(Func<ITextDataStore,T> func)
        {
            using (ChannelFactory<ITextDataStore> cf = new ChannelFactory<ITextDataStore>(new WebHttpBinding(), this.serviceAbsoluteUri))
            {
                cf.Endpoint.Behaviors.Add(new WebHttpBehavior());
                var webChannel = cf.CreateChannel();
                return func(webChannel);
            }            
        }

        private string serviceAbsoluteUri;

        long ITextDataStore.Insert(string tag, string objectToInsert)
        { 
            return CallRemoteDatastore<long>((store) => store.Insert(tag, objectToInsert));
        }

        bool ITextDataStore.Update(string tag, string id, string objectToUpdate)
        {
            return CallRemoteDatastore<bool>((store) => store.Update(tag, id, objectToUpdate));
        }

        bool ITextDataStore.Delete(string tag, string id)
        {
            return CallRemoteDatastore<bool>((store) => store.Delete(tag, id));
        }

        string ITextDataStore.Get(string tag, string id)
        {
            return CallRemoteDatastore<string>((store) => store.Get(tag, id));
        }

        IEnumerable<DataEnvelope> ITextDataStore.GetAll(string tag)
        {
            return CallRemoteDatastore<IEnumerable<DataEnvelope>>((store) => store.GetAll(tag));
        }

        IEnumerable<DataEnvelope> ITextDataStore.GetWhere(string tag, string searchObject)
        {
            return CallRemoteDatastore<IEnumerable<DataEnvelope>>((store) => store.GetWhere(tag, searchObject));
        }

        IEnumerable<DataEnvelope> ITextDataStore.GetBetween(string tag, string startRange, string endRange)
        {
            return CallRemoteDatastore<IEnumerable<DataEnvelope>>((store) => store.GetBetween(tag, startRange, endRange));
        }        
    }
}
