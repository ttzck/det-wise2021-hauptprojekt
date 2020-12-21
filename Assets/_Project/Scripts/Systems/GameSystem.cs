using System.Collections.Generic;

public abstract class GameSystem : ISystem
{
    public virtual void SetUp(IGameState gameState) { }

    public virtual void Execute(IGameState gameState) { }

    public void Initialise(List<ISystem> activeSystems)
    {
        activeSystems.Add(this);
    }
}
