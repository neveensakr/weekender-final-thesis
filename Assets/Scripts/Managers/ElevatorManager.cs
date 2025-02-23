using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElevatorManager : MonoBehaviour
{
    [SerializeField] private GameObject targetKey;
    [SerializeField] private Material targetMaterial;

    public void EnableKey()
    {
        Renderer materialRenderer = targetKey.GetComponent<Renderer>();
        Material[] keyMaterials = materialRenderer.materials;
        keyMaterials[0] = targetMaterial;
        materialRenderer.materials = keyMaterials;
    }
}
