using UnityEngine;

[CreateAssetMenu(fileName = "Chess Piece", menuName = "Scriptable Objects/New Chess Piece")]
public class Piece : ScriptableObject
{
    [SerializeField] Movement[] movement;
    public Movement[] GetMovementSets => movement;

    [SerializeField] Sprite icon;
    public Sprite Icon => icon;
}

[System.Serializable]
public struct Movement 
{
    [SerializeField] Vector2Int direction;
    [SerializeField] bool biDirectional;
    [SerializeField] int maxSquares;

    public Vector2Int GetDirection() => direction;
    public bool IsBiDirectional() => biDirectional;
    public int GetMaxSquares() => maxSquares;
}
