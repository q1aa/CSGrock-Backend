using CSGrock_Frontend.CSGrockLogic.Utils.Enums;
using System.Collections.Generic;

namespace CSGrock_Frontend.CSGrockLogic.Struct
{
    public class RequestResultStruct
    {
        public string resultContent { get; set; }
        public Dictionary<string, string> resultHeaders { get; set; }
        public System.Net.HttpStatusCode resultStatusCode { get; set; }

        public string requestID { get; set; }

        public RequestResultStruct(string resultContent, Dictionary<string, string> resultHeaders, System.Net.HttpStatusCode resultStatusCode, string requestID)
        {
            this.resultContent = resultContent;
            this.resultHeaders = resultHeaders;
            this.resultStatusCode = resultStatusCode;
            this.requestID = requestID;
        }
    }
}
