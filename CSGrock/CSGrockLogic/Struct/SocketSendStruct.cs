using CSGrock.CSGrockLogic.Utils.Enums;

namespace CSGrock.CSGrockLogic.Struct
{
    public class SocketSendStruct
    {
        public RequestEnum.RequestType requestType { get; set; }
        public string url { get; set; }
        public string body { get; set; }
        public string header { get; set; }

        public SocketSendStruct(RequestEnum.RequestType requestType, string url, string body, string header)
        {
            this.requestType = requestType;
            this.url = url;
            this.body = body;
            this.header = header;
        }
    }
}
