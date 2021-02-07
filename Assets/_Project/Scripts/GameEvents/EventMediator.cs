using Bolt;

[BoltGlobalBehaviour]
public class EventMediator : GlobalEventListener
{
    public override void OnEvent(HitBoltEvent evnt)
        => GameEventManager.Publish(evnt);

    public override void SceneLoadLocalDone(string scene, IProtocolToken token)
        => GameEventManager.Publish(new SceneLoadedMessage { Scene = scene });

    public override void OnEvent(SetupGameEvent evnt)
        => GameEventManager.Publish(evnt);
}