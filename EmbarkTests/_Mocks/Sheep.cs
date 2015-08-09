using System;
using System.Collections.Generic;
using System.Linq;

namespace EmbarkTests._Mocks
{
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

        internal static Sheep GetTestSheep()
        {
            return new Sheep
            {
                Name = RandomData.GetRandomString(),
                Age = RandomData.Random.Next(1,7),
                FavouriteIceCream = (IceCream) RandomData.Random.Next(4)
            };
        }

        internal static List<Sheep> GetTestHerd(int count = 10)
        {
            return Enumerable.Range(0, count)
                .Select(i => GetTestSheep())
                .ToList();
        }

        public bool Equals(Sheep other)
        {
            return Name == other.Name &&
                Age == other.Age &&
                FavouriteIceCream == other.FavouriteIceCream;
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
}
