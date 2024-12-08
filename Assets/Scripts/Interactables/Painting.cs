using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Painting : MonoBehaviour, IInteractable
{
    public void StartInteraction()
    {
        Debug.Log("[Painting] Starting Interaction...");
    }

    public void ExitInteraction()
    {
        Debug.Log("[Painting] Exiting Interaction...");
    }
}
