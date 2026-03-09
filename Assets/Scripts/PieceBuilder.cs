using UnityEngine;

public static class PieceBuilder
{
    public static ChessPiece BuildPiece(Piece properties, Side side) 
    {
        return new ChessPiece(properties, side);
    }
}
