using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public abstract class ChessPiece : MonoBehaviour
{
    // Event triggered when the piece is killed
    public UnityEvent<ChessPiece> onKill = new();
    // The piece's current position on the board
    public Vector2 CurrentPosition;
    // The piece's color
    public ChessPieceColor Color;
    // Track if the piece moved before or not for the pawn movement
    public bool HasMoved;
    // The possible directions this piece can move in
    public Vector2[] Directions;
    
    public void Setup(Vector3 initialPosition, ChessPieceColor color)
    {
        // Update the current position, color, and material
        CurrentPosition = new Vector2(initialPosition.x, initialPosition.z);
        transform.position = initialPosition;
        Color = color;
        GetComponentInChildren<Renderer>().material = Resources.Load<Material>("Chess/ChessPiece_" + color + "_M");
    }

    public virtual List<GridBlock> GetPotentialPositions(ChessBoard board)
    {
        List<GridBlock> potentialPositions = new List<GridBlock>();
        // legal positions the piece can move in
        List<Vector2> legalPositions = GetLegalPositions();
        // For every direction the piece can move in
        foreach (Vector2 movementDirection in Directions)
        {
            // Get the current block on the board
            GridBlock currentBlock = board.GetBlockAtPos(CurrentPosition);
            while (true)
            {
                Vector2 potentialPos = currentBlock.Position + movementDirection;
                // If the position is out of bounds, go to next direction
                if (!ChessGameHelperFunctions.CheckIfPosInBounds(potentialPos)) break;
                // If the position is not a valid position for this piece, go to next direction
                if (!legalPositions.Contains(potentialPos)) break;
                // If the position has a piece
                GridBlock potentialBlock = board.GetBlockAtPos(currentBlock.Position + movementDirection);
                if (potentialBlock.CurrentChessPiece)
                {
                    // if it's an opponent's piece, add the position and go to the next direction since we're blocked
                    if (potentialBlock.CurrentChessPiece.Color != Color) potentialPositions.Add(potentialBlock);
                    break;
                }
                // Add the position since it's in bounds and has no piece
                potentialPositions.Add(potentialBlock);
                // Update the current block for the next iteration
                currentBlock = potentialBlock;
            }
        }
    
        return potentialPositions;
    }

    public virtual List<Vector2> GetLegalPositions() { return null; }
    
    public void MovePiece(GridBlock targetBlock)
    {
        // Update the HasMoved flag and the position of the piece
        HasMoved = true;
        CurrentPosition = targetBlock.Position;
        transform.position = new Vector3(CurrentPosition.x, transform.position.y, CurrentPosition.y);
    }

    public void KillPiece()
    {
        // Trigger the onKill method and destroy the piece
        onKill.Invoke(this);
        Destroy(gameObject);
    }
}
