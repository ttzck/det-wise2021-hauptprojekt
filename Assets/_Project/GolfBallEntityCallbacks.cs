using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Bolt;

public class GolfBallEntityCallbacks : EntityBehaviour<IGolfBallState>
{
    public override void Attached()
    {
        state.SetTransforms(state.Transform, transform);
        state.AddCallback(nameof(state.Color), () => GetComponent<SpriteRenderer>().color = state.Color);
    }
}
