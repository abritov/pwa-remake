using System;
using System.Collections.Generic;
using System.Reactive;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using Akka.Actor;
using PWARemake.Game;

namespace PWARemake
{
    class Program
    {
        static void Main(string[] args)
        {
            var res3 = Observable
                .Interval(TimeSpan.FromSeconds(1))
                .TakeUntil(new DateTimeOffset(2013, 1, 1, 12, 0, 0, TimeSpan.Zero));
            res3.Subscribe(val => Console.WriteLine(val));
            Console.WriteLine("Hello world!");
            
            var system = ActorSystem.Create("PWA");
            var actor = system.ActorOf(Props.Create(() => new SessionManager()));
            actor.Tell(new AddSession("", ""));
            Console.ReadLine();
        }
    }
}
