using UnityEngine;

public abstract class BaseState
{
    public WallInteractionUtilityFunctions UtilityFunctions;
    public BaseState(WallInteractionUtilityFunctions utilityFunctions)
    {
        this.UtilityFunctions = utilityFunctions;
    }

    public abstract void EnterState();
    public abstract void UpdateState();
    public abstract void ExitState();
    public abstract WallInteractionState GetNextState();
    
    public virtual void OnTriggerEnter(Collider other) {}
    public virtual void OnTriggerStay(Collider other) {}
    public virtual void OnTriggerExit(Collider other) {}
}
