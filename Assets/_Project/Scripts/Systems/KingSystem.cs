﻿using Bolt;
using System.Linq;
using UnityEngine;

public class KingSystem : ServerSystem
{
    private IGameState gameState;

    public override void SetUp(IGameState gameState)
    {
        this.gameState = gameState;
        GameEventManager.Subscribe<CollisionMessage>(m => OnCollision(m as CollisionMessage));
    }

    private void OnCollision(CollisionMessage message)
    {
        if (message.EntityA.TryFindState(out IGolfBallState golfBallStateA)
            && message.EntityB.TryFindState(out IGolfBallState golfBallStateB)
            && golfBallStateA.TeamId != golfBallStateB.TeamId)
        {
            golfBallStateA.IsKing = golfBallStateB.IsKing = false;
        }

        var kings = SystemUtils.FindAll<IGolfBallState>().Where(i => i.IsKing);
        if (kings.Count() == 1)
        {
            gameState.IsOver = true;
            gameState.WinnerId = kings.First().TeamId;
        }
    }
}