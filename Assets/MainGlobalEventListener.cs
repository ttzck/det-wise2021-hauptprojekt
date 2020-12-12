using Bolt;
using UnityEngine;

[BoltGlobalBehaviour(BoltNetworkModes.Server, "Main")]
public class MainGlobalEventListener : GlobalEventListener
{
    private Match match;

    private void Start()
    {
        match = new Match();
    }

    public override void OnEvent(HitEvent hit)
    {
        match[hit.Guid].GetComponent<Rigidbody2D>().AddForce(hit.Force);
    }
}
