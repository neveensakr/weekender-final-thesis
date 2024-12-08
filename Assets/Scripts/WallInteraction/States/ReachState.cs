using UnityEngine;

public class ReachState : BaseState
{
    public ReachState(ObjectInteractionUtilityFunctions utilityFunctions) : base(utilityFunctions) {}
    
    private float _contactDistanceThreshold = 1.3f;
    private float _timeSinceEnter;
    
    public override void UpdateState()
    {
        _timeSinceEnter += Time.deltaTime;
    }

    public override void ExitState()
    {
        _timeSinceEnter = 0f;
    }

    public override ObjectInteractionState GetNextState()
    {
        if (UtilityFunctions.CurrentObjectCollider != null)
        {
            float distanceFromPlayerToObj = UtilityFunctions.GetDistanceToObject();
            if (distanceFromPlayerToObj < _contactDistanceThreshold)
                return ObjectInteractionState.Contact;
            
            if (_timeSinceEnter > 3f)
                return ObjectInteractionState.Initial;
        }
        
        return ObjectInteractionState.Reach;
    }
    
    public override void OnTriggerStay(Collider other)
    {
        if (other == UtilityFunctions.CurrentObjectCollider)
        {
            UtilityFunctions.UpdateTargetPosition(other, 0.3f);
            UtilityFunctions.MoveTargetToPosition();
        }
    }
}
