using Embark.Interaction;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestClient.IO;
using TestClient.IO.TestData;

namespace TestClient
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

            SimpleTDemo();

            MixedTypeDemo();
        }

        static void SimpleTDemo()
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
            var db = Embark.Client.GetLocalDB(/* Client() defaults to: C:\MyTemp\Embark\Local\ */);

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
    }
}
