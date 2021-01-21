using System;
using System.Threading.Tasks;
using PWARemake.Lib.Utils;

namespace PWARemake.Lib.Protocol 
{
        
    public interface IProtocol {
        Task<IObservable<PwRpc>> EnterWorld(string serverAddr, Account account);
        Task Send(RpcCommand cmd);
    }

}