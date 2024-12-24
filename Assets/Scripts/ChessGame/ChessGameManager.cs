using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChessGameManager : MonoBehaviour
{
    public ChessPlayer player;
    private ChessBoard board;
    
    void Start()
    {
        SpawnBoard();
        player = new ChessPlayer(board, ChessPieceColor.White, new Vector2(3, 1));
    }

    private void Update()
    {
        player.Move();
        
        GridBlock newBlock = board.GetBlockAtPos(player.CurrentPosition);
        if (player.CurrentBlock != newBlock)
        {
            SetHoverEffect(player.CurrentBlock, newBlock, player.ActivePiece);
            player.CurrentBlock = newBlock;
        }
    }

    private void SpawnBoard()
    {
        board = Instantiate(Resources.Load<GameObject>("Chess/ChessBoard")).GetComponent<ChessBoard>();
        board.Initialize();
    }

    private void SetHoverEffect(GridBlock currentBlock, GridBlock nextBlock, ChessPiece activePiece)
    {
        if (currentBlock.CurrentChessPiece == activePiece && activePiece) // we were on a selected piece, don't change the color back to Normal
            currentBlock.AdjustEffect(GridBlockEffect.Selected);
        else if (board.HighlightedBlocks.Contains(board.GetBlockAtPos(currentBlock.Position))) // we were on a potential position
            currentBlock.AdjustEffect(GridBlockEffect.PotentialPosition);
        else // we were not on a selected or potential block, so change the color back to normal
            currentBlock.AdjustEffect(GridBlockEffect.Normal);
        
        nextBlock.AdjustEffect(GridBlockEffect.Hover);
    }

    private void EndGame()
    {
        Debug.Log("Game Ended");
    }
}
