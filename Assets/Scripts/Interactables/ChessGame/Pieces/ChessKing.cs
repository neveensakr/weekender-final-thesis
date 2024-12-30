using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ChessKing : ChessPiece
{
    public ChessKing()
    {
        // Set the directions the King can move in (any of the immediate squares)
        Directions = new[]
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
    }

    public override List<Vector2> GetLegalPositions()
    {
        return ChessGameHelperFunctions.GetDirectNeighboursInBounds(CurrentPosition);
    }
}
