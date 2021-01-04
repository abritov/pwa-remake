using System;
using System.Net.WebSockets;
using System.Threading;
using System.Threading.Tasks;
using Websocket.Client;

class EnterWorld {

    public EnterWorld(string serverAddr, int? selectRoleIndex, Account account) {

    }
}

public abstract class Events {}
sealed class Unknown: Events { }

class Api {

    // Task Connect() {
    //     connection.ReceiveAsync()
    //     return connection.ConnectAsync(new Uri(wsEndpoint), cancelation.Token);
    // }

    public string EnterWorld(string serverAddr, Account account) {
        return $"{{\"addr\": \"{serverAddr}\", \"select_role_by_index\": {account.DefaultRoleIndex}, \"account\": {{\"login\": \"{account.Login}\", \"passwd\": \"{account.Password}\"}}}}";
    }

    public Events ParseEvent(string msg) {
        return new Unknown();
    }
}