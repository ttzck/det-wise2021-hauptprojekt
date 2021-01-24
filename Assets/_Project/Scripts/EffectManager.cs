using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Bolt;

public class EffectManager : GlobalEventListener
{
    [SerializeField] private GameObject hitParticleEffect;
    [SerializeField] private GameObject hitSoundEffect;
    [SerializeField] private GameObject collectableSoundEffect;
    [SerializeField] private GameObject wallCollisionSoundEffect;
    [SerializeField] private LayerMask wallLayerMask;

    private void Start()
    {
        GameEventManager.Subscribe<CollisionMessage>(m => OnCollision(m as CollisionMessage));
    }

    private void OnCollision(CollisionMessage message)
    {
        if (wallLayerMask.Contains(message.GameObjectB.layer))
        {
            WallCollisionEffectEvent.Post(GlobalTargets.Everyone, ReliabilityModes.Unreliable);
        }
    }

    public override void OnEvent(HitBoltEffectEvent evnt)
    {
        Instantiate(hitParticleEffect, evnt.Position, Quaternion.identity);
        Instantiate(hitSoundEffect);
    }

    public override void OnEvent(CollectableEffectBoltEvent evnt)
    {
        Instantiate(collectableSoundEffect);
    }

    public override void OnEvent(WallCollisionEffectEvent evnt)
    {
        Instantiate(wallCollisionSoundEffect);
    }
}
