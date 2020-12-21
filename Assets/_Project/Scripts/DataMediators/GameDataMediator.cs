using UnityEngine;

public class GameDataMediator : DataMediator<IGameState>
{
    public override void Attached()
    {
        base.Attached();
        GameEventManager.Subscribe<EffectMessage>(SpawnEffect);
    }

    private void SpawnEffect(object message)
    {
        var effectMessage = message as EffectMessage;
        Instantiate(effectMessage.Effect, effectMessage.Position, Quaternion.identity);
    }
}
