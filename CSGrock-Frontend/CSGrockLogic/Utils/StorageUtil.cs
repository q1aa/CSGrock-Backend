using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Threading.Tasks;

namespace CSGrock_Frontend.CSGrockLogic.Utils
{
    internal class StorageUtil
    {
        public static ClientWebSocket webSocket = null;

        public static string BackendURL = "wss://localhost:7006/ws";

        public static int ForwardPort = 0;
    }
}
