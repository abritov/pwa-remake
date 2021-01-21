using System;
using Akka.Actor;
using PWARemake.Lib.Game;

namespace PWARemake.Bin
{
    class Program
    {
        static void Main(string[] args)
        {   
            var system = ActorSystem.Create("PWA");
            var actor = system.ActorOf(Props.Create(() => new SessionManager()));
            actor.Tell(new AddSession("", ""));
            Console.ReadLine();
        }
    }
}
