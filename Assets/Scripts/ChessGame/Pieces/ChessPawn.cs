using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChessPawn : ChessPiece
{
    public override List<GridBlock> GetPotentialPositions(ChessBoard board)
    {
        int inverse = (Color == ChessPieceColor.Black) ? -1 : 1;
        List<GridBlock> potentialPositions = new List<GridBlock>();
        // Go Forward
        Vector2 potentialPos = CurrentPosition + (new Vector2(0, 1) * inverse);
        if (ChessGameHelperFunctions.CheckIfPosInBounds(potentialPos)) // in bounds
        {
            GridBlock potentialBlock = board.GetBlockAtPos(potentialPos);
            if (!potentialBlock.CurrentChessPiece)
                potentialPositions.Add(potentialBlock);
        }
        // Can move forward two blocks if it's the first time.
        if (!HasMoved)
        {
            potentialPos = CurrentPosition + (new Vector2(0, 2) * inverse);
            if (ChessGameHelperFunctions.CheckIfPosInBounds(potentialPos)) // in bounds
            {
                GridBlock potentialBlock = board.GetBlockAtPos(potentialPos);
                if (!potentialBlock.CurrentChessPiece)
                    potentialPositions.Add(potentialBlock);
            }
        }
        // Go Diagonal Left
        potentialPos = CurrentPosition + (new Vector2(-1, 1) * inverse);
        if (ChessGameHelperFunctions.CheckIfPosInBounds(potentialPos)) // in bounds
        {
            GridBlock potentialBlock = board.GetBlockAtPos(potentialPos);
            // Can Kill a Piece
            if (potentialBlock.CurrentChessPiece && potentialBlock.CurrentChessPiece.Color != Color)
                potentialPositions.Add(potentialBlock);
        }
        // Go Diagonal Right
        potentialPos = CurrentPosition + (new Vector2(1, 1) * inverse);
        if (ChessGameHelperFunctions.CheckIfPosInBounds(potentialPos)) // in bounds
        {
            GridBlock potentialBlock = board.GetBlockAtPos(potentialPos);
            // Can Kill a Piece
            if (potentialBlock.CurrentChessPiece && potentialBlock.CurrentChessPiece.Color != Color)
                potentialPositions.Add(potentialBlock);
        }
        
        return potentialPositions;
    }
}
