using Bolt;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

public class EffectBoltEventEmitterSystem : ServerSystem
{
    private readonly List<EffectSetting> settings;

    public EffectBoltEventEmitterSystem(List<EffectSetting> settings)
    {
        this.settings = settings;
    }

    public override void SetUp(IGameState gameState)
    {
        GameEventManager.Subscribe<HitBoltEvent>(OnHit);
        GameEventManager.Subscribe<CollisionMessage>(OnCollision);
    }

    private void OnHit(object message)
    {
        var hit = message as HitBoltEvent;
        var hitPosition = BoltNetwork.FindEntity(hit.Id).transform.position;
        PostAllFor<HitBoltEvent>(hitPosition);
    }

    private void OnCollision(object message)
    {
        var collision = message as CollisionMessage;
        PostAllFor<CollisionMessage>(collision.EntityA.transform.position);
    }

    private void PostAllFor<T>(Vector2 position)
    {
        foreach (var effect in FindEffectIDsFor<T>())
        {
            Post(effect, position);
        }
    }

    private IEnumerable<int> FindEffectIDsFor<T>()
    {
        var typeName = typeof(T).Name;
        return settings
            .Where(x => x.TriggerName == typeName)
            .Select(x => x.ID);
    }

    private void Post(int id, Vector2 position)
    {
        EffectBoltEvent.Post(
            GlobalTargets.Everyone,
            ReliabilityModes.Unreliable,
            id,
            position);
    }
}

[System.Serializable]
public class EffectSetting
{
    public string TriggerName;
    public GameObject GameObject;
    public int ID;
}