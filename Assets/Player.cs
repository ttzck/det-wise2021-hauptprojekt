using System.Collections.Generic;
using UnityEngine;
using System;

public class Player
{
    public Color TeamColor { get; private set; }
    public BoltConnection Client { get; private set; }
    public Guid TeamId { get; private set; }
    public List<BoltEntity> GolfBalls { get; private set; }

    public Player(Color teamColor, BoltConnection client)
    {
        TeamColor = teamColor;
        Client = client;
        TeamId = Guid.NewGuid();
        GolfBalls = new List<BoltEntity>();

        SpawnGolfBalls();
    }

    private void SpawnGolfBalls()
    {
        for (int i = 0; i < GlobalSettings.NumberOfGolfBallsPerTeam; i++)
        {
            SpawnGolfball();
        }
    }

    private void SpawnGolfball()
    {
        var golfBall = BoltNetwork.Instantiate(BoltPrefabs.Golf_Ball_Networked_Variant);

        var state = golfBall.GetState<IGolfBallState>();
        state.Guid = Guid.NewGuid();
        state.Color = TeamColor;

        GolfBalls.Add(golfBall);

        if (Client == null)
        {
            golfBall.TakeControl();
        }
        else 
        {
            golfBall.AssignControl(Client);
        }
    }
}
