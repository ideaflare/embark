using Embark.Cache;
using Embark.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Embark
{
    public class Client
    {
        public IChannel IOChannel { get; private set; }

        public Client(IPAddress ip, int port)
        {
            throw new NotImplementedException();
        }

        public Client(string localFolder)
        {
            IOChannel = new Repository(localFolder);
        }
    }
}
