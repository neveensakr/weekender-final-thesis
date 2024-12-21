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
    public void GetPositionByIndex_InBounds()
    {
        int index = 17;
        int rowCount = 8;
        int columnCount = 8;
            
        Assert.AreEqual(new Vector2(1, 2), ChessGameHelperFunctions.GetPositionByIndex(index, rowCount, columnCount));
    }
    
    [Test]
    public void GetPositionByIndex_OutOfBounds()
    {
        int index = 100;
        int rowCount = 8;
        int columnCount = 8;
            
        Assert.AreEqual(new Vector2(0, 0), ChessGameHelperFunctions.GetPositionByIndex(index, rowCount, columnCount));
    }
}
