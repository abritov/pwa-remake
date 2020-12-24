using System;
using System.Reactive.Linq;

class GameContextMemoryProvider: IGameContextUpdatesProvider {
    Offsets offsets;
    IObservable<long> ticker = Observable.Interval(new TimeSpan(0, 0, 1));

    public GameContextMemoryProvider(Offsets offsets) {
        this.offsets = offsets;
    }

    public IObservable<int> GetHpUpdates() {
        return ticker.Scan(0, (acc, value) => acc + 1);
    }
    public IObservable<int> GetMpUpdates() {
        throw new System.Exception("not implemented");
    }
    public IObservable<Role> GetAttackUpdates() {
        throw new System.Exception("not implemented");
    }
    public IObservable<Point3D> GetCoordinateUpdates() {
        throw new System.Exception("not implemented");
    }
}