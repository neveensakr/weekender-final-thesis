using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridBlock : MonoBehaviour
{
    // Block materials for the different effects
    [SerializeField] private Material _hoverMaterial;
    [SerializeField] private Material _selectedMaterial;
    [SerializeField] private Material _potentialPositionMaterial;
    // The chess piece on this block
    public ChessPiece CurrentChessPiece;
    public Vector2 Position;
    // The original material is set based on the color
    private Material _originalMaterial;

    public void Setup(Vector3 initialPosition, ChessPieceColor color)
    {
        // Set the position and material based on the color
        transform.localPosition = initialPosition;
        Position = new Vector2(initialPosition.x, initialPosition.z);
        _originalMaterial = Resources.Load<Material>("Chess/ChessPiece_" + color + "_M");
        GetComponentInChildren<Renderer>().material = _originalMaterial;
    }

    public void AdjustEffect(GridBlockEffect effect)
    {
        // Set the material of the block based on the effect
        switch (effect)
        {
            case GridBlockEffect.Normal:
                GetComponentInChildren<Renderer>().material = _originalMaterial;
                break;
            case GridBlockEffect.Hover:
                GetComponentInChildren<Renderer>().material = _hoverMaterial;
                break;
            case GridBlockEffect.Selected:
                GetComponentInChildren<Renderer>().material = _selectedMaterial;
                break;
            case GridBlockEffect.PotentialPosition:
                GetComponentInChildren<Renderer>().material = _potentialPositionMaterial;
                break;
        }
    }
}
