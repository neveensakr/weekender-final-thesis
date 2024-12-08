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

    private Dictionary<WallInteractionState, BaseState> _states;
    private WallInteractionUtilityFunctions _utilityFunctions;
    private BaseState _currentState;
    private BoxCollider _wallDetector;
    
    void Start()
    {
        _utilityFunctions = new WallInteractionUtilityFunctions(_leftArmIKConstraint, _rightArmIKConstraint, 
            _leftArmRotationConstraint, _rightArmRotationConstraint, _playerCollider);
        
        InitializeStates();
        _currentState = _states[WallInteractionState.Initial];
        _currentState.EnterState();
        
        AddWallDetector();
    }
    
    void Update()
    {
        UpdateWallDetectorPosition();
        _currentState.UpdateState();
        BaseState nextState = _states[_currentState.GetNextState()];
        
        if (nextState != _currentState)
            ChangeStates(nextState);
    }

    private void InitializeStates()
    {
        _states = new Dictionary<WallInteractionState, BaseState>();
        _states.Add(WallInteractionState.Initial, new InitialState(_utilityFunctions));
        _states.Add(WallInteractionState.Discovery, new DiscoveryState(_utilityFunctions));
        _states.Add(WallInteractionState.Reach, new ReachState(_utilityFunctions));
        _states.Add(WallInteractionState.Ascend, new AscendState(_utilityFunctions));
        _states.Add(WallInteractionState.Contact, new ContactState(_utilityFunctions));
    }
    
    private void ChangeStates(BaseState nextState)
    {
        _currentState.ExitState();
        _currentState = nextState;
        _currentState.EnterState();
    }

    private void AddWallDetector()
    {
        _wallDetector = gameObject.AddComponent<BoxCollider>();
        _wallDetector.isTrigger = true;
    }

    private void UpdateWallDetectorPosition()
    {
        if (_wallDetector == null)
            AddWallDetector();
        
        _wallDetector.size = new Vector3(_playerCollider.height, _playerCollider.height, _playerCollider.height);
        _wallDetector.center = new Vector3(_playerCollider.transform.position.x, 
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
        ChangeStates(_states[WallInteractionState.Initial]);
    }
}
