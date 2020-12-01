using UnityEngine;
using System.Collections;

[BoltGlobalBehaviour]
public class NetworkCallbacks : Bolt.GlobalEventListener
{
    [System.Obsolete]
    public override void SceneLoadLocalDone(string scene)
    {
        // randomize a position
        var spawnPosition = new Vector3(Random.Range(-16, 16), Random.Range(-16, 16));

        // instantiate cube
        BoltNetwork.Instantiate(BoltPrefabs.Golfball, spawnPosition, Quaternion.identity);
    }
}