using System.Collections.Generic;
using System.Reflection;
using System.Runtime.InteropServices.WindowsRuntime;
using Unity.Collections;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEditor.Searcher.SearcherWindow.Alignment;

public class MovementSets
{
    private bool CheckBounds(Vector2Int coordinates) => ((coordinates.x >= 0 && coordinates.x <= 8) && (coordinates.y >= 0 && coordinates.y <= 8));



    public List<Vector2Int> GetSquares(Vector2Int originSquare, PieceType type) 
    {
        List<Vector2Int> possibleSquares = new List<Vector2Int>();

        switch (type) 
        {
            case PieceType.Knight: 
                {
                    int x = 0;
                    int y = 0;

                    for (int i = -1; i <= 1; i+=2)
                    {
                        x = i;
                        y = i * 2;

                        Vector2Int horizontal = new Vector2Int(originSquare.x + x, originSquare.y + y);
                        Vector2Int vertical = new Vector2Int(originSquare.x + y, originSquare.y + x);

                        if (CheckBounds(horizontal)) possibleSquares.Add(horizontal);
                        if (CheckBounds(vertical)) possibleSquares.Add(vertical);

                        horizontal = new Vector2Int(originSquare.x - x, originSquare.y + y);
                        vertical = new Vector2Int(originSquare.x - y, originSquare.y + x);

                        if (CheckBounds(horizontal)) possibleSquares.Add(horizontal);
                        if (CheckBounds(vertical)) possibleSquares.Add(vertical);

                    }

                    break;
                }

            case PieceType.Rook: 
                {
                    for (int i = 0; i < 8; i++)
                    {
                        Vector2Int vertical = new Vector2Int(originSquare.x, i);
                        Vector2Int horizontal = new Vector2Int(i, originSquare.y);
                        Debug.Log("Horizontal: " + vertical);
                        if (CheckBounds(horizontal)) 
                        {
                            Debug.Log("True horizontal");
                            possibleSquares.Add(horizontal);
                        }
                        if (CheckBounds(vertical)) 
                        {
                            Debug.Log("True vertical");
                            possibleSquares.Add(vertical);
                        }
                    }
                    break;
                }

            case PieceType.Bishop: 
                {
                    bool verticalFirst = Mathf.Min(originSquare.x, originSquare.y) == originSquare.y; //if verticalFirst is true, the originpoints y value is smaller than the x
                    int dif = verticalFirst ? originSquare.x - originSquare.y : originSquare.y - originSquare.x; //difference between the max and min value, use this as offset for the value that doesnt start from 0 index

                    int difToMax = verticalFirst ? 7 - originSquare.x : 7 - originSquare.y;
                    int counterClockwiseOffset = verticalFirst ? 7 - originSquare.x : 7 - originSquare.y;

                    Vector2Int start = verticalFirst ? new Vector2Int(dif, 0) : new Vector2Int(0, dif); //if original y value was closer to 0, we apply offset to x and vice versa.

                    Vector2Int counterStart = new Vector2Int(originSquare.x,originSquare.y);

                    //decrement the south-east directional until it hits left or upper side of grid
                    //this might be completely braindead change it later
                    while (counterStart.x >= 0 && counterStart.y <= 7) 
                    {
                        counterStart += new Vector2Int(-1, 1);
                    }

                    for (int i = 0; i < 8; i++) 
                    {
                        Vector2Int clockwise = new Vector2Int(start.x+i, start.y+i);       
                        if (CheckBounds(clockwise)) possibleSquares.Add(clockwise);

                        Vector2Int counter = new Vector2Int(counterStart.x+i, counterStart.y-i);
                        if (CheckBounds(counter)) possibleSquares.Add(counter);
                    }

                    break;
                }
        }

        return possibleSquares;
    }
}

public enum PieceType 
{ 
    Knight, Rook, Bishop
}
