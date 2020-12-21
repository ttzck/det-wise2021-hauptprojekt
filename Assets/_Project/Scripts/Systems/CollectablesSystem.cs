using System.Collections;
using UnityEngine;
using System.Linq;

public class CollectablesSystem : ServerSystem
{
    public override void SetUp(IGameState _) 
        => GameEventManager.Subscribe<CollisionMessage>(OnCollision);

    private void OnCollision(object message)
    {
        var collision = message as CollisionMessage;
        if (collision.EntityA.StateIs<IGolfBallState>() 
            && collision.EntityB.StateIs<ICollectable>())
        {
            CollectCollectable(collision.EntityA, collision.EntityB);
        }
    }

    private static void CollectCollectable(BoltEntity collector, BoltEntity collectable)
    {
        collector.GetState<IGolfBallState>().Score++;
        BoltNetwork.Destroy(collectable);
    }

    public override void Execute(IGameState _)
    {
        var queryScore = SystemUtils
            .FindAll<IGolfBallState>()
            .GroupBy(i => i.TeamId, j => j.Score, (teamID, Scores) => new
            {
                Teamid = teamID,
                Score = Scores.Sum()
            });
    }
}
