public class StateMachine
{
    private BaseState currentState;

    public void Initialize(BaseState startState)
    {
        currentState = startState;
        currentState.EnterState();
    }

    public void ChangeStates(BaseState nextState)
    {
        currentState.ExitState();
        currentState = nextState;
        currentState.EnterState();
    }
}
