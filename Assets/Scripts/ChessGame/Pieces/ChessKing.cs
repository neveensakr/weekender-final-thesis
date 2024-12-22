using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChessKing : ChessPiece
{
    public override List<Vector2> GetPotentialPositions()
    {
        List<Vector2> movements = new();
        
        for (int x = -1; x <= 1; x++)
        {
            for (int y = -1; y <= 1; y++)
            {
                Vector2 potentialPosition = new Vector2(CurrentPosition.x + x, CurrentPosition.y + y);
                if (ChessGameHelperFunctions.CheckIfPosInBounds(potentialPosition) && potentialPosition != CurrentPosition)
                    movements.Add(potentialPosition);
            }
        }

        return movements;
    }
}
