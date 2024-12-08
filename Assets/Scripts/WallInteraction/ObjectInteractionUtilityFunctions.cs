using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class ObjectInteractionUtilityFunctions
{
    public Collider CurrentObjectCollider { get; set; }
    public IInteractable CurrentInteractable { get; set; }
    
    private TwoBoneIKConstraint _leftArmIKConstraint;
    private TwoBoneIKConstraint _rightArmIKConstraint;
    private MultiRotationConstraint _leftArmRotationConstraint;
    private MultiRotationConstraint _rightArmRotationConstraint;
    private CapsuleCollider _playerCollider;
    
    private TwoBoneIKConstraint _currentIKConstraint;
    private MultiRotationConstraint _currentRotationConstraint;
    private Vector3 _currentPointOfContact;
    private Vector3 _currentPointOnObject;
    private Vector3 _initialIKPosition;

    public ObjectInteractionUtilityFunctions(TwoBoneIKConstraint _leftArmIKConstraint,
        TwoBoneIKConstraint _rightArmIKConstraint,
        MultiRotationConstraint _leftArmRotationConstraint,
        MultiRotationConstraint _rightArmRotationConstraint,
        CapsuleCollider _playerCollider)
    {
        this._leftArmIKConstraint = _leftArmIKConstraint;
        this._rightArmIKConstraint = _rightArmIKConstraint;
        this._leftArmRotationConstraint = _leftArmRotationConstraint;
        this._rightArmRotationConstraint = _rightArmRotationConstraint;
        this._playerCollider = _playerCollider;
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
        }
        else
        {
            _currentIKConstraint = _rightArmIKConstraint;
            _currentRotationConstraint = _rightArmRotationConstraint;
        }
        _initialIKPosition = _currentIKConstraint.data.target.transform.localPosition;
    }

    public void UpdateTargetPosition(Collider other, float extentOfReach)
    {
        if (_currentIKConstraint == null)
            return;
        
        _currentPointOfContact = other.ClosestPoint(_playerCollider.transform.position);
        _currentPointOfContact.y += _playerCollider.height * extentOfReach;
        _currentPointOnObject = _currentPointOfContact;
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

    public float GetDistanceToObject()
    {
        return Vector3.Distance(_currentPointOnObject, _currentIKConstraint.data.root.position);
    }

    public void MoveTargetToPosition()
    {
        _currentIKConstraint.data.target.position = _currentPointOfContact;
    }
}
