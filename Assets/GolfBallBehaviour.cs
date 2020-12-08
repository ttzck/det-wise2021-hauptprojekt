using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GolfBallBehaviour : MonoBehaviour
{
    public event Action<Vector2> Hit;

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            var force = (Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position) * 100f;
            Hit?.Invoke(force);
        }
    }
}
