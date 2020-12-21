using System.Collections.Generic;

public abstract class ServerSystem : ISystem
{
    public virtual void SetUp(IGameState gameState) { }

    public virtual void Execute(IGameState gameState) { }

    public void Initialise(List<ISystem> activeSystems)
    {
        if (BoltNetwork.IsServer) activeSystems.Add(this);
    }
}
