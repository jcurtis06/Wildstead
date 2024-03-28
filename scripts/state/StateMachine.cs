namespace Wildstead.scripts.state;

public class StateMachine<T>
{
    public IState<T> CurrentState { get; private set; }
    
    private T _owner;
    
    public StateMachine(T owner)
    {
        _owner = owner;
    }
    
    public void TransitionTo(IState<T> state)
    {
        CurrentState?.Exit(_owner);
        CurrentState = state;
        CurrentState.Enter(_owner);
    }
    
    public void Process(double delta)
    {
        CurrentState?.Process(_owner, delta);
    }
}