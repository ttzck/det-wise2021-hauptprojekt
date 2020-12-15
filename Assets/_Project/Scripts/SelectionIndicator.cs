using UnityEngine;

public class SelectionIndicator : MonoBehaviour
{
    [SerializeField] private float maxLength;

    public GolfBallSelectionSystem Owner { get; set; }

    private LineRenderer lineRenderer;

    private void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
    }

    private void Update()
    {
        lineRenderer.enabled = Owner.SelectedGolfBall != null;

        if (lineRenderer.enabled)
        {
            var length = maxLength * Owner.HitCharge;
            var pointA = (Vector2)Owner.SelectedGolfBall.transform.position;
            var pointB = pointA + Owner.HitDirection.normalized * length;
            lineRenderer.SetPositions(new Vector3[] { pointA, pointB });
        }
    }
}