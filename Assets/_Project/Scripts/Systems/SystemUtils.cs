using Bolt;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class SystemUtils
{
    public static IEnumerable<T> FindAll<T>() where T : IState
        => FindEntitiesWith<T>()
            .Select(j => j.GetState<T>());

    public static IEnumerable<BoltEntity> FindEntitiesWith<T>() where T : IState
        => BoltNetwork.Entities
            .Where(i => i.StateIs<T>());

    public static T Find<T>(NetworkId id)
        => BoltNetwork.FindEntity(id)
            .GetState<T>();

    public static bool TryGetState<T>(this GameObject gameObject, out T state)
    {
        if (gameObject.TryGetComponent(out BoltEntity entity) 
            && entity.TryFindState(out state))
            return true;
        state = default;
        return false;
    }

    public static bool IsOfKind<S, T>(this CollisionMessage message, out S stateA, out T stateB)
    {
        if (message.GameObjectA.TryGetState(out stateA) && message.GameObjectB.TryGetState(out stateB))
            return true;
        stateB = default;
        return false;
    }
}