using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Bolt;

public class GolfBallHitSystem : GlobalEventListener
{
    [SerializeField] private float forceScale;

    public override void OnEvent(HitEvent hitEvent)
    {
        var entity = BoltNetwork.FindEntity(hitEvent.Id);
        var rb = entity.GetComponent<Rigidbody2D>();
        var state = entity.GetState<IGolfBallState>();
        if (state.ReadyToMove)
        {
            rb.AddForce(hitEvent.Force * forceScale);
        }
    }
}
