using UnityEngine;

[CreateAssetMenu(fileName = "SO_BoardSetup", menuName = "Scriptable Objects/SO_BoardSetup")]
public class SO_BoardSetup : ScriptableObject
{
    public PieceSetup[] pieceSetups = new PieceSetup[0];
}

[System.Serializable]
public class PieceSetup
{
    public Piece piece;
    public Side side;
    public Vector2Int[] positions;
}


