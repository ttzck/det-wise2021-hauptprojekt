using Bolt;

[BoltGlobalBehaviour]
public class EventMediator : GlobalEventListener
{
    public override void OnEvent(HitBoltEvent evnt)
        => GameEventManager.Publish(evnt);

    public override void OnEvent(EffectBoltEvent evnt)
        => GameEventManager.Publish(evnt);

}