using System;
using System.Collections.Generic;
using System.IO;
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

        internal List<Sheep> GetTestHerd(int count = 10)
        {
            var r = new Random();
            return Enumerable.Range(0, count)
                .Select(i => new Sheep(
                    name: Path.GetRandomFileName().Replace(".", ""),
                    age: r.Next(1, 15),
                    favouriteIceCream: (IceCream)r.Next(0, 4)
                    ))
                .ToList();
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

    public class Cat
    {
        public string Name { get; set; }
        public int Age { get; set; }
        public double FurDensity { get; set; }
        public bool HasMeme { get; set; }
    }

    public enum IceCream
    {
        Bubblegum,
        Chocolate,
        Strawberry,
        Vanilla
    }
}
