using System.Collections.Generic;

/// <summary>
/// Systems are meant to define the logic of the game.
/// Their behaviour should only be dependent on and change external state.
/// </summary>
public interface ISystem
{
    void SetUp(IGameState gameState);

    void Execute(IGameState gameState);

    void Initialise(List<ISystem> activeSystems);
}
