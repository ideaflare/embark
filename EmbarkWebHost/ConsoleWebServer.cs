using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using System.Threading.Tasks;

namespace EmbarkWebHost
{
    class ConsoleWebServer
    {
        static void Main(string[] args)
        {
            var server = new Embark.Server();
            server.Start();

            Console.WriteLine("Service running, press any key to exit...");
            Console.ReadLine();

            server.Stop();
        }
    }
}
