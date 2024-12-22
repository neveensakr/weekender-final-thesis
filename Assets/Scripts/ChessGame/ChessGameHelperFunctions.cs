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

    public static int GetIndexByPosition(int x, int y, int columnCount)
    {
        return (y * columnCount) + x;
    }

    public static bool CheckIfPosInBounds(Vector2 position)
    {
        return (position.x < 8 && position.y < 8 && position.x >= 0 && position.y >= 0);
    }

    public static List<Vector2> GetDiagonalPositions(Vector2 currentPosition)
    {
        List<Vector2> positions = new List<Vector2>();
        for (int x = -7; x <= 7; x++)
        {
            for (int y = -7; y <= 7; y++)
            {
                if (x != y)
                    continue;
                positions.Add(new Vector2(currentPosition.x + x, currentPosition.y + y));
                positions.Add(new Vector2(currentPosition.x - x, currentPosition.y + y));
            }
        }

        return positions;
    }

    public static List<Vector2> GetPositionsAlongAxis(Vector2 currentPosition)
    {
        List<Vector2> movements = new();
        for (int i = -7; i <= 7; i++)
        {
            movements.Add(new Vector2(currentPosition.x + i, currentPosition.y));
            movements.Add(new Vector2(currentPosition.x, currentPosition.y + i));
        }

        return movements;
    }
}
