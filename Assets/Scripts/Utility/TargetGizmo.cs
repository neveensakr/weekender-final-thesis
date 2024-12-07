using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetGizmo : MonoBehaviour
{
    public float Radius = 1;
    void OnDrawGizmos()
    {
        Gizmos.color = new Color(255, 0, 0, 0.5f);
        Gizmos.DrawSphere(transform.position, Radius);
    }
}
