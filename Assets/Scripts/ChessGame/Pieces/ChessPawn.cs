using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChessPawn : ChessPiece
{
    public override List<Vector2> GetPotentialPositions()
    {
        List<Vector2> movements = new();
        
        for (int x = 1; x <= 2; x++)
        {
            int increment = Color == ChessPieceColor.Black ? -x : x;
            movements.Add(new Vector2(CurrentPosition.x, CurrentPosition.y + increment));
        }

        return movements;
    }

    public override void MovePiece()
    {
        throw new System.NotImplementedException();
    }
}
