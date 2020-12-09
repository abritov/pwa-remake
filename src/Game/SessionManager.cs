using Akka.Actor;
using Akka.Event;

abstract class SessionManagerEvents {}
sealed class AddSession : SessionManagerEvents {
    public string Login { get; private set; }
    public string Password { get; private set; }
    public AddSession(string login, string password) {
        Login = login;
        Password = password;
    }
}
sealed class DelSession : SessionManagerEvents { }
sealed class SendSkills : SessionManagerEvents { }
sealed class GetRoles : SessionManagerEvents { }

class SessionManager : ReceiveActor {
    private readonly ILoggingAdapter log = Context.GetLogger();
    // private readonly Session[] = new Session[];

    public SessionManager()
    {
        Receive<AddSession>(msg => {
            log.Info("Received message: {0}", msg.Login);
            Sender.Tell("Ok");
        });
        Receive<DelSession>(msg => {
            log.Info("Received message: {0}", msg);
            Sender.Tell("Ok");
        });
    }
}