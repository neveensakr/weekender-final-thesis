using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public abstract class ChessPiece : MonoBehaviour
{
    public UnityEvent<ChessPiece> onKill = new UnityEvent<ChessPiece>();
    public Vector2 CurrentPosition;
    public ChessPieceColor Color;
    public bool HasMoved = false;
    
    public void Setup(Vector3 initialPosition, ChessPieceColor color)
    {
        CurrentPosition = new Vector2(initialPosition.x, initialPosition.z);
        transform.position = initialPosition;
        Color = color;
        GetComponentInChildren<Renderer>().material = Resources.Load<Material>("Chess/ChessPiece_" + color + "_M");
    }

    public abstract List<GridBlock> GetPotentialPositions(ChessBoard board);

    public void MovePiece(GridBlock targetBlock)
    {
        HasMoved = true;
        CurrentPosition = targetBlock.Position;
        transform.position = new Vector3(CurrentPosition.x, transform.position.y, CurrentPosition.y);
    }

    public void KillPiece()
    {
        onKill.Invoke(this);
        Destroy(gameObject);
    }
}
