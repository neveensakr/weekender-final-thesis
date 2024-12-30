using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ChessPlayerInput
{
    // Movement direction vectors and names
    private static readonly Dictionary<ChessMovementDirection, Vector2> MovementDirection = new() {
        {ChessMovementDirection.Forward, new Vector2(0, 1)},
        {ChessMovementDirection.Backward, new Vector2(0, -1)},
        {ChessMovementDirection.Left, new Vector2(-1, 0)},
        {ChessMovementDirection.Right, new Vector2(1, 0)}
    };

    public static Vector2 MovePlayer(Vector2 initialPosition, ChessMovementDirection direction, bool inverse)
    {
        // Inverse the movement for black
        int inverseMultiplier = inverse ? -1 : 1;
        Vector2 finalPosition = initialPosition + (MovementDirection[direction] * inverseMultiplier);
        // If the position is not in bounds, return the initial position
        if (finalPosition.x < 0 || finalPosition.y < 0 || finalPosition.x > 7 || finalPosition.y > 7)
            return initialPosition;
        // return the new position
        return finalPosition;
    }
}
