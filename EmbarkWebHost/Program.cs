using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using System.Threading.Tasks;

namespace EmbarkWebHost
{
    class Program
    {
        static void Main(string[] args)
        {
            Uri url = new Uri("http://localhost:8000/ola/");
            using(var wh = new WebServiceHost(typeof(Test),url))
            {
                wh.Open();
                Console.WriteLine("Service running, press any key to exit...");
                Console.ReadLine();
                wh.Close();
            }
        }
    }

    [ServiceContract]
    public interface ITest
    {
        [OperationContract, WebGet(UriTemplate = "hi/{message}", ResponseFormat = WebMessageFormat.Json)]
        string GetHello(string message);
    }

    public class Test : ITest
    {
        public string GetHello(string message)
        {
            return string.Join(" ", message.ToArray());
        }
    }
}
