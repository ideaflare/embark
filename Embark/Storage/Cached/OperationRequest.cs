namespace Embark.Storage.Cached
{
    class OperationRequest
    {
        public OperationRequest(StoreOperation storeOperation, string tag, long id, string objectToUpdate = "")
        {
            StoreOperation = storeOperation;
            Tag = tag;
            ID = id;
            ObjectToUpdate = objectToUpdate;
        }

        public StoreOperation StoreOperation { get; }
        public string Tag { get; }
        public long ID { get; }
        public string ObjectToUpdate { get; }
    }
}
