using CSGrock_Frontend.CSGrockLogic.Utils.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using static CSGrock_Frontend.CSGrockLogic.Utils.Enums.RequestMethodeEnum;

namespace CSGrock_Frontend.CSGrockLogic.Utils
{
    internal class ConsoleUtil
    {
        public static void SendLogMessage(string url, RequestMethode requestMethode)
        {
            switch (requestMethode)
            {
                case RequestMethodeEnum.RequestMethode.GET:
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.Write("GET");
                    break;
                case RequestMethode.POST:
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.Write("POST");
                    break;
                case RequestMethode.PUT:
                    Console.ForegroundColor = ConsoleColor.Blue;
                    Console.Write("PUT");
                    break;
                case RequestMethode.PATCH:
                    Console.ForegroundColor = ConsoleColor.Magenta;
                    Console.Write("PATCH");
                    break;
                case RequestMethode.DELETE:
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.Write("DELETE");
                    break;
                case RequestMethode.HEAD:
                    Console.ForegroundColor = ConsoleColor.DarkGreen;
                    Console.Write("HEAD");
                    break;
                case RequestMethode.OPTIONS:
                    Console.ForegroundColor = ConsoleColor.DarkMagenta;
                    Console.Write("OPTIONS");
                    break;
            }

            Console.ForegroundColor = ConsoleColor.White;
            Console.Write($" {url}");
            Console.WriteLine();
        }
    }
}
