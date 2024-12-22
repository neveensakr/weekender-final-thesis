using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class ChessGameHelperFunctionsTests
{
    [Test]
    public void GetGridBlockColor_EvenBlack()
    {
        Vector2 blockPosition = new Vector2(0, 0);
            
        Assert.AreEqual(ChessPieceColor.Black, ChessGameHelperFunctions.GetGridBlockColor(blockPosition));
    }
    
    [Test]
    public void GetGridBlockColor_OddBlack()
    {
        Vector2 blockPosition = new Vector2(3, 1);
            
        Assert.AreEqual(ChessPieceColor.Black, ChessGameHelperFunctions.GetGridBlockColor(blockPosition));
    }
    
    [Test]
    public void GetGridBlockColor_White()
    {
        Vector2 blockPosition = new Vector2(5, 2);
            
        Assert.AreEqual(ChessPieceColor.White, ChessGameHelperFunctions.GetGridBlockColor(blockPosition));
    }
    
    [Test]
    public void GetIndexByPosition_InBounds()
    {
        int x = 5;
        int y = 2;
        int columnCount = 8;
            
        Assert.AreEqual(21, ChessGameHelperFunctions.GetIndexByPosition(x, y, columnCount));
    }
    
    [Test]
    public void CheckIfPosInBounds_True()
    {
        Vector2 position = new Vector2(0, 0);
            
        Assert.True(ChessGameHelperFunctions.CheckIfPosInBounds(position), "The position should be in bounds.");
    }
    
    [Test]
    public void CheckIfPosInBounds_XOut()
    {
        Vector2 position = new Vector2(8, 1);
            
        Assert.False(ChessGameHelperFunctions.CheckIfPosInBounds(position), "The position should not be in bounds.");
    }
    
    [Test]
    public void CheckIfPosInBounds_YOut()
    {
        Vector2 position = new Vector2(3, 10);
            
        Assert.False(ChessGameHelperFunctions.CheckIfPosInBounds(position), "The position should not be in bounds.");
    }
    
    [Test]
    public void CheckIfPosInBounds_NegXOut()
    {
        Vector2 position = new Vector2(-5, 1);
            
        Assert.False(ChessGameHelperFunctions.CheckIfPosInBounds(position), "The position should not be in bounds.");
    }
    
    [Test]
    public void CheckIfPosInBounds_NegYOut()
    {
        Vector2 position = new Vector2(3, -1);
            
        Assert.False(ChessGameHelperFunctions.CheckIfPosInBounds(position),  "The position should not be in bounds.");
    }
    
    [Test]
    public void GetDiagonalPositions_ZeroPoint()
    {
        Vector2 currentPosition = new Vector2(0, 0);
        List<Vector2> result = ChessGameHelperFunctions.GetDiagonalPositions(currentPosition);

        for (int i = -7; i <= 7; i++)
        {
            Vector2 expectedPos1 = new Vector2(currentPosition.x + i, currentPosition.y + i);
            Vector2 expectedPos2 = new Vector2(currentPosition.x - i, currentPosition.y + i);

            Assert.Contains(expectedPos1, result, $"The result should contain the position {expectedPos1}.");
            Assert.Contains(expectedPos2, result, $"The result should contain the position {expectedPos2}.");
        }
    }
    
    [Test]
    public void GetDiagonalPositions_NonZeroPoint()
    {
        Vector2 currentPosition = new Vector2(5, 3);
        List<Vector2> result = ChessGameHelperFunctions.GetDiagonalPositions(currentPosition);

        for (int i = -7; i <= 7; i++)
        {
            Vector2 expectedPos1 = new Vector2(currentPosition.x + i, currentPosition.y + i);
            Vector2 expectedPos2 = new Vector2(currentPosition.x - i, currentPosition.y + i);

            Assert.Contains(expectedPos1, result, $"The result should contain the position {expectedPos1}.");
            Assert.Contains(expectedPos2, result, $"The result should contain the position {expectedPos2}.");
        }
    }
    
    [Test]
    public void GetPositionsAlongAxis_ZeroPoint()
    {
        Vector2 currentPosition = new Vector2(0, 0);
        List<Vector2> result = ChessGameHelperFunctions.GetPositionsAlongAxis(currentPosition);

        for (int i = -7; i <= 7; i++)
        {
            Vector2 expectedPos1 = new Vector2(currentPosition.x + i, currentPosition.y);
            Vector2 expectedPos2 = new Vector2(currentPosition.x, currentPosition.y + i);

            Assert.Contains(expectedPos1, result, $"The result should contain the position {expectedPos1}.");
            Assert.Contains(expectedPos2, result, $"The result should contain the position {expectedPos2}.");
        }
    }
    
    [Test]
    public void GetPositionsAlongAxis_NonZeroPoint()
    {
        Vector2 currentPosition = new Vector2(4, 7);
        List<Vector2> result = ChessGameHelperFunctions.GetPositionsAlongAxis(currentPosition);

        for (int i = -7; i <= 7; i++)
        {
            Vector2 expectedPos1 = new Vector2(currentPosition.x + i, currentPosition.y);
            Vector2 expectedPos2 = new Vector2(currentPosition.x, currentPosition.y + i);

            Assert.Contains(expectedPos1, result, $"The result should contain the position {expectedPos1}.");
            Assert.Contains(expectedPos2, result, $"The result should contain the position {expectedPos2}.");
        }
    }
}
