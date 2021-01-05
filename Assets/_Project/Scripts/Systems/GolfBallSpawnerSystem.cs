using Bolt;
using UnityEngine;
using System.Linq;

public class GolfBallSpawnerSystem : ServerSystem
{
    public override void SetUp(IGameState gameState)
    {
        gameState.NumberOfTeams = BoltNetwork.Clients.Count();
        SpawnGolfBalls();
    }

    private void SpawnGolfBalls()
    {
        var clients = BoltNetwork.Clients.ToArray();
        for (int i = 0; i < clients.Length; i++)
        {
            var color = Color.HSVToRGB((1f / clients.Length) * i, 1, 1);
            SpawnTeam(new Team
            {
                client = clients[i],
                color = GameUtils.GetTeamColor(i, clients.Length),
                teamId = i+1
             }) ;
        }
        SpawnTeam(new Team
        {
            client = null,
            color = Color.magenta,
            teamId = clients.Length + 1
        });
    }

    private void SpawnTeam(Team team)
    {
        for (int j = 0; j < GlobalSettings.NumberOfGolfBallsPerTeam; j++)
        {
            SpawnGolfBall(team, j == 0);
        }
    }

    private void SpawnGolfBall(Team team, bool isKing)
    {
        var golfBall = BoltNetwork.Instantiate(BoltPrefabs.Networked_Golf_Ball);
        golfBall.transform.position = GameUtils.RandomPosition;

        var state = golfBall.GetState<IGolfBallState>();
        state.Color = team.color;
        state.TeamId = team.teamId;
        state.IsKing = isKing;

        if (team.client == null) golfBall.TakeControl();
        else golfBall.AssignControl(team.client);
    }

    private class Team
    {
        public BoltConnection client;
        public Color color;
        public int teamId;
        

    }
}


