using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Bolt;

public class SoundEffectsSystem : GlobalEventListener
{
    [SerializeField] private LayerMask golfBallLayerMask;
    [SerializeField] private LayerMask wallLayerMask;

    private void Start()
    {
        if (BoltNetwork.IsServer)
            CollisionEvent.Published += OnCollision;
    }

    private void OnCollision(CollisionArgs args)
    {
        if (golfBallLayerMask.Contains(args.A.layer) && wallLayerMask.Contains(args.B.layer))
        {
            SoundEffectEvent.Post(
                GlobalTargets.Everyone, 
                ReliabilityModes.Unreliable, 
                SoundIndex: 0);
        }
    }

    public override void OnEvent(SoundEffectEvent soundEffectEvent)
    {
        BoltLog.Info($"Play Sound with index {0}", soundEffectEvent.SoundIndex);
    }
}
