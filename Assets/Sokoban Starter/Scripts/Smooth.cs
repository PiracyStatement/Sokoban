using System;
using UnityEngine;

public class Smooth : MonoBehaviour
{
    [SerializeField] private GameObject grid;
    private GridManager gridManager;

    public Vector2Int gridPosition = new Vector2Int(0, 0);

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        grid = GameObject.Find("Grid");
        gridManager = grid.GetComponent<GridManager>();
        gridPosition.x = Convert.ToInt32(this.transform.position.x + 4.5f);
        gridPosition.y = Mathf.Abs(Convert.ToInt32(this.transform.position.y - 2f));
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public bool PushBlock(string dir) //returns true if this block can be moved
    {
        if (gridManager.BasicMoveCheck(gridPosition, dir, false))
        {
            if (dir == "left")
            {
                gridPosition.x += -1;
                UpdatePosition();
            }
            else if (dir == "right")
            {
                gridPosition.x += 1;
                UpdatePosition();
            }
            else if (dir == "up")
            {
                gridPosition.y += -1;
                UpdatePosition();
            }
            else if (dir == "down")
            {
                gridPosition.y += 1;
                UpdatePosition();
            }

            return true;
        }
        
        return false;
    }
    private void UpdatePosition()
    {
        float x = GridMaker.reference.TopLeft.x + GridMaker.reference.cellWidth * (gridPosition.x + 1 - 0.5f);
        float y = GridMaker.reference.TopLeft.y - GridMaker.reference.cellWidth * (gridPosition.y + 1 - 0.5f);
        this.transform.position = new Vector3(x, y, 0);

        gridManager.UpdatePositionArray();
    }
}
