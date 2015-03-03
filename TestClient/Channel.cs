using Embark;
using Embark.Interfaces;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace TestClient
{
    internal class Channel
    {
        internal static IChannel localCache;
        internal static IChannel serverCache = null;
        
        static Channel()
        {
            var testDir = @"C:\MyTemp\Embark\TestData\";

            Directory.CreateDirectory(testDir);

            localCache = new Client(testDir);

            //serverCache = new Client("127.0.0.1", 80);
        }
    }
}