using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace TestClient.TestData.Basic
{
    public class Cat
    {
        public string Name { get; set; }
        public int Age { get; set; }

        public string Tale { get; set; } //pun intended ;)
        public double FurDensity { get; set; }
        public bool HasMeme { get; set; }
    }
}
