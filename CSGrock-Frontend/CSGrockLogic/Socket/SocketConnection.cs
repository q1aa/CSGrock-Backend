using CSGrock_Frontend.CSGrockLogic.Requests;
using CSGrock_Frontend.CSGrockLogic.Struct;
using CSGrock_Frontend.CSGrockLogic.Utils;
using System;
using System.Collections.Generic;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CSGrock_Frontend.CSGrockLogic.Socket
{
    internal class SocketConnection
    {
        public static async Task ConnectToSocket()
        {
            try
            {
                byte[] buffer = new byte[1056];

                StorageUtil.webSocket = new ClientWebSocket();
                await StorageUtil.webSocket.ConnectAsync(new Uri(StorageUtil.BackendURL), CancellationToken.None);

                byte[] buf = new byte[1056];

                while (StorageUtil.webSocket.State == WebSocketState.Open)
                {
                    var arraySegment = new ArraySegment<byte>(buf);
                    var result = await StorageUtil.webSocket.ReceiveAsync(arraySegment, CancellationToken.None);

                    await HandleMessageReceive(arraySegment, result, buffer);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("ERROR: " + ex.ToString());
            }

        }

        private static async Task HandleMessageReceive(ArraySegment<byte> arraySegment, WebSocketReceiveResult receiveResult, byte[] buffer)
        {
            if (receiveResult.MessageType == WebSocketMessageType.Close) await HandleClose();

            string messageContent = Encoding.UTF8.GetString(arraySegment.Array, arraySegment.Offset, receiveResult.Count);
            await ReceiveMessage(messageContent);
        }

        //TODO: Patch request isnt working!
        private static async Task<Task> ReceiveMessage(string messageContent)
        {
            if (messageContent.StartsWith("You are connected with UUID "))
            {
                string uuid = messageContent.Replace("You are connected with UUID ", "");
                Console.WriteLine("You can perform request trough our backend under the following url now");
                Console.WriteLine($"https://localhost:7006/send/{uuid}/");
                Console.WriteLine($"Example: https://localhost:7006/send/{uuid}/api/v1/getUsers");
                Console.WriteLine("---------------------------------------------------------------");
                Console.WriteLine();
                return Task.CompletedTask;
            }

            try
            {
                var requestJSON = JSONUtil.ConvertJSONToRequest(messageContent);
                requestJSON.requestURL = requestJSON.requestURL.Substring(5);

                string requestURL = "http://localhost:" + StorageUtil.ForwardPort + requestJSON.requestURL.ToString();
                ConsoleUtil.SendLogMessage(requestURL, requestJSON.requestMethode);

                IncomingRequestStruct requestStruct = new IncomingRequestStruct(requestJSON.requestBody, requestJSON.requestHeaders, requestJSON.requestMethode, requestURL, requestJSON.requestID);

                var result = RequestHandler.HandleRequestAsync(requestStruct);
                if (result == null)
                {
                    await SendMessage("Invalid request");
                    return Task.CompletedTask;
                }
                else if (result.Result == null)
                {
                    await SendMessage("Invalid request");
                    return Task.CompletedTask;
                }

                var resultJSON = JSONUtil.ConvertResponseToJSON(result.Result);
                await SendMessage("Receaving from " + resultJSON);
                return Task.CompletedTask;
            }
            catch (Exception e)
            {
                return Task.CompletedTask;
            }
        }

        public static async Task SendMessage(string message)
        {
            byte[] buffer = Encoding.UTF8.GetBytes(message);
            await StorageUtil.webSocket.SendAsync(new ArraySegment<byte>(buffer), WebSocketMessageType.Text, true, CancellationToken.None);
        }

        private static async Task HandleClose()
        {
            await StorageUtil.webSocket.CloseAsync(WebSocketCloseStatus.NormalClosure, null, CancellationToken.None);
        }
    }
}
