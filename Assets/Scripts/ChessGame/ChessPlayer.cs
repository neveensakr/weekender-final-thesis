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
    private bool _inverseMovement;
    private bool _isAI;

    public ChessPlayer(ChessBoard board, ChessPieceColor color, Vector2 currentPosition, bool isAI)
    {
        _board = board;
        Color = color;
        CurrentPosition = currentPosition;
        _isAI = isAI;
        InitializePlayerInput();
    }
    
    private void InitializePlayerInput()
    {
        _inverseMovement = (Color == ChessPieceColor.Black);
        CurrentBlock = _board.GetBlockAtPos(CurrentPosition);
        CurrentBlock.AdjustEffect(GridBlockEffect.Hover);
    }
    
    public bool Move()
    {
        if (_isAI)
        {
            Debug.Log("Picking Possible Block...");
            (GridBlock, GridBlock) targetBlock = PickPossibleBlock();
            Debug.Log("Picked Block at: " + targetBlock.Item1.Position);
            CurrentPosition = targetBlock.Item1.Position;
            CurrentBlock = targetBlock.Item1;
            SetActivePiece();
            Debug.Log("Moving Block to: " + targetBlock.Item2.Position);
            CurrentPosition = targetBlock.Item2.Position;
            CurrentBlock = targetBlock.Item2;
            return MoveActivePiece();
        }
        
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
            return MoveActivePiece();
        
        return false;
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
    
    private bool MoveActivePiece()
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
            CurrentBlock.AdjustEffect(GridBlockEffect.Selected);
            return true;
        }

        return false;
    }

    private (GridBlock, GridBlock) PickPossibleBlock()
    {
       List<GridBlock> blocks = _board.GetAllBlocksOfColor(Color);
       int randomBlockIndex = Random.Range(0, blocks.Count - 1);
       List<GridBlock> potentialPos = blocks[randomBlockIndex].CurrentChessPiece.GetPotentialPositions(_board);
       while (potentialPos.Count == 0)
       {
           blocks.Remove(blocks[randomBlockIndex]); // block has no moves.
           if (blocks.Count == 0) return (null, null); // no more blocks
           randomBlockIndex = Random.Range(0, blocks.Count - 1);
           potentialPos = blocks[randomBlockIndex].CurrentChessPiece.GetPotentialPositions(_board);
       }
       // found a block with moves, return a random movement.
       return (blocks[randomBlockIndex], potentialPos[Random.Range(0, potentialPos.Count - 1)]);
    }
}
