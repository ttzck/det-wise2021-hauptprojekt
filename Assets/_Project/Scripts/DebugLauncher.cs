using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Bolt;
using Bolt.Matchmaking;
using System;

public class DebugLauncher : GlobalEventListener
{
    private void Start()
    {
        if(!BoltNetwork.IsRunning)
            BoltLauncher.StartServer();
    }

    public override void BoltStartDone()
    {
        BoltMatchmaking.CreateSession(
            sessionID: Guid.NewGuid().ToString(),
            sceneToLoad: "Main"
        );
    }
}
