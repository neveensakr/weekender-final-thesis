using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ChessGameHelperFunctions
{
    public static ChessPieceColor GetGridBlockColor(Vector2 blockPosition)
    {
        if (blockPosition.x % 2 == 0 && blockPosition.y % 2 == 0)
            return ChessPieceColor.Black;
        
        if (blockPosition.x % 2 != 0 && blockPosition.y % 2 != 0)
            return ChessPieceColor.Black;
        
        return ChessPieceColor.White;
    }

    public static Vector2 GetPositionByIndex(int index, int rowCount, int columnCount)
    {
        if (index >= rowCount * columnCount)
            return new Vector2(0, 0);
        
        int x = index % rowCount;
        int y = index / rowCount;
        return new Vector2(x, y);
    }
}
