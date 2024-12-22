using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChessBishop : ChessPiece
{
    public override List<Vector2> GetPotentialPositions()
    {
        List<Vector2> movements = new List<Vector2>();
        List<Vector2> potentialMovements = ChessGameHelperFunctions.GetDiagonalPositions(CurrentPosition);

        foreach (Vector2 movement in potentialMovements)
        {
            if (ChessGameHelperFunctions.CheckIfPosInBounds(movement) && movement != CurrentPosition)
                movements.Add(movement);
        }
        
        return movements;
    }
}
