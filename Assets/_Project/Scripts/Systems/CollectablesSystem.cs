using System.Linq;
using UnityEngine;

public class CollectablesSystem : ServerSystem
{
    public override void SetUp(IGameState _)
    {
        GameEventManager.Subscribe<CollisionMessage>(OnCollision);

        foreach (var spot in SystemUtils.FindEntitiesWith<ICollectableSpot>())
        {
            var spotState = spot.GetState<ICollectableSpot>();
            var collectable = BoltNetwork.Instantiate(BoltPrefabs.Star, spot.transform.position, Quaternion.identity);
            var collectableState = collectable.GetState<ICollectable>();

            collectableState.Enabled = false;
            spotState.Enabled = true;
            spotState.CooldownTimestamp = Time.time;
            spotState.Collectable = collectable.NetworkId;
            collectableState.CollectableSpot = spot.NetworkId;
        }
    }

    private void OnCollision(object message)
    {
        var collision = message as CollisionMessage;
        if (collision.EntityA.StateIs<IGolfBallState>() 
            && collision.EntityB.StateIs<ICollectable>())
        {
            CollectCollectable(
                collision.EntityA.GetState<IGolfBallState>(), 
                collision.EntityB.GetState<ICollectable>());
        }
    }

    private static void CollectCollectable(IGolfBallState collector, ICollectable collectable)
    {
        collector.Score++;
        var spot = SystemUtils.Find<ICollectableSpot>(collectable.CollectableSpot);
        collectable.Enabled = false;
        spot.Enabled = true;
        spot.CooldownTimestamp = Time.time;
    }

    public override void Execute(IGameState _)
    {
        UpdateSpots();

        var queryScore = SystemUtils
            .FindAll<IGolfBallState>()
            .GroupBy(i => i.TeamId, j => j.Score, (teamID, Scores) => new
            {
                Teamid = teamID,
                Score = Scores.Sum()
            });
    }

    private static void UpdateSpots()
    {
        foreach (var spot in SystemUtils.FindAll<ICollectableSpot>()
            .Where(i => i.Enabled))
        {
            if (spot.CooldownTimestamp + GlobalSettings.CollectableSpawnCooldown < Time.time)
            {
                SystemUtils.Find<ICollectable>(spot.Collectable).Enabled = true;
                spot.Enabled = false;
            }

            spot.CooldownRatio = Mathf.Clamp01((Time.time - spot.CooldownTimestamp) / GlobalSettings.CollectableSpawnCooldown);
        }
    }
}
