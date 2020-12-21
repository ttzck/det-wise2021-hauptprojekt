using System;
using System.Collections.Generic;

public static class GameEventManager
{
    private static readonly Dictionary<Type, Action<object>> subscribtions
        = new Dictionary<Type, Action<object>>();

    public static void Subscribe<T>(Action<object> action)
    {
        var type = typeof(T);

        if (subscribtions.ContainsKey(type))
        {
            subscribtions[type] += action;
        }
        else
        {
            subscribtions.Add(type, action);
        }
    }
    
    public static void Publish<T>(T message)
    {
        var type = typeof(T);

        if (subscribtions.ContainsKey(type))
        {
            subscribtions[type](message);
        }
    }

    public static void UnsubscribeAll() => subscribtions.Clear();
}