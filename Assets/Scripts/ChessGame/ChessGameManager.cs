using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChessGameManager : MonoBehaviour
{
    public ChessPlayer player_1;
    public ChessPlayer player_2;
    private ChessBoard board;
    private ChessPlayer _playerCurrentTurn;
    private bool _gameEnded;
    
    void Start()
    {
        SpawnBoard();
        player_1 = new ChessPlayer(board, ChessPieceColor.White, new Vector2(3, 1));
        player_2 = new ChessPlayer(board, ChessPieceColor.Black, new Vector2(3, 6));
        _playerCurrentTurn = player_1;
    }

    private void Update()
    {
        if (!_gameEnded)
        {
            bool pieceMoved = _playerCurrentTurn.Move();
            if (pieceMoved) _playerCurrentTurn = (_playerCurrentTurn == player_1) ? player_2 : player_1;
        
            GridBlock newBlock = board.GetBlockAtPos(_playerCurrentTurn.CurrentPosition);
            if (_playerCurrentTurn.CurrentBlock != newBlock)
            {
                SetHoverEffect(_playerCurrentTurn.CurrentBlock, newBlock, _playerCurrentTurn.ActivePiece);
                _playerCurrentTurn.CurrentBlock = newBlock;
            }
        }
    }

    private void SpawnBoard()
    {
        board = Instantiate(Resources.Load<GameObject>("Chess/ChessBoard")).GetComponent<ChessBoard>();
        board.Initialize();
        board.onKingKill.AddListener(EndGame);
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
        _gameEnded = true;
    }
}
