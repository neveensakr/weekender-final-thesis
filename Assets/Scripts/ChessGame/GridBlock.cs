using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridBlock : MonoBehaviour
{
    public ChessPiece CurrentChessPiece;
    private Material _originalMaterial;
    private Material _hoverMaterial;

    private void Awake()
    {
        _hoverMaterial = Resources.Load<Material>("Chess/ChessPiece_Hover_M");
    }

    public void Setup(Vector3 initialPosition, ChessPieceColor color)
    {
        transform.position = initialPosition;
        _originalMaterial = Resources.Load<Material>("Chess/ChessPiece_" + color + "_M");
        GetComponentInChildren<Renderer>().material = _originalMaterial;
    }

    public void EnableHover()
    {
        GetComponentInChildren<Renderer>().material = _hoverMaterial;
    }
    
    public void DisableHover()
    {
        GetComponentInChildren<Renderer>().material = _originalMaterial;
    }
}
