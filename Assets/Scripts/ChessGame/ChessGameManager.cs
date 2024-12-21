using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChessGameManager : MonoBehaviour
{
    void Start()
    {
        SpawnBoard();
    }

    private void SpawnBoard()
    {
        GameObject board = Instantiate(Resources.Load<GameObject>("Chess/ChessBoard"));
        board.GetComponent<ChessBoard>().Initialize();
    }
}
