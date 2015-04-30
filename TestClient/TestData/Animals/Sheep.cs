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
}
