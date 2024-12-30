using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ChessBoard : MonoBehaviour
{
    // Event triggered when the King dies
    public UnityEvent onKingKill = new UnityEvent();
    // Blocks showing the potential movements
    public List<GridBlock> HighlightedBlocks = new List<GridBlock>();
    
    // GridBlocks and Pieces on the board
    private GridBlock[] _gridBlocks = new GridBlock[64];
    private ChessPiece[] _pieces = new ChessPiece[32];
    // The piece order on the board
    private readonly Dictionary<int, ChessPieceName> _pieceOrder = new() {
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
        // Set up the 8x8 chess board with alternating colors
        for (int y = 0; y < 8; y++)
        {
            for (int x = 0; x < 8; x++)
            {
                int index = ChessGameHelperFunctions.GetIndexByPosition(x, y, 8);
                _gridBlocks[index] = Instantiate(Resources.Load<GameObject>("Chess/GridBlock"),
                    transform, false).GetComponent<GridBlock>();
                ChessPieceColor color = ChessGameHelperFunctions.GetGridBlockColor(new Vector2(x, y));
                _gridBlocks[index].Setup(new Vector3(x, -0.1f, y), color);
            }
        }
        
        SpawnPieces();
    }

    private void SpawnPieces()
    {
        // White Row
        for (int i = 0; i < 8; i++) { AddPiece(ChessPieceColor.White, i, 0, _pieceOrder[i]); }
        // White Pawns
        for (int i = 8; i < 16; i++) { AddPiece(ChessPieceColor.White, (i - 8), 1, ChessPieceName.Pawn); }
        // Black Row
        for (int i = 16; i < 24; i++) { AddPiece(ChessPieceColor.Black, i-16, 7, _pieceOrder[i-16]); }
        // Black Pawns
        for (int i = 24; i < 32; i++) { AddPiece(ChessPieceColor.Black, (i-24), 6, ChessPieceName.Pawn); }
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
        foreach (GridBlock block in blocks) { block.AdjustEffect(GridBlockEffect.PotentialPosition); }
    }

    public void ResetHighlightedBlocks()
    {
        foreach (GridBlock block in HighlightedBlocks) { block.AdjustEffect(GridBlockEffect.Normal); }
        HighlightedBlocks.Clear();
    }

    private void PieceKilled(ChessPiece piece)
    {
        if (piece is ChessKing) onKingKill.Invoke();
    }

    public List<GridBlock> GetAllBlocksOfColor(ChessPieceColor color)
    {
        List<GridBlock> blocks = new List<GridBlock>();
        
        foreach (GridBlock block in _gridBlocks)
        {
            if (block.CurrentChessPiece && block.CurrentChessPiece.Color == color)
                blocks.Add(block);
        }

        return blocks;
    }

    private void AddPiece(ChessPieceColor color, int x, int y, ChessPieceName piece)
    {
        int index = ChessGameHelperFunctions.GetIndexByPosition(x, y, 8);
        _pieces[x] = Instantiate(Resources.Load<GameObject>("Chess/ChessPiece_" + piece), 
            transform, false).GetComponent<ChessPiece>();
        _pieces[x].Setup(new Vector3(x, 0.5f, y), color);
        _pieces[x].onKill.AddListener(PieceKilled);
        _gridBlocks[index].CurrentChessPiece = _pieces[x];
    }
}
