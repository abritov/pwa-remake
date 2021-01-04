using System;
using System.Threading.Tasks;

public interface IProtocol {
    Task<IObservable<Events>> EnterWorld(string serverAddr, Account account);
}