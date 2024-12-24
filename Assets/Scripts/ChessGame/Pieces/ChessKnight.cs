using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChessKnight : ChessPiece
{
    public override List<GridBlock> GetPotentialPositions(ChessBoard board)
    {
        List<GridBlock> potentialPositions = new List<GridBlock>();
        
        for (int x = -2; x <= 2; x++)
        {
            for (int y = -2; y <= 2; y++)
            {
                if (Math.Abs(x+y) != 3 && Math.Abs(y-x) != 3) continue;
                
                Vector2 potentialPos = new Vector2(CurrentPosition.x + x, CurrentPosition.y + y);
                if (!ChessGameHelperFunctions.CheckIfPosInBounds(potentialPos)) // out of bounds, go to next point
                    continue;
                GridBlock potentialBlock = board.GetBlockAtPos(potentialPos);
                if (potentialBlock.CurrentChessPiece && potentialBlock.CurrentChessPiece.Color == Color) continue;
                potentialPositions.Add(potentialBlock);
            }
        }

        return potentialPositions;
    }
}
