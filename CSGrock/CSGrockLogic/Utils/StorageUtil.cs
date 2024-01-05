using CSGrock.CSGrockLogic.Struct;
using System.Net.WebSockets;

namespace CSGrock.CSGrockLogic.Utils
{
    public class StorageUtil
    {
        //create a new variable called app but the type is unknown
        public static WebApplication app;
        public static WebSocket ws;

        public static List<WebSocketConnection> allSockettConnections = new List<WebSocketConnection>();

        public static List<RequestResultStruct> allRequestResults = new List<RequestResultStruct>();
    }
}
