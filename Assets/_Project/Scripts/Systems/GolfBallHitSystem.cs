using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Bolt;

public class GolfBallHitSystem : GlobalEventListener
{
    [SerializeField] private float forceScale;

    public override void OnEvent(HitEvent hitEvent)
    {
        BoltNetwork.FindEntity(hitEvent.Id)
            .GetComponent<Rigidbody2D>()
            .AddForce(hitEvent.Force * forceScale);
    }
}
