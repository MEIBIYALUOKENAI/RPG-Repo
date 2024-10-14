using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace SimpleServer.Net
{
    public class ClientSocket
    {
        public Socket socket { get; set; }
        public long LastPingTime { get; set; }
        public ByteArray byteArray=new ByteArray();
    }
}
