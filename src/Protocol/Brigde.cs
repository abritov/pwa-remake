using System;
using System.Reactive.Linq;
using System.Threading;
using System.Threading.Tasks;
using PWARemake.Utils;
using Websocket.Client;

namespace PWARemake.Protocol 
{
    
    class PwToHttpBridge : IProtocol {
        Api api;
        WebsocketClient client;

        public PwToHttpBridge(Api api, Uri wsEndpoint) {
            this.api = api;
            this.client = new WebsocketClient(wsEndpoint) {
                IsTextMessageConversionEnabled = true,
                MessageEncoding = System.Text.Encoding.UTF8
            };
            client.ReconnectTimeout = TimeSpan.FromSeconds(30);
            client.ReconnectionHappened.Skip(1).Subscribe(info =>
                Console.WriteLine($"Reconnection happened, type: {info.Type}"));
            // client.MessageReceived.Do(msg => {
            //     Console.WriteLine(msg.MessageType);
            // }).Subscribe();
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

}