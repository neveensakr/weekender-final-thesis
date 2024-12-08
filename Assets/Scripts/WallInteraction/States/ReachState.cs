using UnityEngine;

public class ReachState : BaseState
{
    public ReachState(WallInteractionUtilityFunctions utilityFunctions) : base(utilityFunctions) {}
    
    private float ascendDistanceThreshold = 1.3f;
    private float _timeSinceEnter;
    
    public override void EnterState()
    {
        Debug.Log("[ReachState] Entered Reach State.");
    }

    public override void UpdateState()
    {
        Debug.Log("[ReachState] Updating Reach State.");
        _timeSinceEnter += Time.deltaTime;
    }

    public override void ExitState()
    {
        Debug.Log("[ReachState] Exiting Reach State.");
        _timeSinceEnter = 0f;
    }

    public override WallInteractionState GetNextState()
    {
        if (UtilityFunctions.GetCurrentWallCollider() != null)
        {
            float distanceFromPlayerToWall = UtilityFunctions.GetDistanceToWall();
            if (distanceFromPlayerToWall < ascendDistanceThreshold)
            {
                return WallInteractionState.Ascend;
            }
            
            if (_timeSinceEnter > 3f)
            {
                return WallInteractionState.Initial;
            }
        }
        
        return WallInteractionState.Reach;
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
            UtilityFunctions.UpdateTargetPosition(other, 0.5f);
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
