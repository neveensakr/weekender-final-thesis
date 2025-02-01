using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Painting : Interactable
{
    [SerializeField] private Animator[] _butterflies;
    
    public override void StartInteraction(GameObject player)
    {
        Debug.Log("[Painting] Starting Interaction...");
        foreach (Animator butterfly in _butterflies)
        {
            butterfly.SetBool("StartFlying", true);
        }
    }

    public override void ExitInteraction()
    {
        Debug.Log("[Painting] Exiting Interaction...");
    }
}
