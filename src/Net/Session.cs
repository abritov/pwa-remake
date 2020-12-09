using Akka.Actor;
using Akka.Event;

class Session : ReceiveActor {
    private readonly ILoggingAdapter log = Context.GetLogger();

    public Session()
    {
        Receive<string>(message => {
            log.Info("Received String message: {0}", message);
            Sender.Tell(message);
        });
    }
}