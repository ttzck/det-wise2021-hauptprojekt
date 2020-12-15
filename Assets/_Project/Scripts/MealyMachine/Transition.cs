using System;

public class Transition
{
    public object From;
    public object To;
    public Func<bool> Condition;
    public Action Output;
}

