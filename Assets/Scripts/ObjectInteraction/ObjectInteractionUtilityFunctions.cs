using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class ObjectInteractionUtilityFunctions
{
    public Collider CurrentObjectCollider { get; set; }
    public IInteractable CurrentInteractable { get; set; }
    public Vector3 LeftTargetRotationDirection { get; set; }
    public Vector3 RightTargetRotationDirection { get; set; }
    public Vector3 LeftTargetContactOffSet { get; set; }
    public Vector3 RightTargetContactOffSet { get; set; }
    
    private TwoBoneIKConstraint _leftArmIKConstraint;
    private TwoBoneIKConstraint _rightArmIKConstraint;
    private MultiRotationConstraint _leftArmRotationConstraint;
    private MultiRotationConstraint _rightArmRotationConstraint;
    private CapsuleCollider _playerCollider;
    private float _rotationSpeed = 300f;
    
    private TwoBoneIKConstraint _currentIKConstraint;
    private MultiRotationConstraint _currentRotationConstraint;
    private Vector3 _currentTargetRotation;
    private Vector3 _currentTargetOffset;
    private Vector3 _currentPointOfContact;
    private Vector3 _initialIKPosition;
    private float _initialDistanceFromObj;

    public ObjectInteractionUtilityFunctions(TwoBoneIKConstraint leftArmIKConstraint,
        TwoBoneIKConstraint rightArmIKConstraint,
        MultiRotationConstraint leftArmRotationConstraint,
        MultiRotationConstraint rightArmRotationConstraint,
        CapsuleCollider playerCollider)
    {
        _leftArmIKConstraint = leftArmIKConstraint;
        _rightArmIKConstraint = rightArmIKConstraint;
        _leftArmRotationConstraint = leftArmRotationConstraint;
        _rightArmRotationConstraint = rightArmRotationConstraint;
        _playerCollider = playerCollider;
    }
    
    public void SetCurrentConstraints(Collider other)
    {
        Vector3 pointOfContact = other.ClosestPoint(_playerCollider.transform.position);
        Vector3 playerLeftShoulderPos = _leftArmIKConstraint.data.root.position;
        Vector3 playerRightShoulderPos = _rightArmIKConstraint.data.root.position;
        bool isLeftCloser = (playerLeftShoulderPos - pointOfContact).magnitude < (playerRightShoulderPos - pointOfContact).magnitude;
        if (isLeftCloser)
        {
            _currentIKConstraint = _leftArmIKConstraint;
            _currentRotationConstraint = _leftArmRotationConstraint;
            _currentTargetRotation = LeftTargetRotationDirection;
            _currentTargetOffset = LeftTargetContactOffSet;
        }
        else
        {
            _currentIKConstraint = _rightArmIKConstraint;
            _currentRotationConstraint = _rightArmRotationConstraint;
            _currentTargetRotation = RightTargetRotationDirection;
            _currentTargetOffset = RightTargetContactOffSet;
        }
        _initialIKPosition = _currentIKConstraint.data.target.transform.localPosition;
    }

    public void UpdateTargetPosition(Collider other)
    {
        if (_currentIKConstraint == null)
            return;
        
        _currentPointOfContact = other.ClosestPoint(_playerCollider.transform.position);
        _currentPointOfContact += _currentTargetOffset;
        Quaternion targetRotation = Quaternion.LookRotation(_currentTargetRotation, _currentIKConstraint.data.target.transform.forward);
        _currentIKConstraint.data.target.rotation =
            Quaternion.RotateTowards(_currentIKConstraint.data.target.rotation, targetRotation, _rotationSpeed * Time.deltaTime);
    }

    public void ResetConstraintPosition()
    {
        if (_currentIKConstraint)
            _currentIKConstraint.data.target.localPosition = _initialIKPosition;
        _currentRotationConstraint = null;
        _currentIKConstraint = null;
        CurrentObjectCollider = null;
        CurrentInteractable = null;
    }

    public void ResetConstraintWeight(float timeElapsed, float timeToSwitch = 2f)
    {
        _leftArmRotationConstraint.weight = Mathf.Lerp(_leftArmRotationConstraint.weight, 0, timeElapsed/timeToSwitch);
        _rightArmRotationConstraint.weight = Mathf.Lerp(_rightArmRotationConstraint.weight, 0, timeElapsed/timeToSwitch);
        _leftArmIKConstraint.weight = Mathf.Lerp(_leftArmIKConstraint.weight, 0, timeElapsed/timeToSwitch);
        _rightArmIKConstraint.weight = Mathf.Lerp(_rightArmIKConstraint.weight, 0, timeElapsed/timeToSwitch);
    }
    
    public void IncreaseConstraintWeight(float threshold, float timeElapsed, float timeToSwitch = 2f)
    {
        _currentIKConstraint.weight = Mathf.Lerp(_currentIKConstraint.weight, threshold, timeElapsed/timeToSwitch);
        _currentRotationConstraint.weight = threshold;
    }

    public float GetDistanceToObject()
    {
        return Vector3.Distance(_currentPointOfContact, _currentIKConstraint.data.root.position);
    }

    public void MoveTargetToPosition()
    {
        _currentIKConstraint.data.target.position = _currentPointOfContact;
    }

    public void SetInitialDistance()
    {
        _initialDistanceFromObj = GetDistanceToObject();
    }

    public bool IsWalkingAway()
    {
        return GetDistanceToObject() > _initialDistanceFromObj;
    }
}
