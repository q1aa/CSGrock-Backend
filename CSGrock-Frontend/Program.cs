using System;
using System.Threading;
using System.Net.WebSockets;
using CSGrock_Frontend.CSGrockLogic.Socket;
using System.Threading.Tasks;
using CSGrock_Frontend.CSGrockLogic.Utils;

namespace CSGrock_Frontend
{
    internal class Program
    {
        static void Main(string[] args)
        {
            if(args.Length == 0) _ = StartUpWithoutArgsAsync();
            
            foreach(string arg in args)
            {
                Console.WriteLine(arg);
            }
            Console.ReadLine();
        }

        static async Task StartUpWithoutArgsAsync()
        {
            Console.WriteLine("Please enter the server type and port you want to start up split by a space. (Example: http 8080)");
            string[] input = Console.ReadLine().Split(' ');
            if(input.Length != 2)
            {
                Console.WriteLine("Invalid input. Please try again.");
                await StartUpWithoutArgsAsync();
            }
            else
            {
                string serverType = input[0];
                string port = input[1];

                StorageUtil.ForwardPort = int.Parse(port);
                Console.Clear();
                await SocketConnection.ConnectToSocket();
                Console.ReadLine();
            }
        }
    }
}
