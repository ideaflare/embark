using Embark;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemoWebClient
{
    class Program
    {
        static void Main(string[] args)
        {
            var server = new Server(port:8000);
            server.Start();

            var thisPc = System.Net.Dns.GetHostName();
            var db = Embark.Client.GetNetworkDB(thisPc);

            var id = db.Basic.Insert(new { Name = "Yana" });

            //var allDocs = db.Generic.s

            Console.Write("server running, press any key to stop");
            Console.Read();

            server.Stop();
        }
    }
}
