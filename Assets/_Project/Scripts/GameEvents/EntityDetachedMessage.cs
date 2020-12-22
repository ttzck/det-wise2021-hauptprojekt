using Bolt;

public class EntityDetachedMessage<T> where T : IState
{
    public T State;
}
