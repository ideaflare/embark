using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Embark.DataChannel;
using System.Collections.Concurrent;
using System.Threading;

namespace Embark.Storage
{
    sealed class CachedDataStore : IDataStore, IDisposable
    {
        private IDataStore diskDataStore;
        private IDataStore runtimeDataStore;

        private Task syncCacheTask;
        private CancellationTokenSource cancelSource = new CancellationTokenSource();

        private BlockingCollection<Task> asyncDiskTasks;

        public CachedDataStore(string directory, int maxAsyncOperations)
        {
            diskDataStore = new DiskDataStore(directory);
            runtimeDataStore = new RuntimeDataStore();

            asyncDiskTasks = new BlockingCollection<Task>(maxAsyncOperations);

            syncCacheTask = Task.Factory
                .StartNew(() => LoadCacheFromDisk())
                .ContinueWith((task) => ConsumeAsyncOperations());
        }

        public void WaitForAsyncComplete()
        {
            while (asyncDiskTasks.Count > 0)
                Thread.Sleep(1);
        }

        private void LoadCacheFromDisk()
        {
            foreach(var collection in diskDataStore.Collections)
                foreach(var document in diskDataStore.GetAll(collection))
            {
                    runtimeDataStore.Insert(collection, document.ID, document.Text);
            }
        }

        private void ConsumeAsyncOperations()
        {
            try
            {
                foreach (var task in asyncDiskTasks.GetConsumingEnumerable(cancelSource.Token))
                {
                    task.Start();
                    task.Wait();
                }
            }
            catch (OperationCanceledException) { }
        }

        private void AddAsyncDiskOperation(Action<IDataStore> diskAction)
            => asyncDiskTasks.Add(new Task(() => diskAction(diskDataStore)));

        IEnumerable<string> IDataStore.Collections
        {
            get
            {
                return runtimeDataStore.Collections;
            }
        }

        bool IDataStore.Delete(string tag, long id)
        {
            AddAsyncDiskOperation((store) => store.Delete(tag, id));

            return runtimeDataStore.Delete(tag, id);
        }

        string IDataStore.Get(string tag, long id)
            => runtimeDataStore.Get(tag, id);

        DataEnvelope[] IDataStore.GetAll(string tag)
            => runtimeDataStore.GetAll(tag);

        void IDataStore.Insert(string tag, long id, string objectToInsert)
        {
            AddAsyncDiskOperation((store) => store.Insert(tag, id, objectToInsert));

            runtimeDataStore.Insert(tag, id, objectToInsert);
        }

        bool IDataStore.Update(string tag, long id, string objectToUpdate)
        {
            AddAsyncDiskOperation((store) => store.Update(tag, id, objectToUpdate));

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
