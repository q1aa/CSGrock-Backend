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

        public static async Task ReceiveMessage(WebSocketConnection socketConnection)
        {
            var buffer = new byte[1024 * 20];

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

                        StorageUtil.app.Logger.LogInformation($"Request with id {resultStruct.requestID} has been completed with status code {resultStruct.resultStatusCode}");
                        StorageUtil.allRequestResults.Add(resultStruct);
                        continue;
                    }
                    else if(message.StartsWith($"Receaving parts "))
                    {
                        string requestResult = message.Replace($"Receaving parts ", "");
                        /*{"partID":709,"partContent":"ruj0ztis4aT8NVmgjQXILWDiJjT8vI3+1Vrt9RbBBAc4xSeeryi/aWm4Z+s4rbZdNtWf0VcdVMqO6AQXrJJbbvlFh3y3XHZQVX2m","partsLenght":1956,"requestID":"e52cfd65-b933-4863-a661-875dec98fbd3"}*/

                        ImagePartStruct imagePart = JSONUtil.ConvertImagePartToJSON(requestResult);
                        if (!StorageUtil.partsList.ContainsKey(imagePart.requestID))
                        {
                            StorageUtil.partsList.Add(imagePart.requestID, imagePart.partContent);
                        }
                        else
                        {
                            StorageUtil.partsList[imagePart.requestID] += imagePart.partContent;
                        }

                        //StorageUtil.app.Logger.LogInformation($"Receaved part {imagePart.partID} of request with id {imagePart.requestID} out of {imagePart.partsLenght}");

                        if (imagePart.partsLenght == imagePart.partID)
                        {
                            StorageUtil.app.Logger.LogInformation($"All parts of request with id {imagePart.requestID} has been received");
                            string base64 = StorageUtil.partsList[imagePart.requestID];

                            //Need to implement filename!!!!!!!!
                            string filePath = Path.Combine(Directory.GetCurrentDirectory(), @$"UserContent/{imagePart.requestID}");
                            await FileUtil.WriteByteToFile(filePath, Convert.FromBase64String(base64), $"{imagePart.requestID}{imagePart.fileType}");

                            var resultStruct = new RequestResultStruct("Lookup-dir", new Dictionary<string, string>(), System.Net.HttpStatusCode.Gone, imagePart.requestID);
                            StorageUtil.app.Logger.LogInformation($"Request with id {resultStruct.requestID} has been completed with status code {resultStruct.resultStatusCode}");
                            StorageUtil.allRequestResults.Add(resultStruct);

                            StorageUtil.partsList.Remove(imagePart.requestID);
                            continue;
                        }

                        /*string filePath = Path.Combine(Directory.GetCurrentDirectory(), @"UserContent/efaeeb49-e99f-4112-9f90-ab3fc90d596d/Z.png");
                            StorageUtil.app.Logger.LogInformation($"Writing file to {filePath}");
                            FileUtil.WriteByteToFile(filePath, Convert.FromBase64String(base64));

                            var resultStruct = new RequestResultStruct("Lookup-dir", new Dictionary<string, string>(), System.Net.HttpStatusCode.Gone, requestID);
                            StorageUtil.app.Logger.LogInformation($"Request with id {resultStruct.requestID} has been completed with status code {resultStruct.resultStatusCode}");
                            StorageUtil.allRequestResults.Add(resultStruct);*/
                        continue;
                    }

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
