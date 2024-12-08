using UnityEngine;

public class DiscoveryState : BaseState
{
    public DiscoveryState(WallInteractionUtilityFunctions utilityFunctions) : base(utilityFunctions) {}

    private float contactDistanceThreshold = 1.5f;
    
    public override void EnterState()
    {
        Debug.Log("[DiscoveryState] Entered Discovery State.");
    }

    public override void UpdateState()
    {
        Debug.Log("[DiscoveryState] Updating Discovery State.");
    }

    public override void ExitState()
    {
        Debug.Log("[DiscoveryState] Exiting Discovery State.");
    }

    public override WallInteractionState GetNextState()
    {
        Debug.Log("[DiscoveryState] Getting Next State...");
        if (UtilityFunctions.GetCurrentWallCollider() != null)
        {
            float distanceFromPlayerToWall = UtilityFunctions.GetDistanceToWall();
            if (distanceFromPlayerToWall < contactDistanceThreshold)
                return WallInteractionState.Reach;
        }
        
        return WallInteractionState.Discovery;
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
            UtilityFunctions.UpdateTargetPosition(other);
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
