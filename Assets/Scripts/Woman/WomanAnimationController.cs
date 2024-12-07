using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WomanAnimationController : MonoBehaviour
{
    private Animator _animator;
    
    private void Start()
    {
        _animator = GetComponent<Animator>();
    }

    public void StopWalking()
    {
        _animator.SetBool("isWalking", false);
    }
}
