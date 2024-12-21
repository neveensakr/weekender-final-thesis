using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChessBoard : MonoBehaviour
{
    private GridBlock[,] _gridBlocks = new GridBlock[8,8];
    private ChessPiece[] _pieces = new ChessPiece[32];

    private Dictionary<int, ChessPieceName> pieceOrder = new() {
        {0, ChessPieceName.Rook},
        {1, ChessPieceName.Knight},
        {2, ChessPieceName.Bishop},
        {3, ChessPieceName.King},
        {4, ChessPieceName.Queen},
        {5, ChessPieceName.Bishop},
        {6, ChessPieceName.Knight},
        {7, ChessPieceName.Rook},
    };

    public void Initialize()
    {
        for (int x = 0; x < 8; x++)
        {
            for (int y = 0; y < 8; y++)
            {
                _gridBlocks[x,y] = Instantiate(Resources.Load<GameObject>("Chess/GridBlock"),
                    transform, false).GetComponent<GridBlock>();
                ChessPieceColor color = ChessGameHelperFunctions.GetGridBlockColor(new Vector2(x, y));
                _gridBlocks[x,y].Setup(new Vector3(x, -0.1f, y), color);
            }
        }
        
        SpawnPieces();
    }

    private void SpawnPieces()
    {
        // White Row
        for (int i = 0; i < 8; i++)
        {
            _pieces[i] = Instantiate(Resources.Load<GameObject>("Chess/ChessPiece_" + pieceOrder[i]), 
                transform, false).GetComponent<ChessPiece>();
            _pieces[i].Setup(new Vector3(i, 0.5f, 0f), ChessPieceColor.White);
            Vector2 chessGridBlockPos = ChessGameHelperFunctions.GetPositionByIndex(i, 8, 8);
            _gridBlocks[(int) chessGridBlockPos.x, (int) chessGridBlockPos.y].CurrentChessPiece = _pieces[i];
        }
        
        // White Pawns
        for (int i = 8; i < 16; i++)
        {
            _pieces[i] = Instantiate(Resources.Load<GameObject>("Chess/ChessPiece_Pawn"), 
                transform, false).GetComponent<ChessPiece>();
            _pieces[i].Setup(new Vector3((i-8), 0.5f, 1f), ChessPieceColor.White);
            Vector2 chessGridBlockPos = ChessGameHelperFunctions.GetPositionByIndex(i, 8, 8);
            _gridBlocks[(int) chessGridBlockPos.x, (int) chessGridBlockPos.y].CurrentChessPiece = _pieces[i];
        }
        
        // Black Row
        for (int i = 16; i < 24; i++)
        {
            _pieces[i] = Instantiate(Resources.Load<GameObject>("Chess/ChessPiece_" + pieceOrder[i-16]), 
                transform, false).GetComponent<ChessPiece>();
            _pieces[i].Setup(new Vector3((i-16), 0.5f, 7f), ChessPieceColor.Black);
            Vector2 chessGridBlockPos = ChessGameHelperFunctions.GetPositionByIndex(i, 8, 8);
            _gridBlocks[(int) chessGridBlockPos.x, (int) chessGridBlockPos.y].CurrentChessPiece = _pieces[i];
        }
        
        // Black Pawns
        for (int i = 24; i < 32; i++)
        {
            _pieces[i] = Instantiate(Resources.Load<GameObject>("Chess/ChessPiece_Pawn"), 
                transform, false).GetComponent<ChessPiece>();
            _pieces[i].Setup(new Vector3((i-24), 0.5f, 6f), ChessPieceColor.Black);
            Vector2 chessGridBlockPos = ChessGameHelperFunctions.GetPositionByIndex(i, 8, 8);
            _gridBlocks[(int) chessGridBlockPos.x, (int) chessGridBlockPos.y].CurrentChessPiece = _pieces[i];
        }
    }

    public GridBlock GetBlockAtPos(Vector2 position)
    {
        return _gridBlocks[(int) position.x, (int) position.y];
    }
}
