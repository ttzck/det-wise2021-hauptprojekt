using Bolt;
using System.Linq;
using UnityEngine;

public class GolfBallSelectionSystem : GameSystem
{
    private const float hitChargeSpeed = 1;

    private Camera cam;
    private IGameInput gameInput;
    private BoltEntity selectedGolfBall;

    public override void SetUp(IGameState gameState)
    {
        cam = Camera.main;
        gameInput = BoltNetwork.Instantiate(BoltPrefabs.Game_Input)
            .GetState<IGameInput>();
    }

    public override void Execute(IGameState _)
    {
        if (gameInput.SelectedGolfBall.IsZero)
        {
            if (Input.GetMouseButtonDown(0))
            {
                selectedGolfBall = FindNearestGolfBall();
                if (selectedGolfBall != null)
                    gameInput.SelectedGolfBall = selectedGolfBall.NetworkId;
            }
        }
        else
        {
            if (Input.GetMouseButtonDown(1))
            {
                ClearSelection();
                if(BoltNetwork.IsServer) 
                    selectedGolfBall.GetState<IGolfBallState>().PreMove = Vector3.zero;
            }
            else if (Input.GetMouseButtonDown(0))
            {
                HitSelectedGolfBall();
                ClearSelection();
            }
            else
            {
                gameInput.Direction = SelectedGolfBallToMouseVector.normalized;
                gameInput.Charge = Mathf.PingPong(Time.time * hitChargeSpeed, 1);
            }
        }
    }

    private BoltEntity FindNearestGolfBall()
    {
        return SystemUtils.FindEntitiesWith<IGolfBallState>()
            .Where(i => i.HasControl && i.GetState<IGolfBallState>().Alive)
            .OrderBy(i => Vector2.Distance(MousePosition, i.transform.position))
            .FirstOrDefault();
    }

    private void ClearSelection()
    {
        gameInput.SelectedGolfBall = new NetworkId();
        gameInput.Direction = Vector2.zero;
        gameInput.Charge = 0;
    }

    private void HitSelectedGolfBall()
    {
        var force = gameInput.Direction.normalized * gameInput.Charge;
        HitBoltEvent.Post(GlobalTargets.OnlyServer, force, gameInput.SelectedGolfBall);
    }

    private Vector2 MousePosition => cam.ScreenToWorldPoint(Input.mousePosition);

    private Vector2 SelectedGolfBallToMouseVector
        => MousePosition - (Vector2)BoltNetwork.FindEntity(gameInput.SelectedGolfBall).transform.position;
}
