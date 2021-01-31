using System.Collections.Generic;
using UnityEngine;

public class GameLogic : MonoBehaviour
{
    [SerializeField] private LayerMask waterLayerMask;

    private readonly List<ISystem> activeSystems = new List<ISystem>();

    private IGameState gameState;

    public void Start()
    {
        GameEventManager.Subscribe<SceneLoadedMessage>(Initialise);
    }

    public void Initialise(object message)
    {
        new GolfBallSelectionSystem().Initialise(activeSystems);
        new GolfBallSpawnerSystem().Initialise(activeSystems);
        new CollectablesSystem().Initialise(activeSystems);
        new ControlZoneSystem().Initialise(activeSystems);
        new KingSystem().Initialise(activeSystems);
        new GolfBallCooldownSystem().Initialise(activeSystems);
        new WaterSystem(waterLayerMask).Initialise(activeSystems);

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
}
