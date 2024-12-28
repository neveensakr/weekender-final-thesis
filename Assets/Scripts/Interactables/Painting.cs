using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Painting : Interactable
{
    public override void StartInteraction(GameObject player)
    {
        Debug.Log("[Painting] Starting Interaction...");
    }

    public override void ExitInteraction()
    {
        Debug.Log("[Painting] Exiting Interaction...");
    }
}
