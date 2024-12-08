using UnityEngine;

public class InitialState : BaseState
{
    private float _timeSinceEnter;
    
    public InitialState(ObjectInteractionUtilityFunctions utilityFunctions) : base(utilityFunctions) {}
    
    public override void EnterState()
    {
        UtilityFunctions.ResetConstraintPosition();
    }

    public override void UpdateState()
    {
        _timeSinceEnter += Time.deltaTime;
    }

    public override void ExitState()
    {
        _timeSinceEnter = 0;
    }

    public override ObjectInteractionState GetNextState()
    {
        if (_timeSinceEnter >= 3)
            return ObjectInteractionState.Discovery;
        
        return ObjectInteractionState.Initial;
    }
}
