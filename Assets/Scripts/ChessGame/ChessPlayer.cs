using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChessPlayer
{
    public Vector2 CurrentPosition;
    public GridBlock CurrentBlock;
    public ChessPiece ActivePiece;
    public ChessPieceColor Color;

    private ChessBoard _board;
    private bool _inverseMovement = false;

    public ChessPlayer(ChessBoard board, ChessPieceColor color, Vector2 currentPosition)
    {
        _board = board;
        Color = color;
        CurrentPosition = currentPosition;
        InitializePlayerInput();
    }
    
    private void InitializePlayerInput()
    {
        _inverseMovement = (Color == ChessPieceColor.Black);
        CurrentPosition = new Vector2(3, 1);
        CurrentBlock = _board.GetBlockAtPos(CurrentPosition);
        CurrentBlock.AdjustEffect(GridBlockEffect.Hover);
    }
    
    public void Move()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow))
            CurrentPosition = ChessPlayerInput.MovePlayer(CurrentPosition, ChessMovementDirection.Forward, _inverseMovement);
        if (Input.GetKeyDown(KeyCode.DownArrow))
            CurrentPosition = ChessPlayerInput.MovePlayer(CurrentPosition, ChessMovementDirection.Backward, _inverseMovement);
        if (Input.GetKeyDown(KeyCode.LeftArrow))
            CurrentPosition = ChessPlayerInput.MovePlayer(CurrentPosition, ChessMovementDirection.Left, _inverseMovement);
        if (Input.GetKeyDown(KeyCode.RightArrow))
            CurrentPosition = ChessPlayerInput.MovePlayer(CurrentPosition, ChessMovementDirection.Right, _inverseMovement);
        
        if (Input.GetKeyDown(KeyCode.KeypadEnter))
            SetActivePiece();
        if (Input.GetKeyDown(KeyCode.Space))
            MoveActivePiece();
    }
    
    private void SetActivePiece()
    {
        ChessPiece newPiece = CurrentBlock.CurrentChessPiece;
        if (newPiece && newPiece.Color == Color)
        {
            if (ActivePiece)
                _board.GetBlockAtPos(ActivePiece.CurrentPosition).AdjustEffect(GridBlockEffect.Normal);
            ActivePiece = newPiece;
            CurrentBlock.AdjustEffect(GridBlockEffect.Selected);
            _board.HighlightBlocks(ActivePiece.GetPotentialPositions(_board));
        }
    }
    
    private void MoveActivePiece()
    {
        if (ActivePiece && _board.HighlightedBlocks.Contains(_board.GetBlockAtPos(CurrentPosition)))
        {
            GridBlock oldBlock = _board.GetBlockAtPos(ActivePiece.CurrentPosition);
            oldBlock.CurrentChessPiece = null;
            oldBlock.AdjustEffect(GridBlockEffect.Normal);
            if (CurrentBlock.CurrentChessPiece)
                CurrentBlock.CurrentChessPiece.KillPiece();
            
            ActivePiece.MovePiece(CurrentBlock);
            CurrentBlock.CurrentChessPiece = ActivePiece;
            _board.ResetHighlightedBlocks();
        }
    }
}
