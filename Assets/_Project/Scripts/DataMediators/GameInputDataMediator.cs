using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Diagnostics.CodeAnalysis.SuppressMessage("CodeQuality", "IDE0051:Remove unused private members", Justification = "Using Unity Messages")]
public class GameInputDataMediator : DataMediator<IGameInput>, Bolt.IEntityReplicationFilter
{
    [SerializeField] private LineRenderer lineRendererPrefab;
    [SerializeField] private float maxLineLength;

    private LineRenderer lineRenderer;
    private BoltEntity selectedGolfBall;

    public override void Attached()
    {
        base.Attached();

        lineRenderer = Instantiate(lineRendererPrefab);
        lineRenderer.enabled = false;
    }

    public bool AllowReplicationTo(BoltConnection connection) => false;

    private void OnSelectedGolfBallChanged()
    {
        selectedGolfBall = BoltNetwork.FindEntity(state.SelectedGolfBall);
        lineRenderer.enabled = selectedGolfBall != null;
    }

    private void OnChargeChanged() => UpdateLineRenderer();

    private void OnDirectrionChanged() => UpdateLineRenderer();

    private void UpdateLineRenderer()
    {
        if (selectedGolfBall != null)
        {
            var length = maxLineLength * state.Charge;
            var pointA = selectedGolfBall.transform.position;
            var pointB = pointA + state.Direction.normalized * length;
            lineRenderer.SetPositions(new Vector3[] { pointA, pointB });
        }
    }
}
