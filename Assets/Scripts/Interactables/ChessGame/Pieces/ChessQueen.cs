using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChessQueen : ChessPiece
{
    public ChessQueen()
    {
        // Set the directions the Queen can move in (along any axis)
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
        List<Vector2> legalPositions = ChessGameHelperFunctions.GetPositionsAlongAxis(CurrentPosition);
        legalPositions.AddRange(ChessGameHelperFunctions.GetDiagonalPositions(CurrentPosition));
        return legalPositions;
    }
}
