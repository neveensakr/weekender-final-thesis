using System.Collections;
using UnityEngine;
using UnityEngine.Playables;

public class Elevator : Interactable
{
    [SerializeField] private PlayableDirector _sequence;
    [SerializeField] private Transform _targetPlayerTransform;
    
    public override void StartInteraction(GameObject player)
    {
        Debug.Log("[Elevator] Starting Interaction...");
        player.transform.position = new Vector3(_targetPlayerTransform.position.x, 
            player.transform.position.y, _targetPlayerTransform.position.z);
        _sequence.Play();
    }

    public override void ExitInteraction()
    {
        Debug.Log("[Elevator] Exiting Interaction...");
        if (IntroManager.Instance.CurrentGameMode == GameMode.InteractiveMode)
            onEnd.Invoke();
    }
}
