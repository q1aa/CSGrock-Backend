using System.Diagnostics;
using CSGrock.CSGrockLogic;
using CSGrock.CSGrockLogic.Utils;
using System.Net.WebSockets;
using System.Text;
using CSGrock.CSGrockLogic.Socket;
using CSGrock.CSGrockLogic.Struct;
using System.Net.NetworkInformation;
using CSGrock.CSGrockLogic.Utils.Enums;
using System.Reflection;
using System.Net.Sockets;
using System.Net;

//create a new main class
namespace CSGrock
{
    class Program
    {
        //create a new main method
        static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);


            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();

            StorageUtil.app = builder.Build();

            //creae a new socket server and log it when started
            StorageUtil.app.UseWebSockets();
            StorageUtil.app.Logger.LogInformation("Socket server started");

            StorageUtil.app.Map("/ws", async context =>
            {
                if (context.WebSockets.IsWebSocketRequest)
                {
                    var ws = await context.WebSockets.AcceptWebSocketAsync();

                    /*var user = new RequestUserStruct("CSGrock", Guid.NewGuid());
                    var messgage = $"User {user.username} connected with UUID {user.UUID}";
                    var bytes = Encoding.ASCII.GetBytes(messgage);
                    var arraySegment = new ArraySegment<byte>(bytes);
                    await ws.SendAsync(arraySegment, WebSocketMessageType.Text, true, CancellationToken.None);
                    //set the id to the socket*/

                    //var socketConnection = new WebSocketConnection(Guid.NewGuid(), "CSGrock", ws);
                    //create a new Guid equal to 1
                    Guid uuid = Guid.Empty;
                    var socketConnection = new WebSocketConnection(uuid, "CSGrock", ws);
                    StorageUtil.app.Logger.LogInformation($"User {socketConnection.username} connected with UUID {socketConnection.UUID}");
                    await SocketHandler.SendMessage(socketConnection, $"You are connected with UUID {socketConnection.UUID}");
                    StorageUtil.allSockettConnections.Add(socketConnection);

                    await SocketHandler.ReceiveMessage(socketConnection);
                }
                else
                {
                    context.Response.StatusCode = StatusCodes.Status400BadRequest;
                    await context.Response.WriteAsync("Not a websocket request");
                }
            });

            //middleware
            _ = StorageUtil.app.Use(async (context, next) =>
            {
                string requestPath = context.Request.Path;
                string requestID = requestPath.Split("/")[1];


                if (requestPath == "/ws" && context.Request.Method.ToString() == "GET" && context.Request.Protocol.ToString() == "HTTP/1.1")
                {
                    StorageUtil.app.Logger.LogInformation("Invoke at ws");
                    await next.Invoke();
                    return;
                }
                else if (requestPath.StartsWith("/send/"))
                {
                    requestID = requestPath.Split("/")[2];
                    //check if the id is in the list
                    var socketConnection = StorageUtil.allSockettConnections.Find(x => x.UUID.ToString() == requestID);
                    if (socketConnection == null)
                    {
                        context.Response.StatusCode = StatusCodes.Status404NotFound;
                        await context.Response.WriteAsync("UUID not found");
                        return;
                    }


                    RequestEnum.RequestType requestMethode = EnumUtil.ParseRequestMethodeEnum(context.Request.Method);
                    string requestBody = await new StreamReader(context.Request.Body).ReadToEndAsync();
                    string[] requestHeaders = context.Request.Headers.Select(x => x.Key + ":" + x.Value).ToArray();
                    requestPath = requestPath.Replace("/" + requestID, "");

                    string requestUUID = Guid.NewGuid().ToString();
                    await SocketHandler.HandleRequestForwarder(socketConnection, requestMethode, requestPath, requestBody, requestHeaders, requestUUID);

                    await HelperUtil.CheckForRequestResult(requestUUID, 100, 100, async (result) =>
                    {
                        if(result.resultContent == StorageUtil.errorOnFrontendMessage && result.resultStatusCode == System.Net.HttpStatusCode.BadGateway)
                        {
                            context.Response.StatusCode = (int)result.resultStatusCode;
                            await context.Response.WriteAsync("Auto generated: cant send a request to the localhost...");
                            return;
                        }
                        context.Response.StatusCode = (int)result.resultStatusCode;
                        await context.Response.WriteAsync(result.resultContent);
                    });
                }
                else
                {
                    StorageUtil.app.Logger.LogInformation("Invoke at else");
                    await next.Invoke();
                }
            });

            StorageUtil.app.UseHttpsRedirection();

            StorageUtil.app.UseAuthorization();

            StorageUtil.app.MapControllers();

            StorageUtil.app.Run();
        }
    }
}

