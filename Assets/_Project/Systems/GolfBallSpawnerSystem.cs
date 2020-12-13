using Bolt;
using UnityEngine;
using System.Linq;

public class GolfBallSpawnerSystem : GlobalEventListener
{
    private void Start()
    {
        if (BoltNetwork.IsServer) SpawnGolfBalls();
    }

    private void SpawnGolfBalls()
    {
        var clients = BoltNetwork.Clients.ToArray();
        for (int i = 0; i < clients.Length; i++)
        {
            var color = Color.HSVToRGB((1f / clients.Length) * i, 1, 1);
            SpawnTeam(clients[i], color);
        }
        SpawnTeam(client: null, Color.black);
    }

    private void SpawnTeam(BoltConnection client, Color color)
    {
        for (int j = 0; j < GlobalSettings.NumberOfGolfBallsPerTeam; j++)
        {
            SpawnGolfBall(client, color, GetRandomPosition());
        }
    }

    private void SpawnGolfBall(BoltConnection client, Color color, Vector2 position)
    {
        var golfBall = BoltNetwork.Instantiate(BoltPrefabs.Networked_Golf_Ball);
        golfBall.transform.position = position;

        var state = golfBall.GetState<IGolfBallState>();
        state.Color = color;

        if (client == null) golfBall.TakeControl();
        else golfBall.AssignControl(client);
    }

    private Vector2 GetRandomPosition()
    {
        return Random.insideUnitCircle * 3;
    }
}
