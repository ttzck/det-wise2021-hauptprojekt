using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Bolt;
using System.Linq;

public class GolfBallSelectionSystem : MonoBehaviour
{
    [SerializeField] private LineRenderer selectionIndicatorPrefab;

    private Camera cam;
    private BoltEntity selectedGolfBall;
    private LineRenderer indicator;

    private void Start()
    {
        cam = Camera.main;

        indicator = Instantiate(selectionIndicatorPrefab);
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (selectedGolfBall == null)
                SelectGolfBall();
            else
                HitSelectedGolfBall();
        }

        if (Input.GetMouseButtonDown(1))
            selectedGolfBall = null;

        UpdateIndicator();
    }

    private void UpdateIndicator()
    {
        indicator.enabled = selectedGolfBall != null;

        if (selectedGolfBall != null)
        {
            Vector3[] line = { MousePosition, selectedGolfBall.transform.position };
            indicator.SetPositions(line);
        }
    }

    private void SelectGolfBall()
    {
        selectedGolfBall = BoltNetwork.Entities
            .Where(i => i.StateIs<IGolfBallState>())
            .Where(i => i.HasControl)
            .OrderBy(i => Vector2.Distance(MousePosition, i.transform.position))
            .First();
    }

    private void HitSelectedGolfBall()
    {
        var force = (MousePosition - (Vector2)selectedGolfBall.transform.position)
            * GlobalSettings.SwingFroceScale;
        HitEvent.Post(GlobalTargets.OnlyServer, force, selectedGolfBall.NetworkId);
        selectedGolfBall = null;
    }

    private Vector2 MousePosition => cam.ScreenToWorldPoint(Input.mousePosition);
}