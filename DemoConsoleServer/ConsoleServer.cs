using System;

namespace DemoWebServer
{
    class ConsoleServer
    {
        static void Main(string[] args)
        {
            Console.WriteLine("This is a VERY basic Embark server.");
            Console.WriteLine("No fancy logging/feedback or UI.. yet!\r\n");

            var server = new Embark.Server(@"C:\MyTemp\Embark\Server");
            try
            {
                server.Start();
                Console.WriteLine("For any feedback & suggestions and to get involved\r\nfeel free to mail EmbarkDB@gmail.com :)\r\n");
                Console.WriteLine("Service running, press any key to exit...");
                Console.ReadLine();
                server.Stop();
            }
            catch (System.ServiceModel.AddressAccessDeniedException ae)
            {
                Console.WriteLine("An AddressAccessDeniedException occurred:");

                Console.WriteLine("Either run the server in admin mode or allow your server app to use the your-machine:port/embark/ uri" + "\r\n" +
                     "see the usage details on https://github.com/ubrgw/embark\r\n");

                Console.WriteLine("Press any key to see error details:\r\n");
                Console.ReadLine();

                Console.WriteLine("Error message:\r\n\r\n" + ae.ToString());
                Console.ReadLine();
            }
        }
    }
}
