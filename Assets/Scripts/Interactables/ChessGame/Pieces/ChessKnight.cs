using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChessKnight : ChessPiece
{
    public override List<GridBlock> GetPotentialPositions(ChessBoard board)
    {
        List<GridBlock> potentialPositions = new List<GridBlock>();
        // Loop through the squares around the knight with a 2 block radius
        for (int x = -2; x <= 2; x++)
        {
            for (int y = -2; y <= 2; y++)
            {
                // The Knight can only move three blocks at a time
                if (Math.Abs(x+y) != 3 && Math.Abs(y-x) != 3) continue;
                
                Vector2 potentialPos = new Vector2(CurrentPosition.x + x, CurrentPosition.y + y);
                // If the position out of bounds, go to next point
                if (!ChessGameHelperFunctions.CheckIfPosInBounds(potentialPos)) continue;
                // If there is a block of the same color at that position, move to the next point
                GridBlock potentialBlock = board.GetBlockAtPos(potentialPos);
                if (potentialBlock.CurrentChessPiece && potentialBlock.CurrentChessPiece.Color == Color) continue;
                // There is no block of the same color and the position is within bounds, we can add it to the list.
                potentialPositions.Add(potentialBlock);
            }
        }

        return potentialPositions;
    }
}
