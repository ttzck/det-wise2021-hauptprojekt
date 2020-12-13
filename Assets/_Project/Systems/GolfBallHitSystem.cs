using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Bolt;

public class GolfBallHitSystem : GlobalEventListener
{
    public override void OnEvent(HitEvent hit)
    {
        BoltNetwork.FindEntity(hit.Id)
            .GetComponent<Rigidbody2D>()
            .AddForce(hit.Force);
    }
}
