using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WomanAnimationController : MonoBehaviour
{
    public static WomanAnimationController Instance;
    
    private Animator _animator;
    public bool isWalking { get; private set; }

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        _animator = GetComponent<Animator>();
    }
    
    public void StartWalking()
    {
        _animator.SetBool("isWalking", true);
        isWalking = true;
    }

    public void StopWalking()
    {
        _animator.SetBool("isWalking", false);
        isWalking = false;
    }
}
