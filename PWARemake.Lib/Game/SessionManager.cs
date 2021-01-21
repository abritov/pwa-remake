using System;
using System.Collections.Generic;
using Akka.Actor;
using Akka.Event;
using PWARemake.Lib.Net;
using PWARemake.Lib.Protocol;

namespace PWARemake.Lib.Game
{

    public abstract class SessionManagerEvents { }
    public sealed class AddSession : SessionManagerEvents
    {
        public string Login { get; private set; }
        public string Password { get; private set; }
        public AddSession(string login, string password)
        {
            Login = login;
            Password = password;
        }
    }
    sealed class DelSession : SessionManagerEvents { }
    sealed class SendSkills : SessionManagerEvents { }
    sealed class GetRoles : SessionManagerEvents { }

    public class SessionManager : ReceiveActor
    {
        private readonly ILoggingAdapter log = Context.GetLogger();
        private readonly List<IActorRef> sessions = new List<IActorRef>();

        public SessionManager()
        {
            Receive<AddSession>(msg =>
            {
                log.Info("Received message: {0}", msg.Login);
                var session = Context.ActorOf(Props.Create(() => 
                    new Session(new PwToHttpBridge(new Api(), new Uri("ws://pw-http-bridge.herokuapp.com/140/ws")), Context.Self)));
                sessions.Add(session);
                session.Tell("start!");
                Sender.Tell("Ok");
            });
            Receive<DelSession>(msg =>
            {
                log.Info("Received message: {0}", msg);
                Sender.Tell("Ok");
            });
        }
    }

}