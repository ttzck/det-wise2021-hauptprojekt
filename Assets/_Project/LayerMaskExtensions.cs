using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public static class LayerMaskExtensions
{ 
    public static bool Contains(this LayerMask layerMask, int layer)
    {
        return (1 << layer & layerMask) > 0;
    }
}
