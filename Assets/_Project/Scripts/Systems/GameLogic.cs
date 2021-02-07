using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameLogic : MonoBehaviour
{
    [SerializeField] private LayerMask waterLayerMask;

    private readonly List<ISystem> activeSystems = new List<ISystem>();

    private IGameState gameState;

    public void Start()
    {
        GameEventManager.Subscribe<SetupGameEvent>(m => Initialise(m as SetupGameEvent));

        if (BoltNetwork.IsServer)
        {
            SetupGameEvent.Post(Bolt.GlobalTargets.Everyone, Bolt.ReliabilityModes.ReliableOrdered, Random.Range(int.MinValue, int.MaxValue));
        }
    }

    public void Initialise(SetupGameEvent message)
    {
        new GameSetupSystem(message.Seed).Initialise(activeSystems);
        new GolfBallSelectionSystem().Initialise(activeSystems);
        new CollectablesSystem().Initialise(activeSystems);
        new ControlZoneSystem().Initialise(activeSystems);
        new KingSystem().Initialise(activeSystems);
        new GolfBallCooldownSystem().Initialise(activeSystems);
        new WaterSystem(waterLayerMask).Initialise(activeSystems);

        gameState = BoltNetwork.Instantiate(BoltPrefabs.Game_State).GetState<IGameState>();
        gameState.NumberOfTeams = BoltNetwork.Clients.Count();

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
