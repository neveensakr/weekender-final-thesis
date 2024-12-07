using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class Movement : MonoBehaviour
{
    [SerializeField] private float _speed = 10f;
    [SerializeField] private Transform _camera;
    private Rigidbody _rigidbody;
    private Animator _animator;
    private bool _movementEnabled = false;
    
    void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _animator = GetComponent<Animator>();
    }

    public void EnablePlayerMovement()
    {
        GetComponent<FollowTarget>().enabled = false;
        _movementEnabled = true;
        GetComponent<RigBuilder>().layers[0].active = false;
    }

    void Update()
    {
        if (_movementEnabled)
        {
            if (_rigidbody.velocity != Vector3.zero)
            {
                _animator.SetBool("isWalking", true);
            }
            else
            {
                _animator.SetBool("isWalking", false);
            }
        }
    }

    private void FixedUpdate()
    {
        if (_movementEnabled)
        {
            float verticalMovement = Input.GetAxisRaw("Vertical");
            
            if (verticalMovement >= 0.1f)
            {
                float angleToRotateTo = verticalMovement + _camera.eulerAngles.y;
                transform.rotation = Quaternion.Euler(0f, angleToRotateTo, 0f);
                Vector3 moveDirection = (Quaternion.Euler(0f, angleToRotateTo, 0f) * Vector3.forward).normalized;
                _rigidbody.AddForce(moveDirection * (_speed * Time.deltaTime), ForceMode.Impulse);
            }
            else
            {
                _rigidbody.velocity = Vector3.zero;
            }
        }
    }
}
