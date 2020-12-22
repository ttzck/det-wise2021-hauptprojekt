using System;
using System.Collections.Generic;
using System.Linq;

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
        var messageType = typeof(T);
        var subsCopy = subscribtions
            .Select(kvp => (kvp.Key, kvp.Value))
            .ToArray();

        foreach (var (type, action) in subsCopy)
        {
            if (type.IsAssignableFrom(messageType))
            {
                action(message);
            }
        }
    }

    public static void UnsubscribeAll() => subscribtions.Clear();
}