using UnityEngine;
using System.Linq;
using System.Collections.Generic;
using Unity.VisualScripting;
public class BoardGenerator : MonoBehaviour
{
    [SerializeField] private const int width = 8, height = 8;
    private Grid _grid;

    [SerializeField] private bool _drawGizmos;
    Square[,] _squares;

    MovementSets _movementSets = new MovementSets();
    [SerializeField] Piece _testPiece;
    [SerializeField] SO_BoardSetup _boardSetup;
    private bool IsValidSquare(Vector2Int squareCoords) 
    {
        if (squareCoords.x < 0 || squareCoords.y < 0) return false;
        if (squareCoords.x > width-1 || squareCoords.y > height-1) return false;
        return true;
    }

    public bool GetSquare(Vector2Int squareCoords, out Square square) 
    {
        square = null;
        if (!IsValidSquare(squareCoords)) return false;

        square = _squares[squareCoords.x, squareCoords.y];
        return true;
    }

    private void SetupBoard() 
    {
        foreach (var setup in _boardSetup.pieceSetups) 
        {
            foreach (var position in setup.positions) 
            {
                if (IsValidSquare(position)) 
                {
                    var piece = PieceBuilder.BuildPiece(setup.piece, setup.side);
                    MovePieceToSquare(piece, _squares[position.x,position.y]);
                    GameObject g = new GameObject();
                    var rend = g.GetOrAddComponent<SpriteRenderer>();
                    rend.sprite = piece.sprite;

                    var pos = new Vector3(piece.currentSquare.x+transform.position.x, piece.currentSquare.y+transform.position.y, -1);
                    g.transform.position = pos;
                    piece.SetGameObject(g);
                    Debug.Log("Kys");
                }
            }
        }
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

        SetupBoard();
    }

    List<Square> possibleSquares = new List<Square>();

    private void Start()
    {
        //var list = _movementSets.GetSquares(new Vector2Int(2, 2), PieceType.Bishop);
        //var cleanedList = list.Where(coordinate => IsValidSquare(coordinate));
        //p = new Pawn(_testPiece, Side.White);
        //enemyPawn = new Pawn(_testPiece, Side.White);
        //MovePieceToSquare(enemyPawn, _squares[1,5]);
        //var newPositionList = getPossibleSquares(_testPiece.GetMovementSets, _squares[3, 4], p);
        //possibleSquares = newPositionList;

        ////foreach (var c in newPositionList)
        ////{
        ////    p = new Pawn(_testPiece, Side.White);
        ////    Debug.Log("CorD: " +c);
        ////    MovePieceToSquare(p, _squares[c.x, c.y]);
        ////}
        //Debug.Log("List count: " + newPositionList.Count());
    }

    private void OnDrawGizmos()
    {
        if (!Application.isPlaying) return;
        if (!_drawGizmos) return;
        foreach (var cell in possibleSquares) 
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
        if (piece == null || square == null) return;
        if (piece.currentSquare != null) piece.currentSquare.occupant = null;
        square.occupant = piece;
        piece.currentSquare = square;
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
            bool cutoffDirectional = false;
            bool cutoffBidirectional = false;
            int maxSquares = move.GetMaxSquares();
            for (int i = 1; i <= maxSquares; i++) 
            {
                int x = move.GetDirection().x * i;
                int y = move.GetDirection().y * i;
                Vector2Int pos = new Vector2Int(startSquare.x + x, startSquare.y + y);

                if (!cutoffDirectional) { 
                    if (IsValidSquare(pos))
                    {
                        bool addSquare = true;
                        Square square = _squares[pos.x, pos.y];
                        if (square.IsOccupied)
                        {
                            cutoffDirectional = true;
                            if (square.occupant.IsSameSidePiece(piece)) addSquare = false;
                        }
                        if (addSquare) availableSquares.Add(_squares[pos.x, pos.y]);
                    }

                }

                if (move.IsBiDirectional() && !cutoffBidirectional)
                {
                    pos = new Vector2Int(startSquare.x - x, startSquare.y - y);
                    if (IsValidSquare(pos))
                    {
                        bool addSquare = true;
                        Square square = _squares[pos.x, pos.y];
                        if (square.IsOccupied)
                        {
                            cutoffBidirectional = true;
                            if (square.occupant.IsSameSidePiece(piece))
                            {
                                addSquare = false;
                            }
                        }

                        if (addSquare) availableSquares.Add(_squares[pos.x, pos.y]);
                    }

                }

                if (cutoffDirectional && cutoffBidirectional) break;
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
