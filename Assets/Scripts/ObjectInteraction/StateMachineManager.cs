using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class StateMachineManager : MonoBehaviour
{
    [SerializeField] private TwoBoneIKConstraint _leftArmIKConstraint;
    [SerializeField] private TwoBoneIKConstraint _rightArmIKConstraint;
    [SerializeField] private MultiRotationConstraint _leftArmRotationConstraint;
    [SerializeField] private MultiRotationConstraint _rightArmRotationConstraint;
    [SerializeField] private CapsuleCollider _playerCollider;
    [SerializeField] private Vector3 _leftTargetRotation = -Vector3.up;
    [SerializeField] private Vector3 _leftContactOffSet = Vector3.zero;
    [SerializeField] private Vector3 _rightTargetRotation = -Vector3.up;
    [SerializeField] private Vector3 _rightContactOffSet = Vector3.zero;

    private Dictionary<ObjectInteractionState, BaseState> _states;
    private ObjectInteractionUtilityFunctions _utilityFunctions;
    private BaseState _currentState;
    
    void Start()
    {
        _utilityFunctions = new ObjectInteractionUtilityFunctions(_leftArmIKConstraint, _rightArmIKConstraint, 
            _leftArmRotationConstraint, _rightArmRotationConstraint, _playerCollider);
        
        InitializeStates();
        _currentState = _states[ObjectInteractionState.Initial];
        Debug.Log("Entering " + _currentState);
        _currentState.EnterState();
    }
    
    void Update()
    {
        _utilityFunctions.LeftTargetRotationDirection = _leftTargetRotation;
        _utilityFunctions.LeftTargetContactOffSet = _leftContactOffSet;
        _utilityFunctions.RightTargetRotationDirection = _rightTargetRotation;
        _utilityFunctions.RightTargetContactOffSet = _rightContactOffSet;
        
        Debug.Log("Updating " + _currentState);
        _currentState.UpdateState();
        BaseState nextState = _states[_currentState.GetNextState()];
        
        if (nextState != _currentState)
            ChangeStates(nextState);
    }

    private void InitializeStates()
    {
        _states = new Dictionary<ObjectInteractionState, BaseState>();
        _states.Add(ObjectInteractionState.Initial, new InitialState(_utilityFunctions));
        _states.Add(ObjectInteractionState.Discovery, new DiscoveryState(_utilityFunctions));
        _states.Add(ObjectInteractionState.Reach, new ReachState(_utilityFunctions));
        _states.Add(ObjectInteractionState.Contact, new ContactState(_utilityFunctions));
    }
    
    private void ChangeStates(BaseState nextState)
    {
        Debug.Log("Exiting " + _currentState);
        _currentState.ExitState();
        _currentState = nextState;
        Debug.Log("Entering " + _currentState);
        _currentState.EnterState();
    }
    
    private void OnTriggerEnter(Collider other)
    {
        _currentState.OnTriggerEnter(other);
    }

    private void OnTriggerStay(Collider other)
    {
        _currentState.OnTriggerStay(other);
    }

    private void OnTriggerExit(Collider other)
    {
        _currentState.OnTriggerExit(other);

        if (other == _utilityFunctions.CurrentObjectCollider)
        {
            _utilityFunctions.ResetConstraintPosition();
            ChangeStates(_states[ObjectInteractionState.Initial]);
        }
    }
}
