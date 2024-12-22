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
}
