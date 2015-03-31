using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using System.Threading.Tasks;

namespace Embark.Interfaces
{
    /// <summary>
    /// All commands to insert/get/update/delete documents
    /// </summary>
    [ServiceContract()]
    public interface ITextDataStore
    {
        // Basic

        /// <summary>
        /// Insert a json serializable object to save to a collection
        /// </summary>
        /// <typeparam name="T">Any json serializable/deserializable object</typeparam>
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
        string Select(string tag, string id);

        // Like
        [OperationContract,
        WebGet(UriTemplate = "{tag}/Like/{searchObject}",
            ResponseFormat = WebMessageFormat.Json)]
        IEnumerable<string> SelectLike(string tag, string searchObject);

        // Between
        [OperationContract,
        WebGet(UriTemplate = "{tag}/Between/{startRange}/{endRange}",
            ResponseFormat = WebMessageFormat.Json)]
        IEnumerable<string> SelectBetween(string tag, string startRange, string endRange);
    }
}
