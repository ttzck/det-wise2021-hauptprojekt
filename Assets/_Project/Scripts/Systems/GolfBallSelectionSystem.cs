using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Bolt;
using System.Linq;


public class GolfBallSelectionSystem : MonoBehaviour
{
    [SerializeField] private SelectionIndicator selectionIndicatorPrefab;
    [SerializeField] private float hitChargeSpeed;

    public BoltEntity SelectedGolfBall { get; private set; }
    public float HitCharge { get; private set; }
    public Vector2 HitDirection { get; private set; }
    public object NoneSelected { get; private set; } = MealyMachine.NewState;
    public object GolfBallSelected { get; private set; } = MealyMachine.NewState;
    public MealyMachine SelectionMachine { get; private set; }

    private Camera cam;
    private SelectionIndicator indicator;

    private void Start()
    {
        cam = Camera.main;
        indicator = Instantiate(selectionIndicatorPrefab);
        indicator.Owner = this;

        CreateSelectionMachine();
    }

    private void Update() => SelectionMachine.Update();

    private void CreateSelectionMachine()
    {
        SelectionMachine = new MealyMachine(startState: NoneSelected);

        SelectionMachine.AddTransition(
            from: NoneSelected,
            to: GolfBallSelected,
            condition: () => Input.GetMouseButtonDown(0),
            output: () =>
            {
                SelectedGolfBall = FindNearestGolfBall();
                HitDirection = SelectedGolfBallToMouseVector.normalized;
            });

        SelectionMachine.AddTransition(
            from: NoneSelected,
            to: NoneSelected,
            condition: () => true,
            output: () => { });

        SelectionMachine.AddTransition(
            from: GolfBallSelected,
            to: NoneSelected,
            condition: () => Input.GetMouseButtonDown(1),
            output: () => ClearSelection());

        SelectionMachine.AddTransition(
            from: GolfBallSelected,
            to: NoneSelected,
            condition: () => Input.GetMouseButtonDown(0),
            output: () =>
            {
                HitSelectedGolfBall();
                ClearSelection();
            });

        SelectionMachine.AddTransition(
            from: GolfBallSelected,
            to: GolfBallSelected,
            condition: () => true,
            output: () =>
            {
                HitDirection = SelectedGolfBallToMouseVector.normalized;
                HitCharge = Mathf.PingPong(Time.time * hitChargeSpeed, 1);
            });
    }

    private void ClearSelection()
    {
        SelectedGolfBall = null;
        HitDirection = Vector2.zero;
        HitCharge = 0;
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
        var force = HitDirection.normalized * HitCharge;
        HitEvent.Post(GlobalTargets.OnlyServer, force, SelectedGolfBall.NetworkId);
    }

    private Vector2 MousePosition => cam.ScreenToWorldPoint(Input.mousePosition);

    private Vector2 SelectedGolfBallToMouseVector
        => MousePosition - (Vector2)SelectedGolfBall.transform.position;
}
