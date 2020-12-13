using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Bolt;
using Bolt.Matchmaking;

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
            sessionID: "test",
            sceneToLoad: "Main"
        );
    }
}
