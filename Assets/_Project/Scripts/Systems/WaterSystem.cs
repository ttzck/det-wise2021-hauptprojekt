using UnityEngine;
using System.Linq;

public class WaterSystem : ServerSystem
{
    private LayerMask waterLayerMask;

    public WaterSystem(LayerMask waterLayerMask) => this.waterLayerMask = waterLayerMask;

    public override void Execute(IGameState gameState)
    {
        UpdateGolfBalls();
        CheckForGameOver(gameState);
    }

    private void UpdateGolfBalls()
    {
        foreach (var ball in SystemUtils.FindEntitiesWith<IGolfBallState>())
        {
            var state = ball.GetState<IGolfBallState>();
            if (!state.Alive) continue;
            if (Physics2D.OverlapPoint(ball.transform.position, waterLayerMask) != null)
            {
                WaterEffectEvent.Post(Bolt.GlobalTargets.Everyone, Bolt.ReliabilityModes.Unreliable, ball.transform.position);
                state.Alive = false;
                state.IsKing = false;
                state.Score = 0;

                ball.transform.position = RandomPositionFarFarAway;
            }
        }
    }

    private void CheckForGameOver(IGameState gameState)
    {
        if (SystemUtils.FindAll<IGolfBallState>().All(i => !i.Alive)) gameState.IsOver = true;
    }

    private static Vector3 RandomPositionFarFarAway =>
        Vector3.up * 1000 + Vector3.right * Random.Range(1000, 2000);
    
}