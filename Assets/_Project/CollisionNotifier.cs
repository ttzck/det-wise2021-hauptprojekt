using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[RequireComponent(typeof(Collider2D))]
public class CollisionNotifier : MonoBehaviour
{
    public static event Action<GameObject, GameObject> Collision;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Collision?.Invoke(gameObject, collision.gameObject);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Collision?.Invoke(gameObject, collision.gameObject);
    }
}