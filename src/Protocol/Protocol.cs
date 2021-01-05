using System;
using System.Threading.Tasks;

public interface IProtocol {
    Task<IObservable<PwRpc>> EnterWorld(string serverAddr, Account account);
}