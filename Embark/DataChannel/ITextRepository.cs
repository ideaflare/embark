using System.Collections.Generic;
using System.ServiceModel;
using System.ServiceModel.Web;

namespace Embark.DataChannel
{
    [ServiceContract()]
    internal interface ITextRepository
    {
        [OperationContract,
        WebInvoke(Method = "POST",
            UriTemplate = "{tag}",
            RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json)]
        long Insert(string tag, string objectToInsert);

        [OperationContract,
        WebInvoke(Method = "PUT",
            UriTemplate = "{tag}/?id={id}&objectToUpdate={objectToUpdate}",
            ResponseFormat = WebMessageFormat.Json)]
        bool Update(string tag, long id, string objectToUpdate);

        [OperationContract,
        WebInvoke(Method = "DELETE",
            UriTemplate = "{tag}/?id={id}",
            ResponseFormat = WebMessageFormat.Json)]
        bool Delete(string tag, long id);

        [OperationContract, 
        WebGet(UriTemplate = "{tag}/?id={id}",
            ResponseFormat = WebMessageFormat.Json)]
        string Get(string tag, long id);

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
