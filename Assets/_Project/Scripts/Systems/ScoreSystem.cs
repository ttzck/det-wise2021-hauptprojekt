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
            CollisionEvent.Published += OnTrigger;
    }

    private void OnTrigger(CollisionArgs args)
    {
        if (golfBallLayerMask.Contains(args.A.layer) && collectablesLayerMask.Contains(args.B.layer))
        {
            args.A.GetComponent<BoltEntity>().GetState<IGolfBallState>().Score++;
            BoltNetwork.Destroy(args.B);

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
