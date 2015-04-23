using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace TestClient.IO.TestData
{
    internal class TestEntities
    {
        internal static Sheep GetTestSheep()
        {
            var r = rnd.Value;
            return new Sheep
            {
                Name = petNames[r.Next(0, petNames.Length)],
                Age = r.Next(1, 15),
                FavouriteIceCream = (IceCream)r.Next(0, 4)
            };
        }

        internal static List<Sheep> GetTestHerd(int count = 10)
        {
            return Enumerable.Range(0, count)
                .Select(i => GetTestSheep())
                .ToList();
        }

        static ThreadLocal<Random> rnd = new ThreadLocal<Random>(() => new Random());
        static string[] petNames = new string[] { "Candyfloss", "Dimples", " Fluffy", " Jingles", " Monkey", " Rambo", " Snowball", " Tumble", " Zebra" };
    }

    public class Sheep : IEquatable<Sheep>
    {
        public string Name { get; set; }
        public int Age { get; set; }
        public IceCream FavouriteIceCream { get; set; }

        public Table OnTable { get; set; }

        public override string ToString()
        {
            return string.Format("Name[{0}] Age[{1}] FavouriteIceCream[{2}]", Name, Age, FavouriteIceCream.ToString());
        }

        public bool Equals(Sheep other)
        {
            return this.Name == other.Name &&
                this.Age == other.Age &&
                this.FavouriteIceCream == other.FavouriteIceCream;
        }

        public override bool Equals(object obj)
        {
            Sheep other = obj as Sheep;
            if (other == null)
                return false;
            else return Equals(other);
        }

        public override int GetHashCode()
        {
            string name = Name ?? "";
            return name.GetHashCode() * Age.GetHashCode() * FavouriteIceCream.GetHashCode();
        }
    }

    public class Table
    {
        public int Legs { get; set; }
        public bool IsSquare { get; set; }
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
