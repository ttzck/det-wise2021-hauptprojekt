using System.Collections.Generic;
using System.Linq;
using Bolt;

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
}