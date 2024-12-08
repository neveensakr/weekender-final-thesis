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

    private Dictionary<ObjectInteractionState, BaseState> _states;
    private ObjectInteractionUtilityFunctions _utilityFunctions;
    private BaseState _currentState;
    private BoxCollider _interactableObjDetector;
    
    void Start()
    {
        _utilityFunctions = new ObjectInteractionUtilityFunctions(_leftArmIKConstraint, _rightArmIKConstraint, 
            _leftArmRotationConstraint, _rightArmRotationConstraint, _playerCollider);
        
        InitializeStates();
        _currentState = _states[ObjectInteractionState.Initial];
        Debug.Log("Entering " + _currentState);
        _currentState.EnterState();
        
        AddInteractableObjDetector();
    }
    
    void Update()
    {
        UpdateInteractableObjDetectorPosition();
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

    private void AddInteractableObjDetector()
    {
        _interactableObjDetector = gameObject.AddComponent<BoxCollider>();
        _interactableObjDetector.isTrigger = true;
    }

    private void UpdateInteractableObjDetectorPosition()
    {
        if (_interactableObjDetector == null)
            AddInteractableObjDetector();
        
        _interactableObjDetector.size = new Vector3(_playerCollider.height, _playerCollider.height, _playerCollider.height);
        _interactableObjDetector.center = new Vector3(_playerCollider.transform.position.x, 
            _playerCollider.transform.position.y + _playerCollider.height * 3/4, 
            _playerCollider.transform.position.z + _playerCollider.height / 2);
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
