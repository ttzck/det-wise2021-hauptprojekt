using UnityEngine;
using Bolt;

public class GolfBallLocalPropertiesUpdater : EntityBehaviour<IGolfBallState>
{
    private Rigidbody2D rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        state.Velocity = rb != null ? rb.velocity.magnitude : 0;
    }
}
