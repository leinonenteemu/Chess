using UnityEngine;
using System.Linq;
public class BoardGenerator : MonoBehaviour
{
    [SerializeField] private int width = 8, height = 8;
    private Grid _grid;

    [SerializeField] private bool _drawGizmos;
    Square[,] _squares;

    MovementSets _movementSets = new MovementSets();

    private bool IsValidSquare(Vector2Int squareCoords) 
    {
        if (squareCoords.x < 0 || squareCoords.y < 0) return false;
        if (squareCoords.x > width-1 || squareCoords.y > height-1) return false;
        return true;
    }

    private void Awake()
    {
        _squares = new Square[width, height];
        for (int col = 0; col < width; col++) 
        {
            for (int row = 0; row < height; row++) 
            {
                Square square = new Square(col,row);
                _squares[col, row] = square;
            }
        }
    }

    Pawn p = new Pawn();

    private void Start()
    {
        var list = _movementSets.GetSquares(new Vector2Int(2, 2), PieceType.Bishop);
        var cleanedList = list.Where(coordinate => IsValidSquare(coordinate));
        foreach (var c in cleanedList)
        {
            Pawn p = new Pawn();
            Debug.Log("CorD: " +c);
            MovePieceToSquare(p, _squares[c.x, c.y]);
        }
        Debug.Log("List count: " + cleanedList.Count());
    }

    private void OnDrawGizmos()
    {
        if (!Application.isPlaying) return;
        if (!_drawGizmos) return;
        foreach (var cell in _squares) 
        {
            Debug.Log("Has occupant? " + cell.occupant != null);
            Gizmos.color = cell.occupant == null ? Color.red : Color.green;
            float radius = cell.occupant == null ? 0.25f : 0.15f;
            //Vector3 worldPos = new Vector3(transform.position.x+cell.position.x, transform.position.y+cell.position.y, 0);
            Gizmos.DrawSphere(new Vector2(transform.position.x+cell.x,transform.position.y+cell.y), radius);
        }
    }

    void MovePieceToSquare(ChessPiece piece, Square square) 
    {
        piece.Move(square);
        square.occupant = piece;
    }

    void ClearSquare(Square square) 
    {
        square.occupant = null;
    }
}

[System.Serializable]
public class Square 
{
    public int x, y;
    public Square(int x, int y)
    {
        this.x = x;
        this.y = y;
    }

    public ChessPiece occupant;
}
