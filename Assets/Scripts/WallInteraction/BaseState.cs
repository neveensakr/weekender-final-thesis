using UnityEngine;

public abstract class BaseState
{
    public ObjectInteractionUtilityFunctions UtilityFunctions;
    public BaseState(ObjectInteractionUtilityFunctions utilityFunctions)
    {
        this.UtilityFunctions = utilityFunctions;
    }

    public virtual void EnterState() {}
    public virtual void UpdateState() {}
    public virtual void ExitState() {}
    public abstract ObjectInteractionState GetNextState();
    
    public virtual void OnTriggerEnter(Collider other) {}
    public virtual void OnTriggerStay(Collider other) {}
    public virtual void OnTriggerExit(Collider other) {}
}
