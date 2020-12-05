using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Bolt;

public class Test1EntityScript : EntityEventListener<ITestEntityState>
{
    public override void Attached()
    {
        state.SetTransforms(state.Transform, transform);

        state.AddCallback(nameof(state.Color), () => GetComponent<SpriteRenderer>().color = state.Color);
    }

    private void Update()
    {
        if (entity.HasControl)
        {
            if (Input.GetMouseButtonDown(0))
            {
                var force = (Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position) * 100f;
                HitEvent.Post(GlobalTargets.OnlyServer, force, state.Guid);
            }
        }
    }
}
