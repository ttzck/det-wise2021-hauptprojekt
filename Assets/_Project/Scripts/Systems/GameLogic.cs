using UnityEngine;
using Bolt;
using System.Collections.Generic;
using System.Collections;

public class GameLogic : MonoBehaviour
{
    [SerializeField] private List<EffectSetting> particleEffectSettings;

    private readonly List<ISystem> activeSystems = new List<ISystem>();

    private IGameState gameState;

    public void Start()
    {
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
        new ControlZoneSystem().Initialise(activeSystems);
        new KingSystem().Initialise(activeSystems);
        
        gameState = BoltNetwork.Instantiate(BoltPrefabs.Game_State).GetState<IGameState>();

        foreach (var system in activeSystems)
        {
            system.SetUp(gameState);
        }
    }

    private void Update()
    {
        foreach (var system in activeSystems)
        {
            system.Execute(gameState);
        }
    }

    public void OnDestroy()
    {
        BoltLog.Info("Unsubscribing all Events");
        GameEventManager.UnsubscribeAll();
    }

    private void SpawnEffect(object message)
    {
        var effectMessage = message as EffectMessage;
        Instantiate(effectMessage.Effect, effectMessage.Position, Quaternion.identity);
    }
}
