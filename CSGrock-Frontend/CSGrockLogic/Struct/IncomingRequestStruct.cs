using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CSGrock_Frontend.CSGrockLogic.Utils.Enums;

namespace CSGrock_Frontend.CSGrockLogic.Struct
{
    internal class IncomingRequestStruct
    {
        public string requestBody { get; set; }

        public Dictionary<string, string> requestHeaders { get; set; }

        public RequestMethodeEnum.RequestMethode requestMethode { get; set; }
        public string requestURL { get; set; }
        public string requestID { get; set; }

        public IncomingRequestStruct(string requestBody, Dictionary<string, string> requestHeaders, RequestMethodeEnum.RequestMethode requestMethode, string requestURL, string requestID)
        {
            this.requestBody = requestBody;
            this.requestHeaders = requestHeaders;
            this.requestMethode = requestMethode;
            this.requestURL = requestURL;
            this.requestID = requestID;
        }
    }
}
