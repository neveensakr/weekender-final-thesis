using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GridBlock : MonoBehaviour
{
    public ChessPiece CurrentChessPiece;
    private Material _originalMaterial;
    [SerializeField] private Material _hoverMaterial;
    [SerializeField] private Material _selectedMaterial;
    [SerializeField] private Material _potentialPositionMaterial;
    [SerializeField] private TextMeshProUGUI _idText;

    public void Setup(int index, Vector3 initialPosition, ChessPieceColor color)
    {
        transform.position = initialPosition;
        _originalMaterial = Resources.Load<Material>("Chess/ChessPiece_" + color + "_M");
        GetComponentInChildren<Renderer>().material = _originalMaterial;
        _idText.text = initialPosition.x + ", " + initialPosition.z + ": i = " + index;
    }

    public void AdjustEffect(GridBlockEffect effect)
    {
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
