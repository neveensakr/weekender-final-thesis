using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.VFX;

public class SnowGlobe : Interactable
{
    [SerializeField] private VisualEffect _snowEffect;
    [SerializeField] private PlayableDirector _sequence;
    [SerializeField] private Transform _targetPlayerTransform;

    private void Start()
    {
        _snowEffect.Stop();
    }

    public override void StartInteraction(GameObject player)
    {
        Debug.Log("[SnowGlobe] Starting Interaction...");
        player.transform.position = new Vector3(_targetPlayerTransform.position.x, 
            player.transform.position.y, _targetPlayerTransform.position.z);
        _sequence.Play();
    }

    public override void ExitInteraction()
    {
        Debug.Log("[SnowGlobe] Exiting Interaction...");
        onEnd.Invoke();
        _snowEffect.Stop();
    }

    public void StartSnow()
    {
        _snowEffect.Play();
    }
}
