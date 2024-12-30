using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ChessGameHelperFunctions
{
    public static ChessPieceColor GetGridBlockColor(Vector2 blockPosition)
    {
        // if blocks are on an even position (both x and y) then they are black
        if (blockPosition.x % 2 == 0 && blockPosition.y % 2 == 0) return ChessPieceColor.Black;
        // if blocks are on an odd position (both x and y) then they are black
        if (blockPosition.x % 2 != 0 && blockPosition.y % 2 != 0) return ChessPieceColor.Black;
        // otherwise they are white
        return ChessPieceColor.White;
    }

    public static int GetIndexByPosition(int x, int y, int columnCount)
    {
        // The index is the number of columns * our current column + the horizontal position
        return (y * columnCount) + x;
    }

    public static bool CheckIfPosInBounds(Vector2 position)
    {
        // The chess board is 8x8
        return (position.x < 8 && position.y < 8 && position.x >= 0 && position.y >= 0);
    }

    public static List<Vector2> GetDirectNeighboursInBounds(Vector2 position)
    {
        List<Vector2> positions = new List<Vector2>();
        // Loop over all the 8 directions
        for (int x = -1; x <= 1; x++)
        {
            for (int y = -1; y <= 1; y++)
            {
                // If the position is within bounds, add it to the potential positions list
                Vector2 potentialPosition = new Vector2(position.x + x, position.y + y);
                if (CheckIfPosInBounds(potentialPosition) && potentialPosition != position)
                    positions.Add(potentialPosition);
            }
        }

        return positions;
    }

    public static List<Vector2> GetDiagonalPositions(Vector2 currentPosition)
    {
        List<Vector2> positions = new List<Vector2>();
        // Go through each of the 7 square blocks
        for (int x = -7; x <= 7; x++)
        {
            for (int y = -7; y <= 7; y++)
            {
                // if x and y don't equal each other then it's not a diagonal
                if (x != y) continue;
                // Add the diagonal, from both sides
                positions.Add(new Vector2(currentPosition.x + x, currentPosition.y + y));
                positions.Add(new Vector2(currentPosition.x - x, currentPosition.y + y));
            }
        }

        return positions;
    }

    public static List<Vector2> GetPositionsAlongAxis(Vector2 currentPosition)
    {
        List<Vector2> movements = new();
        // Go through all 7 squares and add them to the movements list
        for (int i = -7; i <= 7; i++)
        {
            movements.Add(new Vector2(currentPosition.x + i, currentPosition.y));
            movements.Add(new Vector2(currentPosition.x, currentPosition.y + i));
        }

        return movements;
    }
}
