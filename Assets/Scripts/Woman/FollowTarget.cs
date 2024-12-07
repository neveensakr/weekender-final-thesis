using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class FollowTarget : MonoBehaviour
{
    [SerializeField] private Transform _target;

    [ExecuteInEditMode]
    private void Update()
    {
        transform.position = _target.position;
    }
}
