using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Embark.DataChannel;
using System.Collections.Concurrent;
using System.Threading;
using Embark.Storage.Cached;

namespace Embark.Storage
{
    sealed class CachedDataStore : IDataStore, IDisposable
    {
        private IDataStore diskDataStore;
        private IDataStore runtimeDataStore;

        private Task syncCacheTask;
        private CancellationTokenSource cancelSource = new CancellationTokenSource();

        private BlockingCollection<OperationRequest> asyncDiskTasks;

        public CachedDataStore(string directory, int maxAsyncOperations)
        {
            diskDataStore = new DiskDataStore(directory);
            runtimeDataStore = new RuntimeDataStore();

            asyncDiskTasks = new BlockingCollection<OperationRequest>(maxAsyncOperations);

            LoadCacheFromDisk();

            syncCacheTask = Task.Factory.StartNew(ConsumeAsyncOperations);
        }

        public void WaitForAsyncComplete()
        {
            while (asyncDiskTasks.Count > 0)
                Thread.Sleep(1);
        }

        private void ConsumeAsyncOperations()
        {
            try
            {
                foreach (var op in asyncDiskTasks.GetConsumingEnumerable(cancelSource.Token))
                {
                    switch (op.StoreOperation)
                    {
                        case StoreOperation.Insert:
                            diskDataStore.Insert(op.Tag, op.ID, op.ObjectToUpdate);
                            break;
                        case StoreOperation.Update:
                            diskDataStore.Update(op.Tag, op.ID, op.ObjectToUpdate);
                            break;
                        case StoreOperation.Delete:
                            diskDataStore.Delete(op.Tag, op.ID);
                            break;
                        default:
                            break;
                    }
                }
            }
            catch (OperationCanceledException) { }
        }

        private void LoadCacheFromDisk()
        {
            foreach(var collection in diskDataStore.Collections)
                foreach(var document in diskDataStore.GetAll(collection))
            {
                    runtimeDataStore.Insert(collection, document.ID, document.Text);
            }
        }

        IEnumerable<string> IDataStore.Collections
        {
            get
            {
                return runtimeDataStore.Collections;
            }
        }

        bool IDataStore.Delete(string tag, long id)
        {
            asyncDiskTasks.Add(new OperationRequest(StoreOperation.Delete, tag, id));

            return runtimeDataStore.Delete(tag, id);
        }

        string IDataStore.Get(string tag, long id)
            => runtimeDataStore.Get(tag, id);

        DataEnvelope[] IDataStore.GetAll(string tag)
            => runtimeDataStore.GetAll(tag);

        void IDataStore.Insert(string tag, long id, string objectToInsert)
        {
            asyncDiskTasks.Add(new OperationRequest(StoreOperation.Insert, tag, id, objectToInsert));

            runtimeDataStore.Insert(tag, id, objectToInsert);
        }

        bool IDataStore.Update(string tag, long id, string objectToUpdate)
        {
            asyncDiskTasks.Add(new OperationRequest(StoreOperation.Update, tag, id, objectToUpdate));

            return runtimeDataStore.Update(tag, id, objectToUpdate);
        }

        void IDisposable.Dispose()
        {
            asyncDiskTasks.CompleteAdding();

            cancelSource.Cancel();

            WaitForAsyncComplete();
            
            syncCacheTask.Dispose();
        }

        
    }
}
