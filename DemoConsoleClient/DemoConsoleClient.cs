using Embark;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemoWebClient
{
    class DemoConsoleClient
    {
        static void Main(string[] args)
        {
            var server = new Server();
            server.Start();

            var thisPc = System.Net.Dns.GetHostName();
            var db = Embark.Client.GetNetworkDB(thisPc);

            var id = db.Basic.Insert(new { Name = "Yana" });

            var allDocs = db.Basic.GetAll<Object>();

            Console.Write("server running, press any key to stop");
            Console.Read();

            server.Stop();
        }
    }
}
