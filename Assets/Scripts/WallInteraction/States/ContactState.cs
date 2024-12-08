using UnityEngine;

public class ContactState : BaseState
{
    public ContactState(ObjectInteractionUtilityFunctions utilityFunctions) : base(utilityFunctions) {}

    private float _contactTimeElapsed;
    private float _maxContactTime = 8f;

    public override void EnterState()
    {
        UtilityFunctions.CurrentInteractable.StartInteraction();
    }

    public override void UpdateState()
    {
        _contactTimeElapsed += Time.deltaTime;
    }

    public override void ExitState()
    {
        _contactTimeElapsed = 0f;
    }

    public override ObjectInteractionState GetNextState()
    {
        if (_contactTimeElapsed > _maxContactTime)
            return ObjectInteractionState.Initial;
        
        return ObjectInteractionState.Contact;
    }
    
    public override void OnTriggerStay(Collider other)
    {
        if (other == UtilityFunctions.CurrentObjectCollider)
        {
            UtilityFunctions.UpdateTargetPosition(other, 0.5f);
            UtilityFunctions.MoveTargetToPosition();
        }
    }

    public override void OnTriggerExit(Collider other)
    {
        UtilityFunctions.CurrentInteractable.ExitInteraction();
    }
}
