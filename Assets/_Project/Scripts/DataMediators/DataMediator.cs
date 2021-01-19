using Bolt;
using System.Reflection;
using UnityEngine;

/// <summary>
/// A DataMediator is meant to bridge the gap between BoltEntities, GameEvents and arbitrary Components of the GameObject.
///
/// </summary>
/// <remarks>
/// It broadcasts a message of the form <c>OnXChanged</c> each time a property of the State changes, 
/// publishes a CollisionEvent for collisions between BoltEntities (including TriggerEnter),
/// and removes all physic components that should only run on the server.
/// </remarks>
/// <typeparam name="T"></typeparam>
public class DataMediator<T> : EntityBehaviour<T> where T : IState
{
    [SerializeField] private bool attachTransform;

    public override void Attached()
    {
        GameEventManager.Publish(new EntityAttachedMessage<T> { State = state });

        AddCallbacks();

        if (!BoltNetwork.IsServer)
        {
            RemoveComponent<Rigidbody2D>();
            RemoveComponent<Collider2D>();
        }
    }

    public override void Detached()
    {
        GameEventManager.Publish(new EntityDetachedMessage<T> { State = state });
    }

    private void AddCallbacks()
    {
        PropertyInfo[] properties = typeof(T).GetProperties();
        foreach (var property in properties)
        {
            if (property.PropertyType == typeof(NetworkTransform))
                state.SetTransforms(property.GetValue(state) as NetworkTransform, transform);
            else
                state.AddCallback(property.Name, () => BroadcastChange(property.Name));
        }
    }

    private void RemoveComponent<C>() where C : Component
    {
        Destroy(GetComponent<C>());
    }

    private void BroadcastChange(string propertyName)
        => BroadcastMessage(
            methodName: $"On{propertyName}Changed",
            options: SendMessageOptions.DontRequireReceiver);

    private void OnCollisionEnter2D(Collision2D collision)
        => PublishColllision(collision.gameObject);

    private void OnTriggerEnter2D(Collider2D other)
        => PublishColllision(other.gameObject);

    private void PublishColllision(GameObject other)
    {
        GameEventManager.Publish(new CollisionMessage
        {
            GameObjectA = gameObject,
            GameObjectB = other
        });
    }
}
