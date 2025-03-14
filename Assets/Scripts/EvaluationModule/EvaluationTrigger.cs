using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EvaluationTrigger : MonoBehaviour
{
    public String TriggerLabel = "";
    private bool _hasEntered = false;
    private bool _playerInTrigger = false;
    private float _timeSinceEnter = 0;
    private float _totalTimeInTrigger = 0;
    private int _timesEntered = 0;
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<EvaluationPlayer>() && !_playerInTrigger)
        {
            _timeSinceEnter = 0;
            _hasEntered = true;
            _playerInTrigger = true;
            _timesEntered++;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.GetComponent<EvaluationPlayer>())
        {
            _timeSinceEnter += Time.deltaTime;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<EvaluationPlayer>() && _playerInTrigger)
        {
            _totalTimeInTrigger += _timeSinceEnter;
            EvaluationModule.UpdateKey(TriggerLabel + "_triggered", true);
            EvaluationModule.UpdateKey(TriggerLabel + "_duration_in_area", _totalTimeInTrigger);
            EvaluationModule.UpdateKey(TriggerLabel + "_times_triggered", _timesEntered);
            _timeSinceEnter = 0;
            _playerInTrigger = false;
        }
    }

    public bool EnteredTrigger()
    {
        return _hasEntered;
    }
}
