using System.Collections.Generic;
using System.Linq;

public class EffectBoltEventReceiverSystem : GameSystem
{
    private List<EffectSetting> settings;

    public EffectBoltEventReceiverSystem(List<EffectSetting> settings)
    {
        this.settings = settings;
    }

    public override void SetUp(IGameState gameState)
    {
        GameEventManager.Subscribe<EffectBoltEvent>(TranslateToGameObject);
    }

    private void TranslateToGameObject(object message)
    {
        var effectEvent = message as EffectBoltEvent;
        var gameObject = settings.First(x => x.ID == effectEvent.ID).GameObject;
        GameEventManager.Publish(new EffectMessage
        {
            Effect = gameObject,
            Position = effectEvent.Position
        });
    }
}
