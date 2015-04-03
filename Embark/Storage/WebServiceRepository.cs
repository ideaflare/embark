using Embark.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.ServiceModel;
using System.ServiceModel.Description;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;

namespace Embark.Storage
{
    public class WebServiceRepository : ITextDataStore
    {
        public WebServiceRepository(Uri serviceUri)
        {
            this.serviceAbsoluteUri = serviceUri.AbsoluteUri;
        }

        private T CallRemoteDatastore<T>(Func<ITextDataStore,T> func)
        {
            using (ChannelFactory<ITextDataStore> cf = new ChannelFactory<ITextDataStore>(new WebHttpBinding(), this.serviceAbsoluteUri))
            {
                cf.Endpoint.Behaviors.Add(new WebHttpBehavior());
                var webChannel = cf.CreateChannel();
                return func(webChannel);
            }            
        }

        private string serviceAbsoluteUri;
        private JavaScriptSerializer serializer = new JavaScriptSerializer();

        long ITextDataStore.Insert(string tag, string objectToInsert)
        { 
            return CallRemoteDatastore<long>((store) => store.Insert(tag, objectToInsert));
        }

        bool ITextDataStore.Update(string tag, string id, string objectToUpdate)
        {
            return CallRemoteDatastore<bool>((store) => store.Update(tag, id, objectToUpdate));
        }

        bool ITextDataStore.Delete(string tag, string id)
        {
            return CallRemoteDatastore<bool>((store) => store.Delete(tag, id));
        }

        string ITextDataStore.Select(string tag, string id)
        {
            return CallRemoteDatastore<string>((store) => store.Select(tag, id));
        }

        IEnumerable<string> ITextDataStore.SelectAll(string tag)
        {
            return CallRemoteDatastore<IEnumerable<string>>((store) => store.SelectAll(tag));
        }

        IEnumerable<string> ITextDataStore.SelectLike(string tag, string searchObject)
        {
            return CallRemoteDatastore<IEnumerable<string>>((store) => store.SelectLike(tag, searchObject));
        }

        IEnumerable<string> ITextDataStore.SelectBetween(string tag, string startRange, string endRange)
        {
            return CallRemoteDatastore<IEnumerable<string>>((store) => store.SelectBetween(tag, startRange, endRange));
        }
        
        //private HttpClient GetClient()
        //{
        //    var client = new HttpClient();

        //    client.BaseAddress = serviceUri;
        //    client.DefaultRequestHeaders.Accept.Clear();
        //    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

        //    //// HTTP GET
        //    //HttpResponseMessage response = await client.GetAsync("api/products/1");
        //    //if (response.IsSuccessStatusCode)
        //    //{
        //    //    Product product = await response.Content.ReadAsAsync<Product>();
        //    //    Console.WriteLine("{0}\t${1}\t{2}", product.Name, product.Price, product.Category);
        //    //}

        //    //// HTTP POST
        //    //var gizmo = new Product() { Name = "Gizmo", Price = 100, Category = "Widget" };
        //    //response = await client.PostAsJsonAsync("api/products", gizmo);

        //    //if (response.IsSuccessStatusCode)
        //    //{
        //    //    Uri gizmoUrl = response.Headers.Location;

        //    //    // HTTP PUT
        //    //    gizmo.Price = 80;   // Update price
        //    //    response = await client.PutAsJsonAsync(gizmoUrl, gizmo);

        //    //    // HTTP DELETE
        //    //    response = await client.DeleteAsync(gizmoUrl);
        //    //}

        //    return client;
        //}


        
    }
}
