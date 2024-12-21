using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ChessPiece : MonoBehaviour
{
    public void Setup(Vector3 initialPosition, ChessPieceColor color)
    {
        transform.position = initialPosition;
        GetComponentInChildren<Renderer>().material = Resources.Load<Material>("Chess/ChessPiece_" + color + "_M");
    }

    public abstract void MovePiece();

    public void KillPiece()
    {
        Destroy(gameObject);
    }
}
