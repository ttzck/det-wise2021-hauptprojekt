using UnityEngine;
using Bolt;
using System.Collections.Generic;

public class GameLogic : EntityBehaviour<IGameState>
{
    [SerializeField] private List<EffectSetting> particleEffectSettings;

    private readonly List<ISystem> activeSystems = new List<ISystem>();

    public override void Attached()
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
}