public abstract class ChessPiece 
{
    private int ID;
    private string name;
    public readonly Side side;
    public abstract void Move(in Square square);
    public Square currentSquare;

    public readonly Piece properties;

    public bool IsSameSidePiece(ChessPiece other) 
    {
        return side == other.side;
    }

    public ChessPiece(Piece properties, Side side) 
    { 
        this.properties = properties;
        this.side = side;
    }
}

[System.Serializable]
public class Pawn : ChessPiece 
{
    public Pawn(Piece properties, Side side) : base(properties, side)
    {
    }

    public override void Move(in Square square) 
    {
        currentSquare = square;
        square.occupant = this;
    }
}

public enum Row { A,B,C,D,E,F,G,H }
public enum Side { White, Black }

public enum MovementType { Vertical, Horizontal, Diagonal, Knight }
