using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChessPawn : ChessPiece
{
    public override List<GridBlock> GetPotentialPositions(ChessBoard board)
    {
        // Depending on the piece's color, the pawn can move forward or backwards on the board.
        int inverse = (Color == ChessPieceColor.Black) ? -1 : 1;
        List<GridBlock> potentialPositions = new List<GridBlock>();
        // Forward 1 Block check
        Vector2 potentialPos = CurrentPosition + (new Vector2(0, 1) * inverse);
        // If the block is in bounds and no chess piece is on it, we can move to it.
        GridBlock block = CheckIfInBoundsAndBlank(potentialPos, board);
        if (block) potentialPositions.Add(block);
        // Can move forward two blocks if it's the first time.
        if (!HasMoved)
        {
            potentialPos = CurrentPosition + (new Vector2(0, 2) * inverse);
            // If the block is in bounds and no chess piece is on it, we can move to it.
            block = CheckIfInBoundsAndBlank(potentialPos, board);
            if (block) potentialPositions.Add(block);
        }
        // Go Diagonal Left
        potentialPos = CurrentPosition + (new Vector2(-1, 1) * inverse);
        // If the block is in bounds and is blank or an opponent's chess piece is on it, we can move to it.
        block = CheckIfInBoundsAndLegal(potentialPos, board);
        if (block) potentialPositions.Add(block);
        // Go Diagonal Right
        potentialPos = CurrentPosition + (new Vector2(1, 1) * inverse);
        // If the block is in bounds and is blank or an opponent's chess piece is on it, we can move to it.
        block = CheckIfInBoundsAndLegal(potentialPos, board);
        if (block) potentialPositions.Add(block);
        
        return potentialPositions;
    }

    private GridBlock CheckIfInBoundsAndBlank(Vector2 potentialPos, ChessBoard board)
    {
        if (ChessGameHelperFunctions.CheckIfPosInBounds(potentialPos))
        {
            GridBlock potentialBlock = board.GetBlockAtPos(potentialPos);
            if (!potentialBlock.CurrentChessPiece) return potentialBlock;
        }

        return null;
    }
    
    private GridBlock CheckIfInBoundsAndLegal(Vector2 potentialPos, ChessBoard board)
    {
        if (ChessGameHelperFunctions.CheckIfPosInBounds(potentialPos)) // in bounds
        {
            GridBlock potentialBlock = board.GetBlockAtPos(potentialPos);
            // Can Kill a Piece
            if (potentialBlock.CurrentChessPiece && potentialBlock.CurrentChessPiece.Color != Color)
                return potentialBlock;
        }

        return null;
    }
}
