using Bolt;
using UnityEngine;
using System.Linq;

public class GolfBallSpawnerSystem : GlobalEventListener
{
    private float collectableTimeStamp = 0;

    private void Start()
    {
        if (BoltNetwork.IsServer) SpawnGolfBalls();
        else enabled = false;
    }

    private void Update()
    {
        if (BoltNetwork.Time > collectableTimeStamp + GlobalSettings.CollectableSpawnCooldown)
        {
            SpawnCollectable();
        }
    }

    private void SpawnGolfBalls()
    {
        var clients = BoltNetwork.Clients.ToArray();
        for (int i = 0; i < clients.Length; i++)
        {
            var color = Color.HSVToRGB((1f / clients.Length) * i, 1, 1);
            SpawnTeam(clients[i], color, i + 1);
        }
        SpawnTeam(client: null, Color.black, 0);
    }

    private void SpawnTeam(BoltConnection client, Color color, int teamId)
    {
        for (int j = 0; j < GlobalSettings.NumberOfGolfBallsPerTeam; j++)
        {
            SpawnGolfBall(client, color, GameUtils.RandomPosition, teamId);
        }
    }

    private void SpawnGolfBall(BoltConnection client, Color color, Vector2 position, int teamId)
    {
        var golfBall = BoltNetwork.Instantiate(BoltPrefabs.Networked_Golf_Ball);
        golfBall.transform.position = position;

        var state = golfBall.GetState<IGolfBallState>();
        state.Color = color;
        state.TeamId = teamId;

        if (client == null) golfBall.TakeControl();
        else golfBall.AssignControl(client);
    }

    private void SpawnCollectable()
    {
        for (int i = 0; i < GlobalSettings.NumberOfCollectablesPerSpawnRound; i++)
        {
            BoltNetwork.Instantiate(BoltPrefabs.Star, GameUtils.RandomPosition, Quaternion.identity);
        }
        collectableTimeStamp = BoltNetwork.Time;
    }
}
