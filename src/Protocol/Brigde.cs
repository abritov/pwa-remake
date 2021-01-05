using System;
using System.Reactive.Linq;
using System.Threading;
using System.Threading.Tasks;
using Websocket.Client;

class PwToHttpBridge : IProtocol {
    Api api;
    WebsocketClient client;

    public PwToHttpBridge(Api api, Uri wsEndpoint) {
        this.api = api;
        this.client = new WebsocketClient(wsEndpoint);
        client.ReconnectTimeout = TimeSpan.FromSeconds(30);
        client.ReconnectionHappened.Subscribe(info =>
            Console.WriteLine($"Reconnection happened, type: {info.Type}"));
    }
    public async Task<IObservable<PwRpc>> EnterWorld(string serverAddr, Account account) {
        var cmd = api.EnterWorld(serverAddr, account);
        Console.WriteLine(cmd);
        await client.StartOrFail();
        await client.SendInstant(cmd);

        return client.MessageReceived.Select(msg => api.ParseEvent(msg.Text));
    }

    public void SendSkills() {

    }
}