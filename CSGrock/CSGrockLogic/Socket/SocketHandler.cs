﻿using CSGrock.CSGrockLogic.Struct;
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
        /*public static Task SendMessage(WebSocket socket, string message)
        {
            if (socket.State != WebSocketState.Open) return Task.CompletedTask;

            var bytes = Encoding.ASCII.GetBytes(message);
            var buffer = new ArraySegment<byte>(bytes);
            return socket.SendAsync(buffer, WebSocketMessageType.Text, true, CancellationToken.None);
        }

        public static async Task ReceiveMessage(WebSocket socket, Action<string> handleMessage)
        {
            var buffer = new byte[1024 * 4];

            while (socket.State == WebSocketState.Open)
            {
                var result = await socket.ReceiveAsync(buffer: new ArraySegment<byte>(buffer),
                                                                          cancellationToken: CancellationToken.None);

                if (result.MessageType == WebSocketMessageType.Text)
                {
                    handleMessage(Encoding.UTF8.GetString(buffer, 0, result.Count));
                }
                else if (result.MessageType == WebSocketMessageType.Close)
                {
                    await socket.CloseAsync(WebSocketCloseStatus.NormalClosure, "", CancellationToken.None);
                }
            }
        }*/

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

            StorageUtil.app.Logger.LogInformation($"Sending request with UUID {requestID}");

            string requestJSON = JSONUtil.GetRequestJSON(requestMethode, path, requestBody, newHeaders, requestID);
            return SendMessage(socketConnection, requestJSON);
        }

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

                    //if(message.StartsWith($"Receaving from {socketConnection.UUID}"))
                    if(message.StartsWith($"Receaving from "))
                    {
                        //string requestResult = message.Replace($"Receaving from {socketConnection.UUID}:", "");
                        string requestResult = message.Replace($"Receaving from ", "");
                        Console.WriteLine(requestResult);

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

                    await SendMessage(socketConnection, message);
                    //StorageUtil.app.Logger.LogInformation($"Message received: {message} from {socketConnection.UUID}");
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
