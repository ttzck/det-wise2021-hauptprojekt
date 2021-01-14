using UnityEngine;

public class GolfBallCooldownSystem : ServerSystem
{
    public override void SetUp(IGameState gameState)
    {
        var golfBalls = SystemUtils
           .FindAll<IGolfBallState>();

        foreach (var golfBall in golfBalls)
        {
            golfBall.CooldownTimestamp =
                Time.time -
                Random.Range(0, GlobalSettings.GolfBallCooldown * .5f);
        }
    }

    public override void Execute(IGameState _)
    {
        var golfBalls = SystemUtils
            .FindAll<IGolfBallState>();

        foreach (var golfBall in golfBalls)
        {
            UpdateReadyToMove(golfBall);
            UpdateCooldownTimestamp(golfBall);
            UpdateCooldownRatio(golfBall);
        }
    }

    private void UpdateReadyToMove(IGolfBallState golfBall)
        => golfBall.ReadyToMove =
            BoltNetwork.Time > golfBall.CooldownTimestamp + GlobalSettings.GolfBallCooldown;

    private void UpdateCooldownTimestamp(IGolfBallState golfBall)
    {
        if (golfBall.Velocity > 0)
        {
            golfBall.CooldownTimestamp = float.PositiveInfinity;
        }
        else if (golfBall.CooldownTimestamp > BoltNetwork.Time)
        {
            golfBall.CooldownTimestamp = BoltNetwork.Time;
        }
    }

    private void UpdateCooldownRatio(IGolfBallState golfBall)
        => golfBall.CooldownRatio =
            1 - Mathf.Clamp01((BoltNetwork.Time - golfBall.CooldownTimestamp)
                / GlobalSettings.GolfBallCooldown);
}
