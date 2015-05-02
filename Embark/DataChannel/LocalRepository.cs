using System.Linq;
using System.Collections.Generic;
using System.ServiceModel;
using Embark.Storage;
using Embark.TextConversion;

namespace Embark.DataChannel
{
    [ServiceBehavior(
        InstanceContextMode = InstanceContextMode.Single,
        ConcurrencyMode = ConcurrencyMode.Multiple)]
    internal class LocalRepository : ITextRepository
    {
        public LocalRepository(FileDataStore dataStore, ITextConverter textComparer)
        {
            this.dataStore = dataStore;
            this.textComparer = textComparer;
        }

        private FileDataStore dataStore;
        private ITextConverter textComparer;

        // Basic
        long ITextRepository.Insert(string tag, string objectToInsert)
        {
            return dataStore.Insert(tag, objectToInsert);
        }

        bool ITextRepository.Update(string tag, string id, string objectToUpdate)
        {
            return dataStore.Update(tag, id, objectToUpdate);
        }

        bool ITextRepository.Delete(string tag, string id)
        {
            return dataStore.Delete(tag, id);
        }

        string ITextRepository.Get(string tag, string id)
        {
            return dataStore.Get(tag, id);
        }

        IEnumerable<DataEnvelope> ITextRepository.GetAll(string tag)
        {
            return this.GetAll(tag);
        }
        private IEnumerable<DataEnvelope> GetAll(string tag)
        {
            return dataStore.GetAll(tag);
        }

        IEnumerable<DataEnvelope> ITextRepository.GetWhere(string tag, string searchObject)
        {
            var allFiles = this.GetAll(tag);

            var propertyLookup = textComparer.ToComparisonObject(searchObject);

            var matches = allFiles
                .Select(envelope => new
                {
                    envelope = envelope,
                    graph = textComparer.ToComparisonObject(envelope.Text)
                })
                .Where(comparison => textComparer.IsMatch(propertyLookup, comparison.graph))
                .Select(e => e.envelope);

            return matches;
        }

        IEnumerable<DataEnvelope> ITextRepository.GetBetween(string tag, string startRange, string endRange)
        {
            var allFiles = this.GetAll(tag);

            var startLookup = textComparer.ToComparisonObject(startRange);
            var endLookup = textComparer.ToComparisonObject(endRange);

            var matches = allFiles
               .Select(envelope => new
               {
                   envelope = envelope,
                   graph = textComparer.ToComparisonObject(envelope.Text)
               })
               .Where(comparison => textComparer.IsBetweenMatch(startLookup, endLookup, comparison.graph))
               .Select(e => e.envelope);

            return matches;
        }
    }
}
