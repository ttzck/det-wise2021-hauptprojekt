using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

public class Match
{
    public List<Player> Players { get; private set; }

    public Match()
    {
        Players = new List<Player>();

        SpawnPlayers();
    }

    public BoltEntity this[Guid golfBallId]
    {
        get { return null; }
    }

    private void SpawnPlayers()
    {
        var clients = BoltNetwork.Clients.ToArray();
        for (int i = 0; i < clients.Length; i++)
        {
            var color = Color.HSVToRGB((1f / clients.Length) * i, 1, 1);
            Players.Add(new Player(color, clients[i]));
        }
        Players.Add(new Player(Color.black, null));
    }
}
