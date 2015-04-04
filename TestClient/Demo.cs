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

            SimpleDemo();

            Embark.Conversion.Collection db = Embark.Client.GetLocalDB().Basic;
                       
            object box = 235;

            long id = db.Insert(box);

            var n = db.Select<object>(id);

            var search = db.SelectLike<object>("235").ToList();

        }

        static void SimpleDemo()
        {
            // arrange some guinea pig
            var pet = new Sheep { Name = "Fluffy", FavouriteIceCream = IceCream.Vanilla };

            // save data locally
            var db = Embark.Client.GetLocalDB(@"C:\AnimalsDB\"); /* Client.GetLocalDB() defaults to: C:\MyTemp\Embark\Local\ */

            // or over a network (via REST API)
            //var db = Embark.Client.GetNetworkDB("127.0.0.1", 8080);// Not implemented, yet..

            // collections created on-the-fly if needed
            var io = db["sheep"];

            // insert
            long id = io.Insert(pet);

            // get
            Sheep fluffy = io.Select<Sheep>(id);

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
            Sheep fluffy = io.Select<Sheep>(id);

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
