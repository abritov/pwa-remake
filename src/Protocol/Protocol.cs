using System;
using System.Threading.Tasks;
using PWARemake.Utils;

namespace PWARemake.Protocol 
{
        
    public interface IProtocol {
        Task<IObservable<PwRpc>> EnterWorld(string serverAddr, Account account);
        Task Send(RpcCommand cmd);
    }

}