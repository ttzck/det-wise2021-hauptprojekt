using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GolfBallBehaviour : MonoBehaviour
{
    public event Action<Vector2> Hit;

    public bool Ready { get; set; }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0) && Ready)
        {
            var force = (Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position) * 100f;
            Hit?.Invoke(force);
        }
    }
}
