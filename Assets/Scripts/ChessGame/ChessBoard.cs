using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChessBoard : MonoBehaviour
{
    public List<GridBlock> HighlightedBlocks = new List<GridBlock>();
    
    private GridBlock[] _gridBlocks = new GridBlock[64];
    private ChessPiece[] _pieces = new ChessPiece[32];

    private Dictionary<int, ChessPieceName> pieceOrder = new() {
        {0, ChessPieceName.Rook},
        {1, ChessPieceName.Knight},
        {2, ChessPieceName.Bishop},
        {3, ChessPieceName.Queen},
        {4, ChessPieceName.King},
        {5, ChessPieceName.Bishop},
        {6, ChessPieceName.Knight},
        {7, ChessPieceName.Rook},
    };

    public void Initialize()
    {
        for (int y = 0; y < 8; y++)
        {
            for (int x = 0; x < 8; x++)
            {
                int index = ChessGameHelperFunctions.GetIndexByPosition(x, y, 8);
                _gridBlocks[index] = Instantiate(Resources.Load<GameObject>("Chess/GridBlock"),
                    transform, false).GetComponent<GridBlock>();
                ChessPieceColor color = ChessGameHelperFunctions.GetGridBlockColor(new Vector2(x, y));
                _gridBlocks[index].Setup(index, new Vector3(x, -0.1f, y), color);
            }
        }
        
        SpawnPieces();
    }

    private void SpawnPieces()
    {
        // White Row
        for (int i = 0; i < 8; i++)
        {
            int index = ChessGameHelperFunctions.GetIndexByPosition(i, 0, 8);
            _pieces[i] = Instantiate(Resources.Load<GameObject>("Chess/ChessPiece_" + pieceOrder[i]), 
                transform, false).GetComponent<ChessPiece>();
            _pieces[i].Setup(new Vector3(i, 0.5f, 0f), ChessPieceColor.White);
            _gridBlocks[index].CurrentChessPiece = _pieces[i];
        }
        
        // White Pawns
        for (int i = 8; i < 16; i++)
        {
            int index = ChessGameHelperFunctions.GetIndexByPosition((i-8), 1, 8);
            _pieces[i] = Instantiate(Resources.Load<GameObject>("Chess/ChessPiece_Pawn"), 
                transform, false).GetComponent<ChessPiece>();
            _pieces[i].Setup(new Vector3((i-8), 0.5f, 1f), ChessPieceColor.White);
            _gridBlocks[index].CurrentChessPiece = _pieces[i];
        }
        
        // Black Row
        for (int i = 16; i < 24; i++)
        {
            int index = ChessGameHelperFunctions.GetIndexByPosition((i-16), 7, 8);
            _pieces[i] = Instantiate(Resources.Load<GameObject>("Chess/ChessPiece_" + pieceOrder[i-16]), 
                transform, false).GetComponent<ChessPiece>();
            _pieces[i].Setup(new Vector3((i-16), 0.5f, 7f), ChessPieceColor.Black);
            _gridBlocks[index].CurrentChessPiece = _pieces[i];
        }
        
        // Black Pawns
        for (int i = 24; i < 32; i++)
        {
            int index = ChessGameHelperFunctions.GetIndexByPosition((i-24), 6, 8);
            _pieces[i] = Instantiate(Resources.Load<GameObject>("Chess/ChessPiece_Pawn"), 
                transform, false).GetComponent<ChessPiece>();
            _pieces[i].Setup(new Vector3((i-24), 0.5f, 6f), ChessPieceColor.Black);
            _gridBlocks[index].CurrentChessPiece = _pieces[i];
        }
    }

    public GridBlock GetBlockAtPos(Vector2 position)
    {
        int index = ChessGameHelperFunctions.GetIndexByPosition((int) position.x, (int) position.y, 8);
        return _gridBlocks[index];
    }

    public void HighlightBlocks(List<GridBlock> blocks)
    {
        ResetHighlightedBlocks();
        HighlightedBlocks = blocks;
        foreach (GridBlock block in blocks)
        {
            block.AdjustEffect(GridBlockEffect.PotentialPosition);
        }
    }

    public void ResetHighlightedBlocks()
    {
        foreach (GridBlock block in HighlightedBlocks)
        {
            block.AdjustEffect(GridBlockEffect.Normal);
        }
        HighlightedBlocks.Clear();
    }
}
