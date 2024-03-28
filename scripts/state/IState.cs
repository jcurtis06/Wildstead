namespace Wildstead.scripts.state;

public interface IState<T>
{
    void Enter(T owner);
    void Process(T owner, double delta);
    void Exit(T owner);
}