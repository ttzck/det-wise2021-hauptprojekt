using Bolt;
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
            CollisionNotifier.Collision += OnCollision;
    }

    private void OnCollision(GameObject a, GameObject b)
    {
        if (golfBallLayerMask.Contains(a.layer) && wallLayerMask.Contains(b.layer))
        {
            ParticleEffectEvent.Post(
                GlobalTargets.Everyone,
                ReliabilityModes.Unreliable,
                ParticleIndex: 0,
                a.transform.position);
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
