using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChessBishop : ChessPiece
{
    public ChessBishop()
    {
        // Set the directions the Bishop can move in (diagonally)
        Directions = new[]
        {
            new Vector2(1, 1),
            new Vector2(-1, -1),
            new Vector2(-1, 1),
            new Vector2(1, -1)
        };
    }

    public override List<Vector2> GetLegalPositions()
    {
        return ChessGameHelperFunctions.GetDiagonalPositions(CurrentPosition);
    }
}
