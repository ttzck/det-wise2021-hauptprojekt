using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Bolt;
using sys = System;

[BoltGlobalBehaviour(BoltNetworkModes.Server, "Test1")]
public class Test1GlobalEventListener : GlobalEventListener
{
    private Dictionary<sys.Guid, BoltEntity> golfBalls = new Dictionary<sys.Guid, BoltEntity>();

    public override void SceneLoadRemoteDone(BoltConnection connection, IProtocolToken token)
    {
        var golfBall = BoltNetwork.Instantiate(BoltPrefabs.TestEntity);
        golfBall.AssignControl(connection);

        var state = golfBall.GetState<ITestEntityState>();
        state.Guid = sys.Guid.NewGuid();
        state.Color = new Color(Random.value, Random.value, Random.value);

        golfBalls.Add(state.Guid, golfBall);
    }

    public override void OnEvent(HitEvent hit)
    {
        golfBalls[hit.Guid].GetComponent<Rigidbody2D>().AddForce(hit.Force);
    }
}
