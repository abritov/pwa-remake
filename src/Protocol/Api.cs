using System;
using System.Net.WebSockets;
using System.Threading;
using System.Threading.Tasks;

abstract class Events {}

class Api: IDisposable {
    ClientWebSocket connection;
    string wsEndpoint;
    Account account;
    CancellationTokenSource cancelation = new CancellationTokenSource();

    public Api(string wsEndpoint, Account account) {
        connection = new ClientWebSocket();
        this.wsEndpoint = wsEndpoint;
        this.account = account;
    }

    // Task Connect() {
    //     connection.ReceiveAsync()
    //     return connection.ConnectAsync(new Uri(wsEndpoint), cancelation.Token);
    // }

    public IObservable<Events> EnterWorld() {

    }

    public void Dispose()
    {
        cancelation.Cancel();
        connection.Dispose();
    }
}