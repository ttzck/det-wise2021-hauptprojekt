using UnityEngine;
using Bolt;
using System.Collections.Generic;
using System.Collections;

public class GameLogic : DataMediator<IGameState>
{
    [SerializeField] private List<EffectSetting> particleEffectSettings;

    private readonly List<ISystem> activeSystems = new List<ISystem>();

    public override void Attached()
    {
        base.Attached();

        GameEventManager.Subscribe<EffectMessage>(SpawnEffect);
        GameEventManager.Subscribe<SceneLoadedMessage>(Initialise);
    }

    public void Initialise(object message)
    {
        new GolfBallCooldownSystem().Initialise(activeSystems);
        new GolfBallSelectionSystem().Initialise(activeSystems);
        new GolfBallSpawnerSystem().Initialise(activeSystems);
        new CollectablesSystem().Initialise(activeSystems);
        new EffectBoltEventEmitterSystem(particleEffectSettings).Initialise(activeSystems);
        new EffectBoltEventReceiverSystem(particleEffectSettings).Initialise(activeSystems);

        foreach (var system in activeSystems)
        {
            system.SetUp(state);
        }
    }

    private void Update()
    {
        foreach (var system in activeSystems)
        {
            system.Execute(state);
        }
    }

    public override void Detached()
    {
        GameEventManager.UnsubscribeAll();
    }

    private void SpawnEffect(object message)
    {
        var effectMessage = message as EffectMessage;
        Instantiate(effectMessage.Effect, effectMessage.Position, Quaternion.identity);
    }
}