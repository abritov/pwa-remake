using System;

public interface IGameContextUpdatesProvider {
    IObservable<int> GetHpUpdates();
    IObservable<int> GetMpUpdates();
    IObservable<Role> GetAttackUpdates();
    IObservable<Point3D> GetCoordinateUpdates();
}