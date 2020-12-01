using UnityEngine;
using System.Collections;
using Bolt;

public class GolfBallMovement : Bolt.EntityBehaviour<IGolfBallState>
{
    [SerializeField] private float forceScale;

    private Vector2 positonMousePressed;

    private Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public override void Attached()
    {
        state.SetTransforms(state.GolfBallTransform, transform);
    }

    public override void SimulateOwner()
    {
        if (Input.GetMouseButtonDown(0) && ReadyToBeHit)
        {
            positonMousePressed = MousePosition;
        }

        if (Input.GetMouseButtonUp(0) && ReadyToBeHit)
        {
            rb.AddForce((MousePosition - positonMousePressed) * forceScale);
        }
    }

    private Vector2 MousePosition => Camera.main.ScreenToWorldPoint(Input.mousePosition);

    private bool ReadyToBeHit => rb.velocity.magnitude < float.Epsilon;
}