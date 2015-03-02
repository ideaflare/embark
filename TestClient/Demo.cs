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
            var tag = "sheep";
            
            // save data locally
            var client = new Embark.Client(@"C:\MyTemp\Embark");

            // or over a network
            // var client = new Embark.Client("127.0.0.1"), 80);// Not implemented, yet..

            var io = client.IOChannel;

            // insert
            long id = io.Insert(tag, pet);

            // get
            Sheep fluffy = io.Get<Sheep>(tag, id);

            // update
            fluffy.FavouriteIceCream = IceCream.Strawberry;
            
            bool hasSheepUpdated = io.Update(tag, id, fluffy);

            // delete
            bool hasSheepVanished = io.Delete(tag, id);
        }
    }
}
