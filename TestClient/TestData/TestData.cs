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

namespace TestClient.TestData
{
    [TestClass]
    public class Cache
    {
        internal static Client localCache;
        internal static Client serverCache = null;
        internal static IDataStore localSheep;

        [AssemblyInitialize()]
        public static void MyTestInitialize(TestContext testContext)
        {
            var testDir = @"C:\MyTemp\Embark\TestData\";

            Directory.CreateDirectory(testDir);

            //localCache = new Client(testDir);
            localCache = Client.GetLocalDB(testDir);

            localSheep = localCache["sheep"];
            //serverCache = new Client("127.0.0.1", 80);
        }
    }
}