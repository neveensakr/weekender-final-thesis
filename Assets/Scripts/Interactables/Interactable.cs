using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public abstract class Interactable : MonoBehaviour
{
    public UnityEvent onEnd = new UnityEvent();
    
    public abstract void StartInteraction(GameObject player);
    public abstract void ExitInteraction();
}
