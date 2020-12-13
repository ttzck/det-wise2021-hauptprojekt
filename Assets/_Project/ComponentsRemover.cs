using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComponentsRemover : MonoBehaviour
{
    [SerializeField] private List<Component> componentsToRemove;
    [SerializeField] private RemovalCondition removalCondition;

    private void Start()
    {
        if (removalCondition == RemovalCondition.Client && BoltNetwork.IsClient
            || removalCondition == RemovalCondition.Server && BoltNetwork.IsServer)
        {
            foreach (var component in componentsToRemove)
            {
                Destroy(component);
            }
        }
    }

    private enum RemovalCondition
    { 
        Server = 1,
        Client = 2
    }
}