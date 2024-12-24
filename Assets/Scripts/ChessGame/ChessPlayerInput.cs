using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChessPlayerInput
{
    public static Vector2 ForwardMovement = new Vector3(0, 1);
    public static Vector2 BackwardMovement = new Vector3(0, -1);
    public static Vector2 LeftMovement = new Vector3(-1, 0);
    public static Vector2 RightMovement = new Vector3(1, 0);

    public static Vector2 MovePlayer(Vector2 initialPosition, ChessMovementDirection direction, bool inverse)
    {
        Vector3 finalPosition = initialPosition;
        int inverseMultiplier = inverse ? -1 : 1;
        
        switch (direction)
        {
            case ChessMovementDirection.Forward:
                finalPosition = initialPosition + (ForwardMovement * inverseMultiplier);
                break;
            case ChessMovementDirection.Backward:
                finalPosition = initialPosition + (BackwardMovement * inverseMultiplier);
                break;
            case ChessMovementDirection.Left:
                finalPosition = initialPosition + (LeftMovement * inverseMultiplier);
                break;
            case ChessMovementDirection.Right:
                finalPosition = initialPosition + (RightMovement * inverseMultiplier);
                break;
        }

        if (finalPosition.x < 0 || finalPosition.y < 0 || finalPosition.x > 7 || finalPosition.y > 7)
            return initialPosition;

        return finalPosition;
    }
}
