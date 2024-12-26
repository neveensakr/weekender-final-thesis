using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChessRook : ChessPiece
{
    public ChessRook()
    {
        // Set the directions the Rook can move in (along the Horizontal and Vertical axis)
        Directions = new[]
        {
            new Vector2(0, 1),
            new Vector2(0, -1),
            new Vector2(-1, 0),
            new Vector2(1, 0)
        };
    }

    public override List<Vector2> GetLegalPositions()
    {
        return ChessGameHelperFunctions.GetPositionsAlongAxis(CurrentPosition);
    }
}
