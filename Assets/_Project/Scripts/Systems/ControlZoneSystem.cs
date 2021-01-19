using System.Linq;

public class ControlZoneSystem : ServerSystem
{
    private IGameState game;
    public override void SetUp(IGameState gameState)
    {
        game = gameState;
        GameEventManager.Subscribe<CollisionMessage>(m => OnCollision(m as CollisionMessage));
    }
    private void OnCollision(CollisionMessage message)
    {
        if (message.IsOfKind(out IGolfBallState golfBallState, out IControlZone controlZoneState))
        {
            controlZoneState.Color = golfBallState.Color;
            controlZoneState.TeamId = golfBallState.TeamId;
        }

        var zones = SystemUtils.FindAll<IControlZone>();

        if (zones.Any() && zones.First().TeamId != 0
            && zones.All(zone => zone.TeamId == zones.First().TeamId))
        {
            game.IsOver = true;
            game.WinnerId = zones.First().TeamId;
        }
    }
}
