using System;
using System.Reactive.Linq;
using System.Reactive;
using System.Threading;
using System.Threading.Tasks;
using Akka.Actor;
using Akka.Event;

class Session : ReceiveActor {
    private readonly ILoggingAdapter log = Context.GetLogger();
    private long? hostId;

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
        var connection = await protocol.EnterWorld(
            "51.255.67.56:28082", 
            new Account() { Login = "skidrow2", Password = "123456", DefaultRoleIndex = 0 }
        );
        var events = Observable.Publish(connection);

        var awaitConnectionToTheHost = events
            .Where(x => x is PrivateChat)
            .Cast<PrivateChat>()
            .Where(x => x.msg == "hi")
            .Subscribe(x => {
                hostId = x.src_id;
                Console.WriteLine("Connected");
            });

        var readHostMoves = events
            .Where(x => x is RpcContainer)
            .SelectMany(x => ((RpcContainer)x).Packets)
            .Where(x => x is ObjectMove)
            .Cast<ObjectMove>()
            .Where(x => hostId != null && x.id == hostId)
            .Subscribe(target => Console.WriteLine($"{target.dest.X} {target.dest.Y} {target.dest.Z}"));

        events.Connect();
    }
}