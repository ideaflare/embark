using System.Collections.Generic;
using System.ServiceModel;
using System.ServiceModel.Web;

namespace Embark.DataChannel
{
    /// <summary>
    /// All commands to insert/get/update/delete documents
    /// </summary>
    [ServiceContract()]
    internal interface ITextRepository
    {
        /// <summary>
        /// Insert a json serializable object to save to a collection
        /// </summary>
        /// <param name="tag">A name of the collection to save it in</param>
        /// <param name="objectToInsert">Anything json serializable/deserializable object to save</param>
        /// <returns>An Int64 ID unique to the collection the object is saved in</returns>
        [OperationContract,
        WebInvoke(Method = "POST",
            UriTemplate = "{tag}",
            RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json
            )]
        long Insert(string tag, string objectToInsert);

        [OperationContract,
        WebInvoke(Method = "PUT",
            UriTemplate = "{tag}/{id}/{objectToUpdate}",
            ResponseFormat = WebMessageFormat.Json)]
        bool Update(string tag, string id, string objectToUpdate);

        [OperationContract,
        WebInvoke(Method = "DELETE",
            UriTemplate = "{tag}/{id}",
            ResponseFormat = WebMessageFormat.Json)]
        bool Delete(string tag, string id);

        [OperationContract, 
        WebGet(UriTemplate = "{tag}/{id}",
            ResponseFormat = WebMessageFormat.Json)]
        string Get(string tag, string id);

        // TODO 5 Test - what happens if concurrent modify and delete operations occur while iterating
        //        Test - maximum size of insert / read queries ? 
        // OPT Instead of returning IEnumerable object, return Iterator with ID list that calls select(id) internally.

        [OperationContract,
        WebGet(UriTemplate = "{tag}/All/",
            ResponseFormat = WebMessageFormat.Json)]
        IEnumerable<DataEnvelope> GetAll(string tag);

        // Like
        [OperationContract,
        WebGet(UriTemplate = "{tag}/Where/{searchObject}",
            ResponseFormat = WebMessageFormat.Json)]
        IEnumerable<DataEnvelope> GetWhere(string tag, string searchObject);

        // Between
        [OperationContract,
        WebGet(UriTemplate = "{tag}/Between/{startRange}/{endRange}",
            ResponseFormat = WebMessageFormat.Json)]
        IEnumerable<DataEnvelope> GetBetween(string tag, string startRange, string endRange);
    }
}
