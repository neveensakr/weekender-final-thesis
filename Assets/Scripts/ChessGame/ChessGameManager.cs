using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChessGameManager : MonoBehaviour
{
    public Vector2 CurrentPlayerPosition;
    public GridBlock CurrentBlock;
    private ChessBoard board;
    
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

        GridBlock newBlock = board.GetBlockAtPos(CurrentPlayerPosition);
        if (CurrentBlock != newBlock)
        {
            CurrentBlock.DisableHover();
            CurrentBlock = newBlock;
            CurrentBlock.EnableHover();
        }

        CurrentBlock = board.GetBlockAtPos(CurrentPlayerPosition);
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
        CurrentBlock.EnableHover();
    }
}
