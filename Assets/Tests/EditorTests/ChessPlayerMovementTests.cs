using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class ChessPlayerMovementTests
{
    [Test]
    public void ChessPlayerMovement_MoveForward()
    {
        Vector2 initialPosition = new Vector2(4, 4);
        ChessMovementDirection movementDirection = ChessMovementDirection.Forward;
            
        Assert.AreEqual(new Vector2(4, 5), ChessPlayerInput.MovePlayer(initialPosition, movementDirection, false));
    }
    
    [Test]
    public void ChessPlayerMovement_MoveForwardInverse()
    {
        Vector2 initialPosition = new Vector2(4, 4);
        ChessMovementDirection movementDirection = ChessMovementDirection.Forward;
            
        Assert.AreEqual(new Vector2(4, 3), ChessPlayerInput.MovePlayer(initialPosition, movementDirection, true));
    }
    
    [Test]
    public void ChessPlayerMovement_MoveBackward()
    {
        Vector2 initialPosition = new Vector2(4, 4);
        ChessMovementDirection movementDirection = ChessMovementDirection.Backward;
            
        Assert.AreEqual(new Vector2(4, 3), ChessPlayerInput.MovePlayer(initialPosition, movementDirection, false));
    }
    
    [Test]
    public void ChessPlayerMovement_MoveBackwardInverse()
    {
        Vector2 initialPosition = new Vector2(4, 4);
        ChessMovementDirection movementDirection = ChessMovementDirection.Backward;
            
        Assert.AreEqual(new Vector2(4, 5), ChessPlayerInput.MovePlayer(initialPosition, movementDirection, true));
    }
    
    [Test]
    public void ChessPlayerMovement_MoveLeft()
    {
        Vector2 initialPosition = new Vector2(4, 4);
        ChessMovementDirection movementDirection = ChessMovementDirection.Left;
            
        Assert.AreEqual(new Vector2(3, 4), ChessPlayerInput.MovePlayer(initialPosition, movementDirection, false));
    }
    
    [Test]
    public void ChessPlayerMovement_MoveLeftInverse()
    {
        Vector2 initialPosition = new Vector2(4, 4);
        ChessMovementDirection movementDirection = ChessMovementDirection.Left;
            
        Assert.AreEqual(new Vector2(5, 4), ChessPlayerInput.MovePlayer(initialPosition, movementDirection, true));
    }
    
    [Test]
    public void ChessPlayerMovement_MoveRight()
    {
        Vector2 initialPosition = new Vector2(4, 4);
        ChessMovementDirection movementDirection = ChessMovementDirection.Right;
            
        Assert.AreEqual(new Vector2(5, 4), ChessPlayerInput.MovePlayer(initialPosition, movementDirection, false));
    }
    
    [Test]
    public void ChessPlayerMovement_MoveRightInverse()
    {
        Vector2 initialPosition = new Vector2(4, 4);
        ChessMovementDirection movementDirection = ChessMovementDirection.Right;
            
        Assert.AreEqual(new Vector2(3, 4), ChessPlayerInput.MovePlayer(initialPosition, movementDirection, true));
    }
    
    [Test]
    public void ChessPlayerMovement_MoveForwardOverflow()
    {
        Vector2 initialPosition = new Vector2(4, 7);
        ChessMovementDirection movementDirection = ChessMovementDirection.Forward;
        
        Assert.AreEqual(new Vector2(4, 7), ChessPlayerInput.MovePlayer(initialPosition, movementDirection, false));
    }
    
    [Test]
    public void ChessPlayerMovement_MoveForwardOverflowInverse()
    {
        Vector2 initialPosition = new Vector2(4, 0);
        ChessMovementDirection movementDirection = ChessMovementDirection.Forward;
        
        Assert.AreEqual(new Vector2(4, 0), ChessPlayerInput.MovePlayer(initialPosition, movementDirection, true));
    }
    
    [Test]
    public void ChessPlayerMovement_MoveBackwardOverflow()
    {
        Vector2 initialPosition = new Vector2(4, 0);
        ChessMovementDirection movementDirection = ChessMovementDirection.Backward;
            
        Assert.AreEqual(new Vector2(4, 0), ChessPlayerInput.MovePlayer(initialPosition, movementDirection, false));
    }
    
    [Test]
    public void ChessPlayerMovement_MoveBackwardOverflowInverse()
    {
        Vector2 initialPosition = new Vector2(4, 7);
        ChessMovementDirection movementDirection = ChessMovementDirection.Backward;
            
        Assert.AreEqual(new Vector2(4, 7), ChessPlayerInput.MovePlayer(initialPosition, movementDirection, true));
    }
    
    [Test]
    public void ChessPlayerMovement_MoveLeftOverflow()
    {
        Vector2 initialPosition = new Vector2(0, 4);
        ChessMovementDirection movementDirection = ChessMovementDirection.Left;
            
        Assert.AreEqual(new Vector2(0, 4), ChessPlayerInput.MovePlayer(initialPosition, movementDirection, false));
    }
    
    [Test]
    public void ChessPlayerMovement_MoveLeftOverflowInverse()
    {
        Vector2 initialPosition = new Vector2(7, 4);
        ChessMovementDirection movementDirection = ChessMovementDirection.Left;
            
        Assert.AreEqual(new Vector2(7, 4), ChessPlayerInput.MovePlayer(initialPosition, movementDirection, true));
    }
    
    [Test]
    public void ChessPlayerMovement_MoveRightOverflow()
    {
        Vector2 initialPosition = new Vector2(7, 4);
        ChessMovementDirection movementDirection = ChessMovementDirection.Right;
            
        Assert.AreEqual(new Vector2(7, 4), ChessPlayerInput.MovePlayer(initialPosition, movementDirection, false));
    }
    
    [Test]
    public void ChessPlayerMovement_MoveRightOverflowInverse()
    {
        Vector2 initialPosition = new Vector2(0, 4);
        ChessMovementDirection movementDirection = ChessMovementDirection.Right;
            
        Assert.AreEqual(new Vector2(0, 4), ChessPlayerInput.MovePlayer(initialPosition, movementDirection, true));
    }
}
