using System;
using WebSocketSharp;
using WebSocketSharp.Server;

namespace WebSocketControllerService
{
    class Program
    {
        static void Main(string[] args)
        {
            var httpsv = new HttpServer(4649);
#if DEBUG
            // To change the logging level.
            httpsv.Log.Level = LogLevel.Trace;

            // To change the wait time for the response to the WebSocket Ping or Close.
            //httpsv.WaitTime = TimeSpan.FromSeconds (2);

            // Not to remove the inactive WebSocket sessions periodically.
            //httpsv.KeepClean = false;
#endif
            httpsv.AddWebSocketService<Echo>("/Echo");
            httpsv.Start();
            if (httpsv.IsListening)
            {
                Console.WriteLine("Listening on port {0}, and providing WebSocket services:", httpsv.Port);
                foreach (var path in httpsv.WebSocketServices.Paths)
                    Console.WriteLine("- {0}", path);
            }

            Console.WriteLine("\nPress Enter key to stop the server...");
            Console.ReadLine();

            httpsv.Stop();
        }
    }

    public class Echo : WebSocketBehavior
    {
        protected override void OnMessage(MessageEventArgs e)
        {
            Console.WriteLine("- - - - - - - - -");
            Console.WriteLine(e.Data);
            Sessions.Broadcast(e.Data);
        }
    }
}
