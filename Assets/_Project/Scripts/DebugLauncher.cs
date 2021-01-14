using Bolt;
using Bolt.Matchmaking;
using System;
using UnityEngine;

public class DebugLauncher : GlobalEventListener
{
    private void Start()
    {
        GameEventManager.Subscribe<object>(AnnounceEvent);

        if (!BoltNetwork.IsRunning)
            BoltLauncher.StartServer();
    }

    public override void BoltStartDone()
    {
        BoltMatchmaking.CreateSession(
            sessionID: Guid.NewGuid().ToString(),
            sceneToLoad: "Main"
        );
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.X))
        {
            foreach (var ball in SystemUtils.FindAll<IGolfBallState>())
            {
                ball.IsKing = false;
            }
        }
    }

    private void AnnounceEvent(object message)
    {
        BoltLog.Info($"GameEvent: {message.GetType()}");
    }
}
