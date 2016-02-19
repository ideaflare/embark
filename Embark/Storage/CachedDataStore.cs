using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Embark.DataChannel;

namespace Embark.Storage
{
    sealed class CachedDataStore : IDataStore, IDisposable
    {
        private IDataStore diskDataStore;
        private IDataStore runtimeDataStore;

        private Task syncCacheTask;

        // TODO add commands to collection - create command/object enum.

        public CachedDataStore(string directory)
        {
            diskDataStore = new DiskDataStore(directory);
            runtimeDataStore = new RuntimeDataStore();
            syncCacheTask = Task.Factory
                .StartNew(() => LoadCacheFromDisk())
                .ContinueWith((task) => SyncCommandsToDisk());
        }

        private void LoadCacheFromDisk()
        {
            foreach(var collection in diskDataStore.Collections)
                foreach(var document in diskDataStore.GetAll(collection))
            {
                    runtimeDataStore.Insert(collection, document.ID, document.Text);
            }
        }

        private void SyncCommandsToDisk()
        {
            throw new NotImplementedException();
        }

        IEnumerable<string> IDataStore.Collections
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        bool IDataStore.Delete(string tag, long id)
        {
            throw new NotImplementedException();
        }

       

        string IDataStore.Get(string tag, long id)
        {
            throw new NotImplementedException();
        }

        DataEnvelope[] IDataStore.GetAll(string tag)
        {
            throw new NotImplementedException();
        }

        void IDataStore.Insert(string tag, long id, string objectToInsert)
        {
            throw new NotImplementedException();
        }

        bool IDataStore.Update(string tag, long id, string objectToUpdate)
        {
            throw new NotImplementedException();
        }

        void IDisposable.Dispose()
        {
            // TODO cancel
            syncCacheTask.Dispose();
        }
    }
}
