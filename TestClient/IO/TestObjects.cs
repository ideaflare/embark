using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestClient.IO
{
    internal class TestObjects
    {
        internal static Sheep GetTestSheep()
        {
            return new Sheep("Candyfloss", 12, IceCream.Vanilla);
        }
    }

    public class Sheep
    {
        public Sheep(string name, int age, IceCream favouriteIceCream)
        {
            this.Name = name;
            this.Age = age;
            this.FavouriteIceCream = favouriteIceCream;
        }

        public string Name { get; set; }
        public int Age { get; set; }
        public IceCream FavouriteIceCream { get; set; }
    }

    public enum IceCream
    {
        Bubblegum,
        Chocolate,
        Strawberry,
        Vanilla
    }
}
