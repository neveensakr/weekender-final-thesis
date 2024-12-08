using UnityEngine;

public class AscendState : BaseState
{
    public AscendState(WallInteractionUtilityFunctions utilityFunctions) : base(utilityFunctions) {}

    private float touchTimeElapsed;
    
    public override void EnterState()
    {
        Debug.Log("[AscendState] Entered Ascend State.");
    }

    public override void UpdateState()
    {
        Debug.Log("[AscendState] Updating Ascend State.");
        touchTimeElapsed += Time.deltaTime;
    }

    public override void ExitState()
    {
        Debug.Log("[AscendState] Exiting Ascend State.");
        touchTimeElapsed = 0f;
    }

    public override WallInteractionState GetNextState()
    {
        if (touchTimeElapsed > 8f)
        {
            return WallInteractionState.Initial;
        }
        
        return WallInteractionState.Ascend;
    }
    
    public override void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<Wall>() && (UtilityFunctions.GetCurrentWallCollider() == null))
        {
            UtilityFunctions.SetCurrentWallCollider(other);
            UtilityFunctions.SetCurrentConstraints(other);
        }
    }
    
    public override void OnTriggerStay(Collider other)
    {
        if (other == UtilityFunctions.GetCurrentWallCollider())
        {
            UtilityFunctions.UpdateTargetPosition(other, 0.7f);
            UtilityFunctions.MoveTargetToPosition();
        }
    }
    
    public override void OnTriggerExit(Collider other)
    {
        if (other == UtilityFunctions.GetCurrentWallCollider())
        {
            UtilityFunctions.ResetConstraintPosition();
        }
    }
}
