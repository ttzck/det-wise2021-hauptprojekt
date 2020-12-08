using UnityEngine;

public class OfflineGolfBallBehaviour : MonoBehaviour
{
    private Rigidbody2D rb;
    private GolfBallBehaviour gBB;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        gBB = GetComponent<GolfBallBehaviour>();

        gBB.Hit += (force) => rb.AddForce(force);
    }
}