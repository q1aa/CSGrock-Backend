using System.Net.WebSockets;

namespace CSGrock.CSGrockLogic.Struct
{
    public class WebSocketConnection
    {
        public Guid UUID { get; set; }
        public string username { get; set; }
        public WebSocket Socket { get; set; }

        public WebSocketConnection(Guid UUID, string username, WebSocket Socket)
        {
            this.UUID = UUID;
            this.username = username;
            this.Socket = Socket;
        }
    }
}
