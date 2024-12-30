using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChessGameManager : MonoBehaviour
{
    public static ChessGameManager Instance;
    public bool GameEnded { get; private set; }
    
    // Player Instances
    private ChessPlayer _player1;
    private ChessPlayer _player2;
    private ChessPlayer _playerCurrentTurn;
    // The Chessboard
    private ChessBoard _board;
    // Flag set to true when the game starts
    private bool _gameStarted;

    void Start()
    {
        Instance = this;
    }

    public void StartGame()
    {
        // Initialize the Board and Players, setting Black to be an AI.
        SpawnBoard();
        _player1 = new ChessPlayer(_board, ChessPieceColor.White, new Vector2(3, 1), false);
        _player2 = new ChessPlayer(_board, ChessPieceColor.Black, new Vector2(3, 6), true);
        _playerCurrentTurn = _player1;
        _gameStarted = true;
    }
    
    public void ResetGame()
    {
        // Initialize the Board and Players, setting Black to be an AI.
        if (_board.gameObject) Destroy(_board.gameObject);
        GameEnded = false;
    }

    private void Update()
    {
        // Only move if the game didn't end
        if (_gameStarted && !GameEnded)
        {
            // Switch turns only when a piece is moved.
            bool pieceMoved = _playerCurrentTurn.Move();
            if (pieceMoved) _playerCurrentTurn = (_playerCurrentTurn == _player1) ? _player2 : _player1;
            // Update the effect on the block the player is on, if they moved
            GridBlock newBlock = _board.GetBlockAtPos(_playerCurrentTurn.CurrentPosition);
            if (_playerCurrentTurn.CurrentBlock != newBlock)
            {
                SetHoverEffect(_playerCurrentTurn.CurrentBlock, newBlock, _playerCurrentTurn.ActivePiece);
                _playerCurrentTurn.CurrentBlock = newBlock;
            }
        }
    }

    private void SpawnBoard()
    {
        // Spawn and Initialize the board
        _board = Instantiate(Resources.Load<GameObject>("Chess/ChessBoard"), transform, true).GetComponent<ChessBoard>();
        _board.transform.localPosition = Vector3.zero;
        _board.transform.localScale = Vector3.one;
        _board.Initialize();
        // Trigger the EndGame function when the king dies
        _board.onKingKill.AddListener(EndGame);
    }

    private void SetHoverEffect(GridBlock currentBlock, GridBlock nextBlock, ChessPiece activePiece)
    {
        // If the player was on a selected block, keep it Selected
        if (currentBlock.CurrentChessPiece == activePiece && activePiece) 
            currentBlock.AdjustEffect(GridBlockEffect.Selected);
        // If the player was on a potential position block, keep it Highlighted
        else if (_board.HighlightedBlocks.Contains(_board.GetBlockAtPos(currentBlock.Position)))
            currentBlock.AdjustEffect(GridBlockEffect.PotentialPosition);
        // Otherwise, change the color back to Normal
        else currentBlock.AdjustEffect(GridBlockEffect.Normal);
        // Highlight the next block
        nextBlock.AdjustEffect(GridBlockEffect.Hover);
    }

    private void EndGame()
    {
        Debug.Log("[ChessGameManager - EndGame] Game Ended");
        GameEnded = true;
        _gameStarted = false;
    }
}
