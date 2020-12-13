using Bolt;
using UnityEngine;

public class NetworkedGolfBallBehaviour : EntityEventListener<IGolfBallState>
{
    private GolfBallBehaviour gBB;

    private void Start()
    {
        gBB = GetComponent<GolfBallBehaviour>();
    }

    public override void Attached()
    {
        HookUpProperties();
        RemovePhysicsComponents();
        SubscribeToGolfBallHit();
    }

    private void SubscribeToGolfBallHit()
    {
        if (entity.HasControl)
        {
            gBB.Hit += (force) => HitEvent.Post(GlobalTargets.OnlyServer, force, entity.NetworkId);
        }
    }

    private void HookUpProperties()
    {
        state.SetTransforms(state.Transform, transform);
        state.AddCallback(nameof(state.Color), () => GetComponent<SpriteRenderer>().color = state.Color);
    }

    private void RemovePhysicsComponents()
    {
        if (!entity.IsOwner)
        {
            Destroy(GetComponent<Rigidbody2D>());
            Destroy(GetComponent<Collider2D>());
        }
    }
}
