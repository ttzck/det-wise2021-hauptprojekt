using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Bolt;
using System.Linq;

public class GolfBallSelectionSystem : MonoBehaviour
{
    [SerializeField] private LineRenderer selectionIndicatorPrefab;
    [SerializeField] private float hitChargeSpeed;
    [SerializeField] private float maxIndicatorLength;

    private Camera cam;
    private LineRenderer indicator;
    private BoltEntity selectedGolfBall;
    private float hitCharge;
    private Vector2 hitDirection;

    private enum State { NoneSelected, Selected, Charge }
    private State state;

    private void Start()
    {
        cam = Camera.main;

        indicator = Instantiate(selectionIndicatorPrefab);
    }

    void Update()
    {
        switch (state)
        {
            case State.NoneSelected:
                UpdateWithNoneSelected();
                break;
            case State.Selected:
                UpdateWithSelected();
                break;
            case State.Charge:
                UpdateWithCharge();
                break;
        }

        UpdateIndicator();
    }

    private void UpdateWithNoneSelected()
    {
        if (Input.GetMouseButtonDown(0))
        {
            TransitionToSelected();
        }
    }

    private void UpdateWithSelected()
    {
        hitDirection = SelectedGolfBallToMouseVector.normalized;

        if (Input.GetMouseButtonDown(1))
        {
            TransitionToNoneSelected();
        }
        else if (Input.GetMouseButtonDown(0))
        {
            TransitionToCharge();
        }
    }

    private void UpdateWithCharge()
    {
        if (Input.GetMouseButtonDown(1))
        {
            TransitionToNoneSelected();
        }
        else if (Input.GetMouseButton(0))
        {
            hitCharge = Mathf.PingPong(Time.time * hitChargeSpeed, 1f);
        }
        else if (Input.GetMouseButtonUp(0))
        {
            HitSelectedGolfBall();
            TransitionToNoneSelected();
        }
    }

    private void TransitionToNoneSelected()
    {
        state = State.NoneSelected;
        selectedGolfBall = null;
        hitDirection = Vector2.zero;
    }

    private void TransitionToSelected()
    {
        state = State.Selected;
        selectedGolfBall = FindNearestGolfBall();
    }

    private void TransitionToCharge()
    {
        state = State.Charge;
        hitCharge = 0;
    }

    private void UpdateIndicator()
    {
        indicator.enabled = state == State.Selected || state == State.Charge;

        if (state == State.Selected || state == State.Charge)
        {
            var indicatorLength = state == State.Charge ? hitCharge * maxIndicatorLength : maxIndicatorLength *.5f;
            var pointA = (Vector2)selectedGolfBall.transform.position;
            var pointB = pointA + hitDirection.normalized * indicatorLength;
            indicator.SetPositions(new Vector3[] { pointA, pointB });
        }
    }

    private BoltEntity FindNearestGolfBall()
    {
        return BoltNetwork.Entities
            .Where(i => i.StateIs<IGolfBallState>())
            .Where(i => i.HasControl)
            .OrderBy(i => Vector2.Distance(MousePosition, i.transform.position))
            .First();
    }

    private void HitSelectedGolfBall()
    {
        var force = hitDirection.normalized * hitCharge;
        HitEvent.Post(GlobalTargets.OnlyServer, force, selectedGolfBall.NetworkId);
    }

    private Vector2 MousePosition => cam.ScreenToWorldPoint(Input.mousePosition);

    private Vector2 SelectedGolfBallToMouseVector
        => MousePosition - (Vector2)selectedGolfBall.transform.position;
}