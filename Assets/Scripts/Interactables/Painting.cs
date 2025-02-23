using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class Painting : Interactable
{
    [SerializeField] private Animator[] _butterflies;
    [SerializeField] private PlayableDirector _sequence;
    [SerializeField] private Transform _targetPlayerTransform;
    
    public override void StartInteraction(GameObject player)
    {
        Debug.Log("[Painting] Starting Interaction...");
        player.transform.position = new Vector3(_targetPlayerTransform.position.x, 
            player.transform.position.y, _targetPlayerTransform.position.z);
        _sequence.Play();
    }

    public override void ExitInteraction()
    {
        Debug.Log("[Painting] Exiting Interaction...");
        if (IntroManager.Instance.CurrentGameMode == GameMode.InteractiveMode)
            onEnd.Invoke();
    }

    public void StartFlyingEffect()
    {
        foreach (Animator butterfly in _butterflies)
        {
            butterfly.SetBool("StartFlying", true);
        }
    }
}
