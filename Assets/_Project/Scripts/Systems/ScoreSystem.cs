using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

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
            BoltNetwork.Destroy(b);

        }
    }

    private void Update()
    {
        var queryScore = BoltNetwork.Entities
            .Where(i => i.StateIs<IGolfBallState>())
            .Select(j => j.GetState<IGolfBallState>())
            .GroupBy(i => i.TeamId, j => j.Score, (teamID, Scores) => new
            {
                Teamid = teamID,
                Score = Scores.Sum()
            });

    }
}
