using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChessKnight : ChessPiece
{
    public override List<Vector2> GetPotentialPositions()
    {
        List<Vector2> movements = new();
        
        for (int x = -2; x <= 2; x++)
        {
            for (int y = -2; y <= 2; y++)
            {
                if (Math.Abs(x+y) != 3 && Math.Abs(y-x) != 3) continue;
                
                Vector2 potentialPosition = new Vector2(CurrentPosition.x + x, CurrentPosition.y + y);
                if (ChessGameHelperFunctions.CheckIfPosInBounds(potentialPosition) && potentialPosition != CurrentPosition)
                    movements.Add(potentialPosition);
            }
        }

        return movements;
    }
}
