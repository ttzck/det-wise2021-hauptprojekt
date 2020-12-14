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
            ColliderNotifier.CollisionEntered += OnCollision;
    }

    private void OnCollision(GameObject a, GameObject b)
    {
        if (golfBallLayerMask.Contains(a.layer) && wallLayerMask.Contains(b.layer))
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
