using UnityEngine;

public class InitialState : BaseState
{
    private float _timeSinceEnter;
    
    public InitialState(WallInteractionUtilityFunctions utilityFunctions) : base(utilityFunctions) {}
    
    public override void EnterState()
    {
        Debug.Log("[InitialState] Entered Initial State.");
        UtilityFunctions.ResetConstraintPosition();
    }

    public override void UpdateState()
    {
        Debug.Log("[InitialState] Updating Initial State.");
        _timeSinceEnter += Time.deltaTime;
    }

    public override void ExitState()
    {
        Debug.Log("[InitialState] Exiting Initial State.");
        _timeSinceEnter = 0;
    }

    public override WallInteractionState GetNextState()
    {
        Debug.Log("[InitialState] Getting Next State...");
        if (_timeSinceEnter >= 3)
        {
            return WallInteractionState.Discovery;
        }
        
        return WallInteractionState.Initial;
    }
}
