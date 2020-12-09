using System;
using System.Reactive.Linq;

class GameContextMemoryProvider: IGameContextUpdatesProvider {
    Offsets offsets;
    IObservable<long> ticker = Observable.Interval(new TimeSpan(0, 0, 1));

    public GameContextMemoryProvider(Offsets offsets) {
        this.offsets = offsets;
    }

    IObservable<int> GetHpUpdates() {
        return ticker.Scan(0, (acc, value) => acc + 1);
    }
    IObservable<int> GetMpUpdates() {
        throw new System.Exception("not implemented");
    }
    IObservable<Role> GetAttackUpdates() {
        throw new System.Exception("not implemented");
    }
    IObservable<Point3D> GetCoordinateUpdates() {
        throw new System.Exception("not implemented");
    }
}