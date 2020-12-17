using Bolt;
using System.Linq;
using UnityEngine;

public class GolfBallCooldownSystem : MonoBehaviour
{
    [SerializeField] private float cooldown;

    private void Update()
    {
        if (!BoltNetwork.IsServer) return;

        var golfBalls = BoltNetwork.Entities
            .Where(i => i.StateIs<IGolfBallState>())
            .Select(i => i.GetState<IGolfBallState>());

        foreach (var golfBall in golfBalls)
        {
            UpdateReadyToMove(golfBall);
            UpdateCooldownTimestamp(golfBall);
            UpdateCooldownRatio(golfBall);
        }
    }

    private void UpdateReadyToMove(IGolfBallState golfBall)
    {
        golfBall.ReadyToMove = BoltNetwork.Time > golfBall.CooldownTimestamp + cooldown;
    }

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
    {
        golfBall.CooldownRatio = 1 - Mathf.Clamp01((BoltNetwork.Time - golfBall.CooldownTimestamp) / cooldown);
    }
}
