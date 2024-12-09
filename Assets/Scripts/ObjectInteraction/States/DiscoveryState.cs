using UnityEngine;

public class DiscoveryState : BaseState
{
    public DiscoveryState(ObjectInteractionUtilityFunctions utilityFunctions) : base(utilityFunctions) {}

    private float _reachDistanceThreshold = 1.5f;

    public override ObjectInteractionState GetNextState()
    {
        if (UtilityFunctions.CurrentObjectCollider != null)
        {
            float distanceFromPlayerToObj = UtilityFunctions.GetDistanceToObject();
            if (distanceFromPlayerToObj < _reachDistanceThreshold)
                return ObjectInteractionState.Reach;
        }
        
        return ObjectInteractionState.Discovery;
    }
    
    public override void OnTriggerEnter(Collider other)
    {
        IInteractable interactable = other.GetComponent<IInteractable>();
        if (interactable != null && (UtilityFunctions.CurrentObjectCollider == null))
        {
            UtilityFunctions.CurrentInteractable = interactable;
            UtilityFunctions.CurrentObjectCollider = other;
            UtilityFunctions.SetCurrentConstraints(other);
        }
    }
    
    public override void OnTriggerStay(Collider other)
    {
        if (other == UtilityFunctions.CurrentObjectCollider)
            UtilityFunctions.UpdateTargetPosition(other);
    }
}
