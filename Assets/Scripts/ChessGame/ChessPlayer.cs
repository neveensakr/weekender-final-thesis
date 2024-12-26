using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChessPlayer
{
    // The player's current position, block, and piece on the board
    public Vector2 CurrentPosition;
    public GridBlock CurrentBlock;
    public ChessPiece ActivePiece;
    // The player's color
    private ChessPieceColor _color;
    // The ChessBoard
    private ChessBoard _board;
    // Flag to inverse the movement based on the player's color
    private bool _inverseMovement;
    // Flag to move based on AI vs manual
    private bool _isAI;

    public ChessPlayer(ChessBoard board, ChessPieceColor color, Vector2 currentPosition, bool isAI)
    {
        CurrentPosition = currentPosition;
        _board = board;
        _color = color;
        _isAI = isAI;
        InitializePlayerInput();
    }
    
    private void InitializePlayerInput()
    {
        // Set the inverse movement and current block
        _inverseMovement = (_color == ChessPieceColor.Black);
        CurrentBlock = _board.GetBlockAtPos(CurrentPosition);
        CurrentBlock.AdjustEffect(GridBlockEffect.Hover);
    }
    
    public bool Move()
    {
        // Move with AI if it's an AI
        if (_isAI) return AIMove();
        // Otherwise, move with the keyboard
        if (Input.GetKeyDown(KeyCode.UpArrow))
            CurrentPosition = ChessPlayerInput.MovePlayer(CurrentPosition, ChessMovementDirection.Forward, _inverseMovement);
        if (Input.GetKeyDown(KeyCode.DownArrow))
            CurrentPosition = ChessPlayerInput.MovePlayer(CurrentPosition, ChessMovementDirection.Backward, _inverseMovement);
        if (Input.GetKeyDown(KeyCode.LeftArrow))
            CurrentPosition = ChessPlayerInput.MovePlayer(CurrentPosition, ChessMovementDirection.Left, _inverseMovement);
        if (Input.GetKeyDown(KeyCode.RightArrow))
            CurrentPosition = ChessPlayerInput.MovePlayer(CurrentPosition, ChessMovementDirection.Right, _inverseMovement);
        
        if (Input.GetKeyDown(KeyCode.KeypadEnter)) SetActivePiece();
        if (Input.GetKeyDown(KeyCode.Space)) return MoveActivePiece();
        
        return false;
    }

    private bool AIMove()
    {
        // Pick a possible block
        Debug.Log("[ChessPlayer - AIMove] Picking Possible Block...");
        (GridBlock, GridBlock) targetBlock = PickPossibleBlock();
        Debug.Log("[ChessPlayer - AIMove] Picked Block at: " + targetBlock.Item1.Position);
        // Set the picked block as the current block
        CurrentPosition = targetBlock.Item1.Position;
        CurrentBlock = targetBlock.Item1;
        SetActivePiece();
        // Move the picked block to the target positon
        Debug.Log("[ChessPlayer - AIMove] Moving Block to: " + targetBlock.Item2.Position);
        CurrentPosition = targetBlock.Item2.Position;
        CurrentBlock = targetBlock.Item2;
        return MoveActivePiece();
    }
    
    private void SetActivePiece()
    {
        ChessPiece newPiece = CurrentBlock.CurrentChessPiece;
        if (newPiece && newPiece.Color == _color)
        {
            // If there is an active piece, deactivate it's effect
            if (ActivePiece) _board.GetBlockAtPos(ActivePiece.CurrentPosition).AdjustEffect(GridBlockEffect.Normal);
            ActivePiece = newPiece;
            CurrentBlock.AdjustEffect(GridBlockEffect.Selected);
            // Update the board to show the new piece's potential movements
            _board.HighlightBlocks(ActivePiece.GetPotentialPositions(_board));
        }
    }
    
    private bool MoveActivePiece()
    {
        // If there is an active piece to move and we are on a potential position
        if (ActivePiece && _board.HighlightedBlocks.Contains(_board.GetBlockAtPos(CurrentPosition)))
        {
            // Move the piece from the old block to the new block
            GridBlock oldBlock = _board.GetBlockAtPos(ActivePiece.CurrentPosition);
            oldBlock.CurrentChessPiece = null;
            oldBlock.AdjustEffect(GridBlockEffect.Normal);
            // If there was a chess piece, kill it
            if (CurrentBlock.CurrentChessPiece) CurrentBlock.CurrentChessPiece.KillPiece();
            // Move the piece to the new block
            ActivePiece.MovePiece(CurrentBlock);
            CurrentBlock.CurrentChessPiece = ActivePiece;
            // Reset the materials
            _board.ResetHighlightedBlocks();
            CurrentBlock.AdjustEffect(GridBlockEffect.Selected);
            // Return true since a piece was moved
            return true;
        }
        // Return false since no piece was moved
        return false;
    }

    private (GridBlock, GridBlock) PickPossibleBlock()
    { 
       // Get all the block of the player's color
       List<GridBlock> blocks = _board.GetAllBlocksOfColor(_color);
       // Pick a random block
       int randomBlockIndex = Random.Range(0, blocks.Count - 1);
       // Pick a random position for that block
       List<GridBlock> potentialPos = blocks[randomBlockIndex].CurrentChessPiece.GetPotentialPositions(_board);
       while (potentialPos.Count == 0)
       {
           // If there is no moves for that block, remove it from the list of potential blocks
           blocks.Remove(blocks[randomBlockIndex]);
           // If there are no more blocks, return null for both initial and target blocks
           if (blocks.Count == 0) return (null, null);
           // Pick another random block and position
           randomBlockIndex = Random.Range(0, blocks.Count - 1);
           potentialPos = blocks[randomBlockIndex].CurrentChessPiece.GetPotentialPositions(_board);
       }
       // Found a block with moves, return the initial block and the potential position
       return (blocks[randomBlockIndex], potentialPos[Random.Range(0, potentialPos.Count - 1)]);
    }
}
