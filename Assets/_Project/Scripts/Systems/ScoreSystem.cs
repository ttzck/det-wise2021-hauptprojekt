using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreSystem : MonoBehaviour
{
    [SerializeField] private LayerMask golfBallLayerMask;
    [SerializeField] private LayerMask collectablesLayerMask;

    private void Start()
    {
        if (BoltNetwork.IsServer)
            ColliderNotifier.TriggerEntered += OnTrigger;
    }

    private void OnTrigger(GameObject a, GameObject b)
    {
        if (golfBallLayerMask.Contains(a.layer) && collectablesLayerMask.Contains(b.layer))
        {
            a.GetComponent<BoltEntity>().GetState<IGolfBallState>().Score++;
        }
    }
}
