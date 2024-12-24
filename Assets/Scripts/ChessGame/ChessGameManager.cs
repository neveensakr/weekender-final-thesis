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
    private bool _inverseMovement = false;
    
    void Start()
    {
        SpawnBoard();
        InitializePlayerInput();
        _inverseMovement = (_playerColor == ChessPieceColor.Black);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow))
            CurrentPlayerPosition = ChessPlayerInput.MovePlayer(CurrentPlayerPosition, ChessMovementDirection.Forward, _inverseMovement);
        if (Input.GetKeyDown(KeyCode.DownArrow))
            CurrentPlayerPosition = ChessPlayerInput.MovePlayer(CurrentPlayerPosition, ChessMovementDirection.Backward, _inverseMovement);
        if (Input.GetKeyDown(KeyCode.LeftArrow))
            CurrentPlayerPosition = ChessPlayerInput.MovePlayer(CurrentPlayerPosition, ChessMovementDirection.Left, _inverseMovement);
        if (Input.GetKeyDown(KeyCode.RightArrow))
            CurrentPlayerPosition = ChessPlayerInput.MovePlayer(CurrentPlayerPosition, ChessMovementDirection.Right, _inverseMovement);
        if (Input.GetKeyDown(KeyCode.KeypadEnter))
            SetActivePiece();
        if (Input.GetKeyDown(KeyCode.Space))
            MoveActivePiece();

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
            board.HighlightBlocks(ActivePiece.GetPotentialPositions(board));
        }
    }

    private void MoveActivePiece()
    {
        if (ActivePiece && board.HighlightedBlocks.Contains(board.GetBlockAtPos(CurrentPlayerPosition)))
        {
            GridBlock oldBlock = board.GetBlockAtPos(ActivePiece.CurrentPosition);
            oldBlock.CurrentChessPiece = null;
            oldBlock.AdjustEffect(GridBlockEffect.Normal);
            if (CurrentBlock.CurrentChessPiece)
            {
                if (CurrentBlock.CurrentChessPiece is ChessKing)
                    EndGame();
                CurrentBlock.CurrentChessPiece.KillPiece();
            }
            ActivePiece.MovePiece(CurrentBlock);
            CurrentBlock.CurrentChessPiece = ActivePiece;
            board.ResetHighlightedBlocks();
        }
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
