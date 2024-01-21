using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CSGrock_Frontend.CSGrockLogic.Struct;
using Newtonsoft.Json;

namespace CSGrock_Frontend.CSGrockLogic.Utils
{
    internal class JSONUtil
    {
        public static string ConvertResponseToJSON(RequestResultStruct requestResultStruct)
        {
            string responseJSON = JsonConvert.SerializeObject(requestResultStruct);
            return responseJSON;
        }

        public static IncomingRequestStruct ConvertJSONToRequest(string requestJSON)
        {
            IncomingRequestStruct requestStruct = JsonConvert.DeserializeObject<IncomingRequestStruct>(requestJSON);
            return requestStruct;
        }
    }
}
