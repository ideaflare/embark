using System.Collections.Generic;
using Embark.DataChannel;

namespace Embark.Storage
{
    internal interface IDataStore
    {
        IEnumerable<string> Collections { get; }

        void Insert(string tag, long id, string objectToInsert);
        bool Update(string tag, long id, string objectToUpdate);
        bool Delete(string tag, long id);

        string Get(string tag, long id);
        DataEnvelope[] GetAll(string tag);
    }
}