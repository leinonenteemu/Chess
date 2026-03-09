using UnityEngine;
using UnityEngine.InputSystem;

public class InputHandler : MonoBehaviour
{
    public Vector2 mousePos;
    [SerializeField] BoardGenerator _board;
    [SerializeField] GameObject testObject;
    [SerializeField] InputActionAsset _inputActions;
    InputAction mouseInput;

    Transform _movablePiece;

    private void OnEnable()
    {
        var map = _inputActions.FindActionMap("Player");
        map.Enable();
        mouseInput = _inputActions.FindActionMap("Player").FindAction("Mouse0");
    }

    private void OnDisable()
    {
        var map = _inputActions.FindActionMap("Player");
        map.Disable();
    }

    // Update is called once per frame
    void Update()
    {
        mousePos = Mouse.current.position.ReadValue();

        if (mouseInput.WasPressedThisFrame()) 
        { 
            var pos = mousePosToBoardCoordinates(mousePos);

            if (_board.GetSquare(pos, out Square sq)) 
            {
                if (sq.IsOccupied) 
                {
                    Debug.Log("Is occupied");
                    GameObject g = sq.occupant.gameObject;
                    g.transform.position.Set(pos.x, pos.y, g.transform.position.z);
                    _movablePiece = g.transform;
                }
            }
            Debug.Log(pos.ToString());
        }

        if (_movablePiece != null) 
        {
            var pos = Camera.main.ScreenToWorldPoint(mousePos);
            pos.z = -1;
            _movablePiece.transform.position = pos;
        } 
    }

    Vector2Int mousePosToBoardCoordinates(Vector2 mouseScreenPosition) 
    { 
        var coords = Camera.main.ScreenToWorldPoint(mouseScreenPosition);
        coords.z = 0;
        Vector2Int intCoords = new Vector2Int(Mathf.RoundToInt(coords.x-_board.transform.position.x), Mathf.RoundToInt(coords.y-_board.transform.position.y));

        return intCoords;
    }
}
