using CSGrock.CSGrockLogic.Struct;
using CSGrock.CSGrockLogic.Utils;
using CSGrock.CSGrockLogic.Utils.Enums;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics.Eventing.Reader;
using System.Net.WebSockets;
using System.Text;

namespace CSGrock.CSGrockLogic.Socket
{
    public class SocketHandler
    {
        public static Task SendMessage(WebSocketConnection socketConnection, string message)
        {
            if (socketConnection.Socket.State != WebSocketState.Open) return Task.CompletedTask;

            var bytes = Encoding.ASCII.GetBytes(message);
            var buffer = new ArraySegment<byte>(bytes);
            
            return socketConnection.Socket.SendAsync(buffer, WebSocketMessageType.Text, true, CancellationToken.None);
        }

        public static Task HandleRequestForwarder(WebSocketConnection socketConnection, RequestEnum.RequestType requestMethode, string path, string requestBody, string[] headers, string requestID)
        {
            Dictionary<string, string> newHeaders = new Dictionary<string, string>();
            foreach (string header in headers)
            {
                string[] headerSplit = header.Split(":");
                newHeaders.Add(headerSplit[0], headerSplit[1]);
            }
            string requestJSON = JSONUtil.GetRequestJSON(requestMethode, path, requestBody, newHeaders, requestID);
            return SendMessage(socketConnection, requestJSON);
        }

        static string base64 = string.Empty;

        public static async Task ReceiveMessage(WebSocketConnection socketConnection)
        {
            var buffer = new byte[1024 * 4];

            while (socketConnection.Socket.State == WebSocketState.Open)
            {
                var result = await socketConnection.Socket.ReceiveAsync(buffer: new ArraySegment<byte>(buffer),
                                                                                             cancellationToken: CancellationToken.None);

                if (result.MessageType == WebSocketMessageType.Text)
                {
                    string message = Encoding.UTF8.GetString(buffer, 0, result.Count);

                    if(message.StartsWith($"Receaving from "))
                    {
                        string requestResult = message.Replace($"Receaving from ", "");
                        var resultStruct = JSONUtil.ConvertResponseToJSON(requestResult);
                        /*if(resultStruct.resultContent == StorageUtil.errorOnFrontendMessage)
                        {
                            StorageUtil.app.Logger.LogInformation("result struct is error message");
                            continue;
                        }*/


                        StorageUtil.app.Logger.LogInformation($"Request with id {resultStruct.requestID} has been completed with status code {resultStruct.resultStatusCode}");
                        StorageUtil.allRequestResults.Add(resultStruct);
                        continue;
                    }
                    /*else if(message.StartsWith($"Receaving parts "))
                    {
                        string requestResult = message.Replace($"Receaving parts ", "");
                        *{"partID":709,"partContent":"ruj0ztis4aT8NVmgjQXILWDiJjT8vI3+1Vrt9RbBBAc4xSeeryi/aWm4Z+s4rbZdNtWf0VcdVMqO6AQXrJJbbvlFh3y3XHZQVX2m","partsLenght":1956,"requestID":"e52cfd65-b933-4863-a661-875dec98fbd3"}
                        var data = message.Split(",");
                        string base64Part = data[1].Replace("\"partContent\":\"", "").Replace("\"", "");
                        base64 += base64Part;
                        if (requestResult.Contains("\"partID\":1956"))
                        {
                            var requestID = data[3].Replace("\"requestID\":\"", "").Replace("\"", "").Replace("}", "");

                            var resultStruct = new RequestResultStruct(base64, new Dictionary<string, string>(), System.Net.HttpStatusCode.OK, requestID);
                            StorageUtil.app.Logger.LogInformation($"Request with id {resultStruct.requestID} has been completed with status code {resultStruct.resultStatusCode}");
                            StorageUtil.allRequestResults.Add(resultStruct);
                        }
                        continue;
                    }*/

                    await SendMessage(socketConnection, message);
                }
                else if (result.MessageType == WebSocketMessageType.Close)
                {
                    await socketConnection.Socket.CloseAsync(WebSocketCloseStatus.NormalClosure, "", CancellationToken.None);
                    StorageUtil.app.Logger.LogInformation($"Socket with UUID {socketConnection.UUID} closed");
                }
            }
            StorageUtil.app.Logger.LogInformation($"Socket with UUID {socketConnection.UUID} closed");
            StorageUtil.allSockettConnections.Remove(socketConnection);
        }
    }
}
