using Embark.Interaction;
using EmbarkTests._Mocks;
using System;
using System.IO;
using System.Linq;

namespace DemoConsoleClient
{
    /// <summary>
    /// Simple code example to demonstrate how to use embark
    /// </summary>
    class Demo
    {
        public static void Main()
        {
            //var defaultDir = Directory.GetCurrentDirectory() + "\\Collections\\";
            //var defaultDir = @"C:\MyTemp\Embark\Local\Collections\";
            var defaultDir = @"C:\AnimalsDB\Collections\";

            if (Directory.Exists(defaultDir))
                Directory.Delete(defaultDir, recursive: true);

            //GenericDemo();

            //BasicDemo();

            //MixedTypeDemo();

            WebServerDemo();

        }

        private static void GenericDemo()
        {
            // arrange some guinea pig
            var pet = new Sheep { Name = "Fluffy", FavouriteIceCream = IceCream.Vanilla };

            // save data locally
            var db = Embark.Client.GetLocalDB(@"C:\AnimalsDB\");

            // or over a network via REST API to WCF server *see usage section below*
            //var db = Embark.Client.GetNetworkDB("192.168.1.24", 8080);

            // collections created on-the-fly if needed
            var io = db.GetCollection<Sheep>("sheep");

            // insert
            long id = io.Insert(pet);

            // get
            Sheep fluffy = io.Get(id);

            // update
            fluffy.FavouriteIceCream = IceCream.Strawberry;
            bool fluffyNowLikesStrawberry = io.Update(id, fluffy);

            // delete
            bool hasSheepVanished = io.Delete(id);

            // non-type specific collection if you want to save Apples & Oranges in the same fruit collection
            //var io = db["fruit"];
        }

        static void BasicDemo()
        {
            // arrange some guinea pig
            var pet = new Sheep { Name = "Fluffy", FavouriteIceCream = IceCream.Vanilla };

            // save data locally
            var db = Embark.Client.GetLocalDB(@"C:\AnimalsDB\");

            // or over a network via REST API to WCF server *see usage section below*
            //var db = Embark.Client.GetNetworkDB("192.168.1.24", 8080);

            // collections created on-the-fly if needed
            var io = db.GetCollection<Sheep>("sheep");

            // insert
            long id = io.Insert(pet);

            // get
            Sheep fluffy = io.Get(id);

            // update
            fluffy.FavouriteIceCream = IceCream.Strawberry;
            bool fluffyNowLikesStrawberry = io.Update(id, fluffy);

            // delete
            bool hasSheepVanished = io.Delete(id);

            // non-type specific collection if you want to save Apples & Oranges in the same fruit collection
            //var io = db["fruit"];
        }

        static void MixedTypeDemo()
        {
            // arrange some guinea pigs
            var pet = new Sheep { Name = "Fluffy", FavouriteIceCream = IceCream.Vanilla };
            var mittens = new Cat { Name = "Mittens", FurDensity = 0.1 };

            // save data locally
            var db = Embark.Client.GetLocalDB(@"C:\AnimalsDB\"); /* Client.GetLocalDB() defaults to: C:\MyTemp\Embark\Local\ */

            // or over a network (via REST API)
            //var db = Embark.Client.GetNetworkDB("127.0.0.1", 8080);// Not implemented, yet..

            // collections created on-the-fly if needed
            var io = db["sheep"];

            // insert
            long id = io.Insert(pet);
            long d2 = io.Insert((object)500);
            long d3 = io.Insert("a string!");
            long d4 = io.Insert<Cat>(mittens);

            // get
            Sheep fluffy = io.Get<Sheep>(id);

            var xs = (int)io.Get<object>(d2);
            var ss = io.Get<string>(d3);
            //Cat indy = io.Get<Cat>(d2);
            //var x = io.Get<Object>(d3);
            //string empty = io.Get<string>(d4);

            DocumentWrapper<Sheep> fluffbox = io.GetWrapper<Sheep>(id);

            // update
            fluffy.FavouriteIceCream = IceCream.Strawberry;
            bool fluffyNowLikesStrawberry = io.Update(id, fluffy);

            // delete
            bool hasSheepVanished = io.Delete(id);
        }

        static void SearchDemo()
        {
            // arrange some guinea pig
            var pet = new Sheep { Name = "Fluffy", FavouriteIceCream = IceCream.Vanilla };

            // save data locally
            var db = Embark.Client.GetLocalDB(@"C:\AnimalsDB\"); /* Client.GetLocalDB() defaults to: C:\MyTemp\Embark\Local\ */
            
            // or over a network
            // var io = Embark.Client.GetNetworkDB("127.0.0.1", 8765);// Not implemented, yet..

            // reference collections when used a lot so you don't have to keep typing them out
            var io = db["sheep"];

            // insert
            long id = io.Insert(pet);

            // get
            Sheep fluffy = io.Get<Sheep>(id);

            // update
            fluffy.FavouriteIceCream = IceCream.Strawberry;
            bool fluffyNowLikesStrawberry = io.Update(id, fluffy);

            // delete
            bool hasSheepVanished = io.Delete(id);

            //io.UpdateBetween(new { FurDensity = 0.7, Name = "A" },
            //    new { FurDensity = 0.6, Name = "B" },
            //    new { Name = "Mass" });
        }

        static void WebServerDemo()
        {
            var server = new Embark.Server(@"C:\MyTemp\EmbarkDemo\AnimalsDB\");
            server.Start();

            var thisPc = System.Net.Dns.GetHostName();
            var db = Embark.Client.GetNetworkDB(thisPc);

            for (int i = 0; i < 2; i++)
            {
                db["cats"].Insert(new
                {
                    name = "Cat number " + i,
                    Age = (i % 20) + 1,
                    sheep = Enumerable.Range(0, 100)
                                      .Select(r => new Sheep { Age = i + r, Name = "uhetnohtnu", OnTable = new Table { IsSquare = true } })
                                      .ToList()
                });
            }

            var allDocs = db["cats"].GetAll<object>().ToList();

            Console.Write("server running, press any key to stop");
            Console.Read();

            server.Stop();
        }
    }
}
