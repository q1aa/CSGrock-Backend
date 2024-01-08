using CSGrock.CSGrockLogic.Utils.Enums;
using Newtonsoft.Json;
using CSGrock.CSGrockLogic.Struct;
using Newtonsoft.Json.Linq;

namespace CSGrock.CSGrockLogic.Utils
{
    public class JSONUtil
    {
        public static string GetRequestJSON(RequestEnum.RequestType requestMethode, string requestURL, string requestBody, Dictionary<string, string> requestHeaders, string requestID)
        {
            string headersAsJSON = JsonConvert.SerializeObject(requestHeaders);
            requestBody = requestBody.Replace("\"", "\\\"");

            return $"{{\"requestMethode\": {(int)requestMethode}, \"requestURL\": \"{requestURL}\", \"requestBody\": \"{requestBody}\", \"requestHeaders\": {headersAsJSON}, \"requestID\": \"{requestID}\"}}";
        }

        public static RequestResultStruct ConvertResponseToJSON(string response)
        {
            if(response.ToString() == "null")
            {
                return new RequestResultStruct(StorageUtil.errorOnFrontendMessage, new Dictionary<string, string>(), System.Net.HttpStatusCode.BadGateway, "null");
            }
            JObject parsedJson = JObject.Parse(response);

            string resultContent = parsedJson["resultContent"].ToString();
            JObject resultHeaders = (JObject)parsedJson["resultHeaders"];
            int resultStatusCode = (int)parsedJson["resultStatusCode"];
            string requestID = (string)parsedJson["requestID"];

            System.Net.HttpStatusCode statusCode = (System.Net.HttpStatusCode)resultStatusCode;
            Dictionary<string, string> resultHeadersDict = new Dictionary<string, string>();
            foreach (KeyValuePair<string, JToken> header in resultHeaders)
            {
                resultHeadersDict.Add(header.Key, header.Value.ToString());
            }

            return new RequestResultStruct(resultContent, resultHeadersDict, statusCode, requestID);
        }
    }
}
