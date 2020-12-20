using System;
using UnityEngine;

public abstract class GameEvent<T>
{
    public static event Action<T> Published;

    public static void Publish(T args)
    {
        Published?.Invoke(args);
    }
}

public class CollisionEvent : GameEvent<CollisionArgs> { }

public class CollisionArgs : EventArgs 
{
    public GameObject A;
    public GameObject B;
}