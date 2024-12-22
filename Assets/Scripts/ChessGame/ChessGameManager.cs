using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChessGameManager : MonoBehaviour
{
    public Vector2 CurrentPlayerPosition;
    public GridBlock CurrentBlock;
    public ChessPiece ActivePiece;
    private ChessBoard board;
    private ChessPieceColor _playerColor = ChessPieceColor.White;
    
    void Start()
    {
        SpawnBoard();
        InitializePlayerInput();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow))
            CurrentPlayerPosition = ChessPlayerInput.MovePlayer(CurrentPlayerPosition, ChessMovementDirection.Forward);
        if (Input.GetKeyDown(KeyCode.DownArrow))
            CurrentPlayerPosition = ChessPlayerInput.MovePlayer(CurrentPlayerPosition, ChessMovementDirection.Backward);
        if (Input.GetKeyDown(KeyCode.LeftArrow))
            CurrentPlayerPosition = ChessPlayerInput.MovePlayer(CurrentPlayerPosition, ChessMovementDirection.Left);
        if (Input.GetKeyDown(KeyCode.RightArrow))
            CurrentPlayerPosition = ChessPlayerInput.MovePlayer(CurrentPlayerPosition, ChessMovementDirection.Right);
        if (Input.GetKeyDown(KeyCode.KeypadEnter))
            SetActivePiece();

        GridBlock newBlock = board.GetBlockAtPos(CurrentPlayerPosition);
        if (CurrentBlock != newBlock)
        {
            SetHoverEffect(CurrentBlock, newBlock, ActivePiece);
            CurrentBlock = newBlock;
        }
    }

    private void SpawnBoard()
    {
        board = Instantiate(Resources.Load<GameObject>("Chess/ChessBoard")).GetComponent<ChessBoard>();
        board.Initialize();
    }

    private void InitializePlayerInput()
    {
        CurrentPlayerPosition = new Vector2(3, 1);
        CurrentBlock = board.GetBlockAtPos(CurrentPlayerPosition);
        CurrentBlock.AdjustEffect(GridBlockEffect.Hover);
    }

    private void SetActivePiece()
    {
        ChessPiece newPiece = CurrentBlock.CurrentChessPiece;
        if (newPiece && newPiece.Color == _playerColor)
        {
            if (ActivePiece)
                board.GetBlockAtPos(ActivePiece.CurrentPosition).AdjustEffect(GridBlockEffect.Normal);
            ActivePiece = newPiece;
            CurrentBlock.AdjustEffect(GridBlockEffect.Selected);
        }
    }

    private void SetHoverEffect(GridBlock currentBlock, GridBlock nextBlock, ChessPiece activePiece)
    {
        if (currentBlock.CurrentChessPiece == activePiece && activePiece) // we are on a selected piece, so don't change the color back to Normal
            currentBlock.AdjustEffect(GridBlockEffect.Selected);
        else  // we are not on a selected piece, so change the color back to normal
            currentBlock.AdjustEffect(GridBlockEffect.Normal);
        
        nextBlock.AdjustEffect(GridBlockEffect.Hover);
    }
}
