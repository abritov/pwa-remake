using System;

class PwToHttpBridge : IProtocol {
    Api api;

    public PwToHttpBridge(Api api) {
        this.api = api;
    }
    public IObservable<string> EnterWorld() {
        // return Observable.create
    }

    public IObservable<Role[]> GetRoles() {

    }
    public void SendSkills() {

    }
}