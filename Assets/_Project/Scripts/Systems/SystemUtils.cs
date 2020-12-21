﻿using System.Collections.Generic;
using System.Linq;
using Bolt;

public static class SystemUtils
{
    public static IEnumerable<T> FindAll<T>() where T : IState
    {
        return BoltNetwork.Entities
            .Where(i => i.StateIs<T>())
            .Select(j => j.GetState<T>());
    }
}