using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[RequireComponent(typeof(Collider2D))]
public class ColliderNotifier : MonoBehaviour
{
    public static event Action<GameObject, GameObject> CollisionEntered;
    public static event Action<GameObject, GameObject> TriggerEntered;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        TriggerEntered?.Invoke(gameObject, collision.gameObject);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        CollisionEntered?.Invoke(gameObject, collision.gameObject);
    }
}