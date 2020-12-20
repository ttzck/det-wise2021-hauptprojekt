using UnityEngine;
using Bolt;
using System.Reflection;

public class DataMediator<T> : EntityEventListener<T> where T : IState
{
    [SerializeField] private bool attachTransform;

    public override void Attached()
    {
        PropertyInfo[] properties = typeof(T).GetProperties();
        foreach (var property in properties)
        {
            if (property.PropertyType == typeof(Transform))
                state.SetTransforms(property.GetValue(this) as NetworkTransform, transform);
            else
                state.AddCallback(property.Name, () => BroadcastChange(property.Name));
        }
    }

    private void BroadcastChange(string propertyName)
        => BroadcastMessage(
            methodName: $"On{propertyName}Changed",
            options: SendMessageOptions.DontRequireReceiver);

    private void OnCollisionEnter2D(Collision2D collision)
        => CollisionEvent.Publish(new CollisionArgs
        {
            A = gameObject,
            B = collision.gameObject
        });

    private void OnTriggerEnter2D(Collider2D other)
        => CollisionEvent.Publish(new CollisionArgs
        {
            A = gameObject,
            B = other.gameObject
        });
}
