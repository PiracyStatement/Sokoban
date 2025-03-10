using System;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private GameObject grid;
    private GridManager gridManager;

    public Vector2Int gridPosition = new Vector2Int(0, 0);

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        grid = GameObject.Find("Grid");
        gridManager = grid.GetComponent<GridManager>();
        //correcting in-game coords to correspond to grid coords
        gridPosition.x = Convert.ToInt32(this.transform.position.x + 4.5f);
        gridPosition.y = Mathf.Abs(Convert.ToInt32(this.transform.position.y - 2f));
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            MovePlayer("left");
        }
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            MovePlayer("right");
        }
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            MovePlayer("up");
        }
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            MovePlayer("down");
        }
    }

    public void MovePlayer(string dir)
    {
        if (dir == "left")
        {
            if (gridManager.BasicMoveCheck(gridPosition, "left", false))
            {
                gridPosition.x += -1;
                UpdatePosition();
            }
            
        }
        else if (dir == "right")
        {
            if (gridManager.BasicMoveCheck(gridPosition, "right", false))
            {
                gridPosition.x += 1;
                UpdatePosition();
            }
        }
        else if (dir == "up")
        {
            if (gridManager.BasicMoveCheck(gridPosition, "up", false))
            {
                gridPosition.y += -1;
                UpdatePosition();
            }
        }
        else if (dir == "down")
        {
            if (gridManager.BasicMoveCheck(gridPosition, "down", false))
            {
                gridPosition.y += 1;
                UpdatePosition();
            }
        }
    }

    //visually updates position on grid according to your gridPosition
    private void UpdatePosition()
    {
        float x = GridMaker.reference.TopLeft.x + GridMaker.reference.cellWidth * (gridPosition.x + 1 - 0.5f);
        float y = GridMaker.reference.TopLeft.y - GridMaker.reference.cellWidth * (gridPosition.y + 1 - 0.5f);
        this.transform.position = new Vector3(x, y, 0);

        gridManager.UpdatePositionArray();
    }
}
