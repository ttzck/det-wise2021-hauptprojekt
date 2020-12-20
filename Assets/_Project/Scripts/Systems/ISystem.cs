using System.Collections.Generic;

public interface ISystem
{
    void Execute();

    void Initialise(List<ISystem> activeSystems);
}

public abstract class GameSystem : ISystem
{
    public abstract void Execute();

    public void Initialise(List<ISystem> activeSystems)
    {
        activeSystems.Add(this);
    }
}

public abstract class ServerSytem : ISystem
{
    public abstract void Execute();

    public void Initialise(List<ISystem> activeSystems)
    {
        if (BoltNetwork.IsServer) activeSystems.Add(this);
    }
}
