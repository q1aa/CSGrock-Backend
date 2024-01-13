using CSGrock.CSGrockLogic.Struct;

namespace CSGrock.CSGrockLogic.Utils
{
    public class HelperUtil
    {
        //uuid 61dcf4a6-3600-45b5-8e2a-289d5a45aff7
        public static bool isValidUUID(string uuid)
        {
            return Guid.TryParse(uuid, out _);
        }

        public static async Task<bool> isUUIDInList(string uuid)
        {
            foreach (WebSocketConnection socketConnection in StorageUtil.allSockettConnections)
            {
                if (socketConnection.UUID.ToString() == uuid)
                {
                    return true;
                }
            }
            return false;
        }

        public static async Task CheckForRequestResult(string requestID, int timeout, int repeat, Action<RequestResultStruct> handleMessage)
        {
            int i = 0;
            while (i < repeat)
            {
                //reverse for loop trough all request results
                for(int j = StorageUtil.allRequestResults.Count - 1; j >= 0; j--)
                {
                    RequestResultStruct requestResult = StorageUtil.allRequestResults[j];
                    if (requestResult.requestID == requestID)
                    {
                        handleMessage(requestResult);
                        return;
                    }
                }
                await Task.Delay(timeout);
                i++;
            }

            handleMessage(new RequestResultStruct(StorageUtil.errorOnFrontendMessage, new Dictionary<string, string>(), System.Net.HttpStatusCode.BadGateway, requestID));
        }
    }
}
