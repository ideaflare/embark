using System.Linq;
using System.Collections.Generic;
using System.ServiceModel;
using Embark.Storage;
using Embark.TextConversion;
using Embark.Interaction.Concurrency;

namespace Embark.DataChannel
{
    [ServiceBehavior(
        InstanceContextMode = InstanceContextMode.Single,
        ConcurrencyMode = ConcurrencyMode.Multiple)]
    internal class LocalRepository : ITextRepository
    {
        public LocalRepository(IDataStore dataStore, ITextConverter textComparer)
        {
            this.dataStore = dataStore;
            this.textComparer = textComparer;

            var futureTimeStamp = System.DateTime.Now.AddMilliseconds(100).Ticks;

            keyProvider = new DocumentKeySource(futureTimeStamp);

            hashLock = new HashLock(avaliableLocks: 2000);
        }

        private IDataStore dataStore;
        private ITextConverter textComparer;
        private DocumentKeySource keyProvider;
        private HashLock hashLock;

        // Basic
        long ITextRepository.Insert(string tag, string objectToInsert)
        {
            var id = keyProvider.GetNewKey();

            lock (hashLock.GetLock(id))
                dataStore.Insert(tag, id, objectToInsert);

            return id;
        }

        bool ITextRepository.Update(string tag, long id, string objectToUpdate)
        {
            lock (hashLock.GetLock(id))
                return dataStore.Update(tag, id, objectToUpdate);
        }

        bool ITextRepository.Delete(string tag, long id)
        {
            lock (hashLock.GetLock(id))
                return dataStore.Delete(tag, id);
        }

        string ITextRepository.Get(string tag, long id)
        {
            lock (hashLock.GetLock(id))
                return dataStore.Get(tag, id);
        }

        IEnumerable<DataEnvelope> ITextRepository.GetAll(string tag) => dataStore.GetAll(tag);

        IEnumerable<DataEnvelope> ITextRepository.GetWhere(string tag, string searchObject)
        {
            var propertyLookup = textComparer.ToComparisonObject(searchObject);

            return GetByFilter(tag, compareObject 
                => textComparer.IsMatch(propertyLookup, compareObject));
        }

        IEnumerable<DataEnvelope> ITextRepository.GetBetween(string tag, string startRange, string endRange)
        {
            var startLookup = textComparer.ToComparisonObject(startRange);
            var endLookup = textComparer.ToComparisonObject(endRange);

            return GetByFilter(tag, compareObject 
                => textComparer.IsBetweenMatch(startLookup, endLookup, compareObject));
        }

        private IEnumerable<DataEnvelope> GetByFilter(string tag, System.Func<object, bool> filterEnvelopes)
            =>
            dataStore.GetAll(tag)
            .Where(envelope => filterEnvelopes(textComparer.ToComparisonObject(envelope.Text)));
    }
}
