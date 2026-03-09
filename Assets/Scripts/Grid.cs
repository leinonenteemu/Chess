using UnityEngine;

public class ChessPiece 
{
    public readonly Side side;
    public Square currentSquare;
    public readonly Piece properties;
    public Sprite sprite;

    public GameObject gameObject;

    public bool IsSameSidePiece(ChessPiece other) 
    {
        return side == other.side;
    }

    public void SetGameObject(GameObject g) => gameObject = g;

    public ChessPiece(Piece properties, Side side) 
    { 
        this.properties = properties;
        this.side = side;
        this.sprite = properties.Icon;
    }

    public void SetSquare(Square square) => this.currentSquare = square;
}

//[System.Serializable]
//public class Pawn : ChessPiece 
//{
//    public Pawn(Piece properties, Side side) : base(properties, side)
//    {
//    }

//    public override void Move(in Square square) 
//    {
//        currentSquare = square;
//        square.occupant = this;
//    }
//}

public enum Row { A,B,C,D,E,F,G,H }
public enum Side { White, Black }

public enum MovementType { Vertical, Horizontal, Diagonal, Knight }
