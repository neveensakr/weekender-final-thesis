using UnityEngine;

public class ContactState : BaseState
{
    public ContactState(ObjectInteractionUtilityFunctions utilityFunctions) : base(utilityFunctions) {}

    private float _contactTimeElapsed;
    private float _maxContactTime = 80f;

    public override void EnterState()
    {
        UtilityFunctions.CurrentInteractable.StartInteraction();
    }

    public override void UpdateState()
    {
        _contactTimeElapsed += Time.deltaTime;
        UtilityFunctions.IncreaseConstraintWeight(1, _contactTimeElapsed);
    }

    public override void ExitState()
    {
        _contactTimeElapsed = 0f;
    }

    public override ObjectInteractionState GetNextState()
    {
        if (_contactTimeElapsed > _maxContactTime)
            return ObjectInteractionState.Initial;
        
        if (UtilityFunctions.CurrentObjectCollider && UtilityFunctions.IsWalkingAway())
        {
            return ObjectInteractionState.Reach;
        }
        
        return ObjectInteractionState.Contact;
    }
    
    public override void OnTriggerStay(Collider other)
    {
        if (other == UtilityFunctions.CurrentObjectCollider)
        {
            UtilityFunctions.UpdateTargetPosition(other);
            UtilityFunctions.MoveTargetToPosition();
        }
    }

    public override void OnTriggerExit(Collider other)
    {
        UtilityFunctions.CurrentInteractable.ExitInteraction();
    }
}
