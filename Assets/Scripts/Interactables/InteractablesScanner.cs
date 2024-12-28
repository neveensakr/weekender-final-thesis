using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractablesScanner : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        Interactable interactable = other.GetComponent<Interactable>();
        if (interactable != null && !InteractionManager.Instance.CurrentInteractable)
        {
            InteractionManager.Instance.SetInteractUIVisibility(true);
            InteractionManager.Instance.SetCurrentInteractable(interactable);
        }
    }
    
    private void OnTriggerExit(Collider other)
    {
        Interactable interactable = other.GetComponent<Interactable>();
        if (interactable != null)
        {
            InteractionManager.Instance.SetInteractUIVisibility(false);
            InteractionManager.Instance.SetCurrentInteractable(null);
        }
    }
}
