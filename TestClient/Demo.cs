using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestClient.IO;

namespace TestClient
{
    /// <summary>
    /// Simple code example to demonstrate how to use embark
    /// </summary>
    class Demo
    {
        void SimpleDemo()
        {
            // arrange some guinea pig
            var pet = new Sheep { Name = "Fluffy", FavouriteIceCream = IceCream.Vanilla };
            
            // save data locally
            var db = new Embark.Client(@"C:\MyTemp\Embark");
            
            // or over a network
            //var io = new Embark.Client("127.0.0.1", 80);// Not implemented, yet..

            // reference to a collection
            var io = db["sheep"];

            // insert
            long id = io.Insert(pet);
            // or long id = db["sheep"].Insert(pet);

            // get
            Sheep fluffy = io.Select<Sheep>(id);

            // update
            fluffy.FavouriteIceCream = IceCream.Strawberry;            
            bool hasSheepUpdated = io.Update(id, fluffy);

            // delete
            bool hasSheepVanished = io.Delete(id);
        }

        void SearchDemo()
        {
            // arrange some guinea pig
            var tag = "sheep";
            var pet = new Sheep { Name = "Fluffy", FavouriteIceCream = IceCream.Vanilla };
            
            // save data locally
            var db = new Embark.Client();

            var io = db["sheep"];

            var itag = db[tag];

            io.Delete(15);

            itag.Select<Sheep>(53);

            //itag.SelectLike<Cat>(new { Name = "Miaau" });
            itag.SelectLike<Cat>(15);

            db["cat"].Select<Cat>(31);

            itag.UpdateBetween(new { FurDensity = 0.7, Name = "A" },
                new { FurDensity = 0.6, Name = "B" },
                new { Name = "Mass" });
        }
    }
}
