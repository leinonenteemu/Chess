using UnityEngine;
using System.Linq;
using System.Collections.Generic;
public class BoardGenerator : MonoBehaviour
{
    [SerializeField] private int width = 8, height = 8;
    private Grid _grid;

    [SerializeField] private bool _drawGizmos;
    Square[,] _squares;

    MovementSets _movementSets = new MovementSets();
    [SerializeField] Piece _testPiece;

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
        //var list = _movementSets.GetSquares(new Vector2Int(2, 2), PieceType.Bishop);
        //var cleanedList = list.Where(coordinate => IsValidSquare(coordinate));

        var newPositionList = getPossibleSquares(_testPiece.GetMovementSets, _squares[3, 6], p);

        foreach (var c in newPositionList)
        {
            Pawn p = new Pawn();
            Debug.Log("CorD: " +c);
            MovePieceToSquare(p, _squares[c.x, c.y]);
        }
        Debug.Log("List count: " + newPositionList.Count());
    }

    private void OnDrawGizmos()
    {
        if (!Application.isPlaying) return;
        if (!_drawGizmos) return;
        foreach (var cell in _squares) 
        {
            Debug.Log("Has occupant? " + cell.occupant != null);
            Gizmos.color = cell.occupant == null ? Color.red : Color.green;
            float radius = !cell.IsOccupied ? 0.25f : 0.15f;
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

    List<Square> getPossibleSquares(in Movement[] movements, Square startSquare, ChessPiece piece) 
    {
        List<Square> availableSquares = new List<Square>();

        foreach (var move in movements) 
        {
            int maxSquares = move.GetMaxSquares();
            for (int i = 1; i <= maxSquares; i++) 
            { 
                int x = move.GetDirection().x * i;
                int y = move.GetDirection().y * i;

                Vector2Int pos = new Vector2Int(startSquare.x + x, startSquare.y + y);
                
                if (IsValidSquare(pos)) 
                {
                    Square square = _squares[pos.x, pos.y];
                    if (square.IsOccupied && square.occupant.IsSameSidePiece(piece)) break;
                    availableSquares.Add(_squares[pos.x,pos.y]); 
                }

                if (move.IsBiDirectional()) 
                {
                    pos = new Vector2Int(startSquare.x - x, startSquare.y - y);
                    if (IsValidSquare(pos))
                    {
                        Square square = _squares[pos.x, pos.y];
                        if (square.IsOccupied && square.occupant.IsSameSidePiece(piece)) break;
                        availableSquares.Add(_squares[pos.x, pos.y]);
                    }
                }
            }
        }

        return availableSquares;
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
    public bool IsOccupied => occupant != null;
}
