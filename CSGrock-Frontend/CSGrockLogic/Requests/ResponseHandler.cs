using CSGrock_Frontend.CSGrockLogic.Struct;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace CSGrock_Frontend.CSGrockLogic.Requests
{
    internal class ResponseHandler
    {
        public static async Task<RequestResultStruct> HandleResponseAsync(HttpResponseMessage response, string requestID)
        {
            Dictionary<string, string> responseHeaders = new Dictionary<string, string>();
            foreach (var header in response.Headers)
            {
                if (header.ToString().Contains(":")) continue;
                if (header.ToString().Split(':').Length < 2) continue;

                string headerKey = header.ToString().Split(':')[0];
                string headerValue = header.ToString().Split(':')[1];
                responseHeaders.Add(headerKey, headerValue);
            }

            string responseContent = await response.Content.ReadAsStringAsync();
            return new RequestResultStruct(responseContent, responseHeaders, response.StatusCode, requestID);
        }
    }
}
