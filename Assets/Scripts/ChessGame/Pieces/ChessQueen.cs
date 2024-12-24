using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChessQueen : ChessPiece
{
    private Vector2[] direcions = new Vector2[8]
    {
        new Vector2(0, 1),
        new Vector2(0, -1),
        new Vector2(-1, 0),
        new Vector2(1, 0),
        new Vector2(1, 1),
        new Vector2(-1, -1),
        new Vector2(1, -1),
        new Vector2(-1, 1),
    };
    
    public override List<GridBlock> GetPotentialPositions(ChessBoard board)
    {
        List<GridBlock> potentialPositions = new List<GridBlock>();
        List<Vector2> legalPositions = ChessGameHelperFunctions.GetPositionsAlongAxis(CurrentPosition);
        legalPositions.AddRange(ChessGameHelperFunctions.GetDiagonalPositions(CurrentPosition));

        foreach (Vector2 movementDirection in direcions)
        {
            GridBlock currentBlock = board.GetBlockAtPos(CurrentPosition);
            while (true)
            {
                Vector2 potentialPos = currentBlock.Position + movementDirection;
                if (!ChessGameHelperFunctions.CheckIfPosInBounds(potentialPos)) // out of bounds, go to next direction
                    break;
                if (!legalPositions.Contains(potentialPos)) break; // not a valid position for this piece, break
                GridBlock potentialBlock = board.GetBlockAtPos(currentBlock.Position + movementDirection);
                // Found a piece
                if (potentialBlock.CurrentChessPiece)
                {
                    // if opponent's piece, add the position
                    if (potentialBlock.CurrentChessPiece.Color != Color)
                        potentialPositions.Add(potentialBlock);
                    break; // can't proceed in this direction, break.
                }
                potentialPositions.Add(potentialBlock); // add it if it passed all the above
                currentBlock = potentialBlock;
            }
        }

        return potentialPositions;
    }
}
