using Bolt;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleEffectsSystem : GlobalEventListener
{
    [SerializeField] private LayerMask golfBallLayerMask;
    [SerializeField] private LayerMask wallLayerMask;

    [SerializeField] private List<GameObject> particleEffects;

    private void Start()
    {
        if (BoltNetwork.IsServer)
            CollisionEvent.Published += OnCollision;
    }

    private void OnCollision(CollisionArgs args)
    {
        if (golfBallLayerMask.Contains(args.A.layer) && wallLayerMask.Contains(args.B.layer))
        {
            ParticleEffectEvent.Post(
                GlobalTargets.Everyone,
                ReliabilityModes.Unreliable,
                ParticleIndex: 0,
                args.A.transform.position);
        }
    }

    public override void OnEvent(ParticleEffectEvent particleEffectEvent)
    {
        Instantiate(
            particleEffects[particleEffectEvent.ParticleIndex],
            particleEffectEvent.Position, 
            Quaternion.identity);
    }
}
