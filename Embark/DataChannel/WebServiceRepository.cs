using System;
using System.Collections.Generic;
using System.ServiceModel;
using System.ServiceModel.Description;

namespace Embark.DataChannel
{
    internal class WebServiceRepository : ITextRepository
    {
        public WebServiceRepository(string serviceAbsoluteUri)
        {
            this.serviceAbsoluteUri = serviceAbsoluteUri;
        }

        private string serviceAbsoluteUri;

        private T CallRemoteDatastore<T>(Func<ITextRepository,T> func)
        {
            using (ChannelFactory<ITextRepository> cf = new ChannelFactory<ITextRepository>(new WebHttpBinding(), serviceAbsoluteUri))
            {
                cf.Endpoint.Behaviors.Add(new WebHttpBehavior());
                var webChannel = cf.CreateChannel();
                return func(webChannel);
            }            
        }

        long ITextRepository.Insert(string tag, string objectToInsert)
            => CallRemoteDatastore((store) => store.Insert(tag, objectToInsert));

        bool ITextRepository.Update(string tag, string id, string objectToUpdate)
            => CallRemoteDatastore((store) => store.Update(tag, id, objectToUpdate));

        bool ITextRepository.Delete(string tag, string id)
            => CallRemoteDatastore((store) => store.Delete(tag, id));

        string ITextRepository.Get(string tag, string id)
            => CallRemoteDatastore((store) => store.Get(tag, id));

        IEnumerable<DataEnvelope> ITextRepository.GetAll(string tag)
            => CallRemoteDatastore((store) => store.GetAll(tag));

        IEnumerable<DataEnvelope> ITextRepository.GetWhere(string tag, string searchObject)
            => CallRemoteDatastore((store) => store.GetWhere(tag, searchObject));

        IEnumerable<DataEnvelope> ITextRepository.GetBetween(string tag, string startRange, string endRange)
            => CallRemoteDatastore((store) => store.GetBetween(tag, startRange, endRange));
    }
}
