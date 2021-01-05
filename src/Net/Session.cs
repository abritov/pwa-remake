using System;
using System.Reactive.Linq;
using System.Threading;
using System.Threading.Tasks;
using Akka.Actor;
using Akka.Event;

class Session : ReceiveActor {
    private readonly ILoggingAdapter log = Context.GetLogger();

    private IProtocol protocol;
    public Session(IProtocol protocol, IActorRef manager)
    {
        this.protocol = protocol;

        Receive<string>(message => {
            log.Info("Received String message: {0}", message);
            Start();
        });
    }

    private async void Start() {
        var events = await protocol.EnterWorld("51.255.67.56:28082", new Account() { Login = "skidrow1", Password = "123456", DefaultRoleIndex = 1 });
        events.Subscribe();
    }
}