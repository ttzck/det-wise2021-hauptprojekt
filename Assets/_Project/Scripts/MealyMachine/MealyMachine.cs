using System.Collections;
using System.Collections.Generic;
using System;

public class MealyMachine
{
    public object State { get; private set; }

    private List<Transition> transitions
        = new List<Transition>();

    public MealyMachine(object startState)
    {
        State = startState;
    }

    public void AddTransition(object from, object to, Func<bool> condition, Action output)
    {
        transitions.Add(
            new Transition
            {
                From = from,
                To = to,
                Condition = condition,
                Output = output
            }
        );
    }

    public void AddTransition(Transition transition)
    {
        transitions.Add(transition);
    }

    public void Update()
    {
        foreach (var transition in transitions)
        {
            if (State == transition.From && transition.Condition())
            {
                transition.Output();
                State = transition.To;
                return;
            }
        }
    }

    public static object NewState => new object();
}
