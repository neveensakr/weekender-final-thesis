using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChessBoard : MonoBehaviour
{
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
        SpawnPieces();
    }

    private void SpawnPieces()
    {
        // White Row
        for (int i = 0; i < 8; i++)
        {
            _pieces[i] = Instantiate(Resources.Load<GameObject>("Chess/ChessPiece_" + pieceOrder[i]), 
                transform, true).GetComponent<ChessPiece>();
            _pieces[i].Setup(new Vector3(i * 1.25f, 0.5f, 0f), ChessPieceColor.White);
        }
        
        // White Pawns
        for (int i = 8; i < 16; i++)
        {
            _pieces[i] = Instantiate(Resources.Load<GameObject>("Chess/ChessPiece_Pawn"), 
                transform, true).GetComponent<ChessPiece>();
            _pieces[i].Setup(new Vector3((i-8) * 1.25f, 0.5f, 1.35f), ChessPieceColor.White);
        }
        
        // Black Row
        for (int i = 16; i < 24; i++)
        {
            _pieces[i] = Instantiate(Resources.Load<GameObject>("Chess/ChessPiece_" + pieceOrder[i-16]), 
                transform, true).GetComponent<ChessPiece>();
            _pieces[i].Setup(new Vector3((i-16) * 1.25f, 0.5f, 8.75f), ChessPieceColor.Black);
        }
        
        // Black Pawns
        for (int i = 24; i < 32; i++)
        {
            _pieces[i] = Instantiate(Resources.Load<GameObject>("Chess/ChessPiece_Pawn"), 
                transform, true).GetComponent<ChessPiece>();
            _pieces[i].Setup(new Vector3((i-24) * 1.25f, 0.5f, 7.6f), ChessPieceColor.Black);
        }
    }
}
