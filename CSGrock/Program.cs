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
using CSGrock.Pages._404;
using CSGrock.Pages.Homepage;
using Microsoft.Extensions.FileProviders;

//create a new main class
namespace CSGrock
{
    class Program
    {
        static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);


            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();

            StorageUtil.app = builder.Build();

            //creae a new socket server and log it when started
            StorageUtil.app.UseWebSockets();
            StorageUtil.app.Logger.LogInformation("Socket server started");

            StorageUtil.app.UseStaticFiles();
            StorageUtil.app.UseStaticFiles(new StaticFileOptions
            {
                FileProvider = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "UserContent")),
            });

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
                    Guid uuid = Guid.NewGuid();
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

            StorageUtil.app.Map("/", async context =>
            {
                context.Response.StatusCode = StatusCodes.Status200OK;
                await context.Response.WriteAsync(Homepage_page.GetHomepage());
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
                else if(requestPath == "/")
                {
                    await next.Invoke();
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
                    
                    //requestPath = requestPath.Replace("/" + requestID, ""); this will not replace /send so its causing errors!!!
                    requestPath = requestPath.Replace("/send/" + requestID, "");

                    string requestUUID = Guid.NewGuid().ToString();
                    await SocketHandler.HandleRequestForwarder(socketConnection, requestMethode, requestPath, requestBody, requestHeaders, requestUUID);

                    await HelperUtil.CheckForRequestResult(requestUUID, 100, 100, async (result) =>
                    {
                        if (result.resultContent == StorageUtil.errorOnFrontendMessage && result.resultStatusCode == System.Net.HttpStatusCode.BadGateway)
                        {
                            context.Response.StatusCode = (int)result.resultStatusCode;
                            await context.Response.WriteAsync("Auto generated: cant send a request to the localhost...");
                            return;
                        }

                        if(result.resultContent == "Lookup-dir" && result.resultStatusCode == HttpStatusCode.Gone)
                        {
                            string fileDirectory = Path.Combine(Directory.GetCurrentDirectory(), @$"UserContent/{result.requestID}");

                            string[] files = Directory.GetFiles(fileDirectory);
                            if (files.Length > 0)
                            {
                                string filePath = files[0];
                                string fileName = Path.GetFileName(filePath);
                                string fileExtension = Path.GetExtension(filePath);
                                if (!File.Exists(filePath))
                                {
                                    context.Response.StatusCode = StatusCodes.Status404NotFound;
                                    await context.Response.WriteAsync("File not found");
                                    return;
                                }

                                //await ResponseHeaders.AddFileExtensionHeaders(context, fileExtension);
                                await context.Response.Body.WriteAsync(File.ReadAllBytes(filePath)).AsTask();

                                Directory.Delete(Path.Combine(Directory.GetCurrentDirectory(), @$"UserContent/{result.requestID}"), true);
                                return;
                            }
                            else
                            {
                                context.Response.StatusCode = StatusCodes.Status404NotFound;
                                await context.Response.WriteAsync("File not found");
                                return;
                            }
                        }

                        StorageUtil.app.Logger.LogInformation("Started main response");

                        /*var mimeType = "image/jpeg"; // Assuming JPEG format, adjust as needed
                        var dataUri = $"data:{mimeType};base64,{result.resultContent}";
                        Console.WriteLine(dataUri);
                        context.Response.ContentType = mimeType;
                        await context.Response.WriteAsync($"<img src=\"{dataUri}\" />");*/


                        //unsure if its working, have to test it! if not, i have to find another way to send all the headers along with the response
                        foreach (var header in result.resultHeaders)
                        {
                            StorageUtil.app.Logger.LogWarning($"Header: {header.Key} : {header.Value}");
                            context.Response.Headers.Add(header.Key, header.Value);
                        }

                        StorageUtil.app.Logger.LogInformation("Finished main response");
                        context.Response.StatusCode = (int)result.resultStatusCode;
                        await context.Response.WriteAsync(result.resultContent);
                    });
                }
                else
                {
                    //StorageUtil.app.Logger.LogInformation("Invoke at else");
                    StorageUtil.app.Logger.LogInformation("Invoke at else");
                    await context.Response.WriteAsync(Get404.Get404Page());
                    //await next.Invoke();
                }
            });

            StorageUtil.app.UseHttpsRedirection();

            StorageUtil.app.UseAuthorization();

            StorageUtil.app.MapControllers();

            StorageUtil.app.Run();
        }
    }
}