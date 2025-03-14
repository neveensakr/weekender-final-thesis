using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EvaluationTrigger : MonoBehaviour
{
    public String TriggerLabel = "";
    private bool _hasEnterd = false;
    private bool _playerInTrigger = false;
    private float _timeSinceEnter = 0;
    private float _totalTimeInTrigger = 0;
    private int _timesEntered = 0;
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<EvaluationPlayer>() && !_playerInTrigger)
        {
            _timeSinceEnter = 0;
            _hasEnterd = true;
            _playerInTrigger = true;
            _timesEntered++;
            EvaluationModule.LogEntry("[EvaluationTrigger - " + TriggerLabel + "] Entered At " + Time.realtimeSinceStartup);
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
            EvaluationModule.LogEntry("[EvaluationTrigger - " + TriggerLabel + "] Exited At " +
                                      Time.realtimeSinceStartup);
            _timeSinceEnter = 0;
            _playerInTrigger = false;
        }
    }

    public bool EnteredTrigger()
    {
        return _hasEnterd;
    }
}
