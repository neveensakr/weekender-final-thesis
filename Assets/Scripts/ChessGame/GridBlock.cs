using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridBlock : MonoBehaviour
{
    public void Setup(Vector3 initialPosition, ChessPieceColor color)
    {
        transform.position = initialPosition;
        GetComponentInChildren<Renderer>().material = Resources.Load<Material>("Chess/ChessPiece_" + color + "_M");
    }
}
