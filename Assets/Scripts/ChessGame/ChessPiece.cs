using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ChessPiece : MonoBehaviour
{
    public Vector2 CurrentPosition;
    public ChessPieceColor Color;
    
    public void Setup(Vector3 initialPosition, ChessPieceColor color)
    {
        CurrentPosition = new Vector2(initialPosition.x, initialPosition.z);
        transform.position = initialPosition;
        Color = color;
        GetComponentInChildren<Renderer>().material = Resources.Load<Material>("Chess/ChessPiece_" + color + "_M");
    }

    public abstract List<Vector2> GetPotentialPositions();
    public abstract void MovePiece();

    public void KillPiece()
    {
        Destroy(gameObject);
    }
}
