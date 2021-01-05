public class ControlZoneSystem : ServerSystem
{

    public override void SetUp(IGameState gameState)
    {
        GameEventManager.Subscribe<CollisionMessage>(m => OnCollision(m as CollisionMessage));
    }
    private void OnCollision(CollisionMessage message)
    {
        if (message.EntityB.TryFindState(out IGolfBallState golfBallState) && (message.EntityA.TryFindState(out IControlZone controlZone)))
        {
            controlZone.Color = golfBallState.Color;
            controlZone.TeamId = golfBallState.TeamId;
        }
    }


}
