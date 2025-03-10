using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    public GameObject[,] gridPlacements = new GameObject[10, 5];

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        UpdatePositionArray();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
    }

    public void UpdatePositionArray()
    {
        for (int y = 0; y < gridPlacements.GetLength(1); y++)
        {
            for (int x = 0; x < gridPlacements.GetLength(0); x++)
            {
                gridPlacements[x, y] = null;
            }
        }

        foreach (GameObject obj in FindObjectsOfType(typeof(GameObject)))
        {
            if (obj.GetComponent<GridObject>() != null)
            {
                int objX = Convert.ToInt32(obj.transform.position.x + 4.5f);
                int objY = Mathf.Abs(Convert.ToInt32(obj.transform.position.y - 2f));

                //print(obj.name + ": " + objX + ", " + objY);
                gridPlacements[objX, objY] = obj;
            }
        }
    }

    //returns true if the space you are moving into is empty
    //if you are moving into a pushable block, call this function from that block to see if it can move
    //this way, if it can't move, you can't move either
    public bool BasicMoveCheck(Vector2Int gridPosition, string dir, bool isSticky)
    {
        //basic grid movement checks
        if (dir == "left" && gridPosition.x == 0)
        {
            return false;
        }

        if (dir == "right" && gridPosition.x == 9)
        {
            return false;
        }

        if (dir == "up" && gridPosition.y == 0)
        {
            return false;
        }

        if (dir == "down" && gridPosition.y == 4)
        {
            return false;
        }

        //checking blocks
        //these variables represent spaces orthogonally around subject
        GameObject blockUp = null;
        GameObject blockDown = null;
        GameObject blockLeft = null;
        GameObject blockRight = null;

        if (gridPosition.y > 0)
        {
            blockUp = gridPlacements[gridPosition.x, gridPosition.y - 1];
        }
        if (gridPosition.y < 4)
        {
            blockDown = gridPlacements[gridPosition.x, gridPosition.y + 1];
        }
        if (gridPosition.x > 0)
        {
            blockLeft = gridPlacements[gridPosition.x - 1, gridPosition.y];
        }
        if (gridPosition.x < 9)
        {
            blockRight = gridPlacements[gridPosition.x + 1, gridPosition.y];
        }

        //normal block
        if (dir == "left" && blockLeft != null)
        {
            if (blockLeft.tag == "Wall")
            {
                return false;
            }
            else if (blockLeft.tag == "Smooth")
            {
                return blockLeft.GetComponent<Smooth>().PushBlock("left");
            }
            else if (blockLeft.tag == "Sticky" && blockLeft.GetComponent<Sticky>().isInMotion == false)
            {
                return blockLeft.GetComponent<Sticky>().PushBlock("left");
            }
            else if (blockLeft.tag == "Clingy")
            {
                return false;
            }
        }
        if (dir == "left")
        {
            if (isSticky == false)
            {
                if (blockRight != null && blockRight.tag == "Sticky")
                {
                    //if the sticky is already being moved, don't trigger its pull, so we don't move twice or worse on a loop
                    //for that we check isInMotion
                    if (blockRight.GetComponent<Sticky>().isInMotion)
                    {
                        return true;
                    }
                    else
                    {
                        return blockRight.GetComponent<Sticky>().PullBlock("left");
                    }
                }
                else if (blockRight != null && blockRight.tag == "Clingy")
                {
                    if (blockRight.GetComponent<Clingy>().isInMotion)
                    {
                        return true;
                    }
                    else
                    {
                        return blockRight.GetComponent<Clingy>().PushBlock("left");
                    }
                }
                if (blockUp != null && blockUp.tag == "Sticky")
                {
                    if (blockUp.GetComponent<Sticky>().isInMotion)
                    {
                        return true;
                    }
                    else
                    {
                        return blockUp.GetComponent<Sticky>().PullBlock("left");
                    }
                }
                if (blockDown != null && blockDown.tag == "Sticky")
                {
                    if (blockDown.GetComponent<Sticky>().isInMotion)
                    {
                        return true;
                    }
                    else
                    {
                        return blockDown.GetComponent<Sticky>().PullBlock("left");
                    }
                }
            }
            else //if we're sticky
            {
                if (blockRight != null)
                {
                    if (blockRight.tag == "Smooth")
                    {
                        blockRight.GetComponent<Smooth>().PushBlock("left");
                    }
                    else if (blockRight.tag == "Sticky")
                    {
                        if (blockRight.GetComponent<Sticky>().isInMotion)
                        {
                            return true;
                        }
                        else
                        {
                            return blockRight.GetComponent<Sticky>().PullBlock("left");
                        }
                    }
                    else if (blockRight.tag == "Clingy")
                    {
                        if (blockRight.GetComponent<Clingy>().isInMotion)
                        {
                            return true;
                        }
                        else
                        {
                            return blockRight.GetComponent<Clingy>().PushBlock("left");
                        }
                    }
                }
                if (blockUp != null)
                {
                    if (blockUp.tag == "Smooth")
                    {
                        blockUp.GetComponent<Smooth>().PushBlock("left");
                    }
                    else if (blockUp.tag == "Sticky")
                    {
                        if (blockUp.GetComponent<Sticky>().isInMotion)
                        {
                            return true;
                        }
                        else
                        {
                            return blockUp.GetComponent<Sticky>().PullBlock("left");
                        }
                    }
                }
                if (blockDown != null)
                {
                    if (blockDown.tag == "Smooth")
                    {
                        blockDown.GetComponent<Smooth>().PushBlock("left");
                    }
                    else if (blockDown.tag == "Sticky")
                    {
                        if (blockDown.GetComponent<Sticky>().isInMotion)
                        {
                            return true;
                        }
                        else
                        {
                            return blockDown.GetComponent<Sticky>().PullBlock("left");
                        }
                    }
                }
            }
        }

        if (dir == "right" && blockRight != null)
        {
            if (blockRight.tag == "Wall")
            {
                return false;
            }
            else if (blockRight.tag == "Smooth")
            { 
                return blockRight.GetComponent<Smooth>().PushBlock("right");
            }
            else if (blockRight.tag == "Sticky" && blockRight.GetComponent<Sticky>().isInMotion == false)
            {
                return blockRight.GetComponent<Sticky>().PushBlock("right");
            }
            else if (blockRight.tag == "Clingy")
            {
                return false;
            }
        }
        if (dir == "right")
        {
            if (isSticky == false)
            {
                if (blockLeft != null && blockLeft.tag == "Sticky")
                {
                    if (blockLeft.GetComponent<Sticky>().isInMotion)
                    {
                        return true;
                    }
                    else
                    {
                        return blockLeft.GetComponent<Sticky>().PullBlock("right");
                    }
                }
                else if (blockLeft != null && blockLeft.tag == "Clingy")
                {
                    if (blockLeft.GetComponent<Clingy>().isInMotion)
                    {
                        return true;
                    }
                    else
                    {
                        return blockLeft.GetComponent<Clingy>().PushBlock("right");
                    }
                }
                if (blockUp != null && blockUp.tag == "Sticky")
                {
                    if (blockUp.GetComponent<Sticky>().isInMotion)
                    {
                        return true;
                    }
                    else
                    {
                        return blockUp.GetComponent<Sticky>().PullBlock("right");
                    }
                }
                if (blockDown != null && blockDown.tag == "Sticky")
                {
                    if (blockDown.GetComponent<Sticky>().isInMotion)
                    {
                        return true;
                    }
                    else
                    {
                        return blockDown.GetComponent<Sticky>().PullBlock("right");
                    }
                }
            }
            else
            {
                if (blockLeft != null)
                {
                    if (blockLeft.tag == "Smooth")
                    {
                        blockLeft.GetComponent<Smooth>().PushBlock("right");
                    }
                    else if (blockLeft.tag == "Sticky")
                    {
                        if (blockLeft.GetComponent<Sticky>().isInMotion)
                        {
                            return true;
                        }
                        else
                        {
                            return blockLeft.GetComponent<Sticky>().PullBlock("right");
                        }
                    }
                    else if (blockLeft.tag == "Clingy")
                    {
                        if (blockLeft.GetComponent<Clingy>().isInMotion)
                        {
                            return true;
                        }
                        else
                        {
                            return blockLeft.GetComponent<Clingy>().PushBlock("right");
                        }
                    }
                }
                if (blockUp != null)
                {
                    if (blockUp.tag == "Smooth")
                    {
                        blockUp.GetComponent<Smooth>().PushBlock("right");
                    }
                    else if (blockUp.tag == "Sticky")
                    {
                        if (blockUp.GetComponent<Sticky>().isInMotion)
                        {
                            return true;
                        }
                        else
                        {
                            return blockUp.GetComponent<Sticky>().PullBlock("right");
                        }
                    }
                }
                if (blockDown != null)
                {
                    if (blockDown.tag == "Smooth")
                    {
                        blockDown.GetComponent<Smooth>().PushBlock("right");
                    }
                    else if (blockDown.tag == "Sticky")
                    {
                        if (blockDown.GetComponent<Sticky>().isInMotion)
                        {
                            return true;
                        }
                        else
                        {
                            return blockDown.GetComponent<Sticky>().PullBlock("right");
                        }
                    }
                }
            }
        }

        if (dir == "up" && blockUp != null)
        {
            if (blockUp.tag == "Wall")
            {
                return false;
            }
            else if (blockUp.tag == "Smooth")
            {
                return blockUp.GetComponent<Smooth>().PushBlock("up");
            }
            else if (blockUp.tag == "Sticky" && blockUp.GetComponent<Sticky>().isInMotion == false)
            {
                return blockUp.GetComponent<Sticky>().PushBlock("up");
            }
            else if (blockUp.tag == "Clingy")
            {
                return false;
            }
        }
        if (dir == "up")
        {
            if (isSticky == false)
            {
                if (blockLeft != null && blockLeft.tag == "Sticky")
                {
                    if (blockLeft.GetComponent<Sticky>().isInMotion)
                    {
                        return true;
                    }
                    else
                    {
                        return blockLeft.GetComponent<Sticky>().PullBlock("up");
                    }
                }
                if (blockRight != null && blockRight.tag == "Sticky")
                {
                    if (blockRight.GetComponent<Sticky>().isInMotion)
                    {
                        return true;
                    }
                    else
                    {
                        return blockRight.GetComponent<Sticky>().PullBlock("up");
                    }
                }
                if (blockDown != null && blockDown.tag == "Sticky")
                {
                    if (blockDown.GetComponent<Sticky>().isInMotion)
                    {
                        return true;
                    }
                    else
                    {
                        return blockDown.GetComponent<Sticky>().PullBlock("up");
                    }
                }
                else if (blockDown != null && blockDown.tag == "Clingy")
                {
                    if (blockDown.GetComponent<Clingy>().isInMotion)
                    {
                        return true;
                    }
                    else
                    {
                        return blockDown.GetComponent<Clingy>().PushBlock("up");
                    }
                }
            }
            else
            {
                if (blockLeft != null)
                {
                    if (blockLeft.tag == "Smooth")
                    {
                        blockLeft.GetComponent<Smooth>().PushBlock("up");
                    }
                    else if (blockLeft.tag == "Sticky")
                    {
                        if (blockLeft.GetComponent<Sticky>().isInMotion)
                        {
                            return true;
                        }
                        else
                        {
                            return blockLeft.GetComponent<Sticky>().PullBlock("up");
                        }
                    }
                }
                if (blockRight != null)
                {
                    if (blockRight.tag == "Smooth")
                    {
                        blockRight.GetComponent<Smooth>().PushBlock("up");
                    }
                    else if (blockRight.tag == "Sticky")
                    {
                        if (blockRight.GetComponent<Sticky>().isInMotion)
                        {
                            return true;
                        }
                        else
                        {
                            return blockRight.GetComponent<Sticky>().PullBlock("up");
                        }
                    }
                }
                if (blockDown != null)
                {
                    if (blockDown.tag == "Smooth")
                    {
                        blockDown.GetComponent<Smooth>().PushBlock("up");
                    }
                    else if (blockDown.tag == "Sticky")
                    {
                        if (blockDown.GetComponent<Sticky>().isInMotion)
                        {
                            return true;
                        }
                        else
                        {
                            return blockDown.GetComponent<Sticky>().PullBlock("up");
                        }
                    }
                    else if (blockDown.tag == "Clingy")
                    {
                        if (blockDown.GetComponent<Clingy>().isInMotion)
                        {
                            return true;
                        }
                        else
                        {
                            return blockDown.GetComponent<Clingy>().PushBlock("up");
                        }
                    }
                }
            }
        }

        if (dir == "down" && blockDown != null)
        {
            if (blockDown.tag == "Wall")
            {
                return false;
            }
            else if (blockDown.tag == "Smooth")
            {
                return blockDown.GetComponent<Smooth>().PushBlock("down");
            }
            else if (blockDown.tag == "Sticky" && blockDown.GetComponent<Sticky>().isInMotion == false)
            {
                return blockDown.GetComponent<Sticky>().PushBlock("down");
            }
            else if (blockDown.tag == "Clingy")
            {
                return false;
            }
        }
        if (dir == "down")
        {
            if (isSticky == false)
            {
                if (blockLeft != null && blockLeft.tag == "Sticky")
                {
                    if (blockLeft.GetComponent<Sticky>().isInMotion)
                    {
                        return true;
                    }
                    else
                    {
                        return blockLeft.GetComponent<Sticky>().PullBlock("down");
                    }
                }
                if (blockRight != null && blockRight.tag == "Sticky")
                {
                    if (blockRight.GetComponent<Sticky>().isInMotion)
                    {
                        return true;
                    }
                    else
                    {
                        return blockRight.GetComponent<Sticky>().PullBlock("down");
                    }
                }
                if (blockUp != null && blockUp.tag == "Sticky")
                {
                    if (blockUp.GetComponent<Sticky>().isInMotion)
                    {
                        return true;
                    }
                    else
                    {
                        return blockUp.GetComponent<Sticky>().PullBlock("down");
                    }
                }
                else if (blockUp != null && blockUp.tag == "Clingy")
                {
                    if (blockUp.GetComponent<Clingy>().isInMotion)
                    {
                        return true;
                    }
                    else
                    {
                        return blockUp.GetComponent<Clingy>().PushBlock("down");
                    }
                }
            }
            else
            {
                if (blockLeft != null)
                {
                    if (blockLeft.tag == "Smooth")
                    {
                        blockLeft.GetComponent<Smooth>().PushBlock("down");
                    }
                    else if (blockLeft.tag == "Sticky")
                    {
                        if (blockLeft.GetComponent<Sticky>().isInMotion)
                        {
                            return true;
                        }
                        else
                        {
                            return blockLeft.GetComponent<Sticky>().PullBlock("down");
                        }
                    }
                }
                if (blockRight != null)
                {
                    if (blockRight.tag == "Smooth")
                    {
                        blockRight.GetComponent<Smooth>().PushBlock("down");
                    }
                    else if (blockRight.tag == "Sticky")
                    {
                        if (blockRight.GetComponent<Sticky>().isInMotion)
                        {
                            return true;
                        }
                        else
                        {
                            return blockRight.GetComponent<Sticky>().PullBlock("down");
                        }
                    }
                }
                if (blockUp != null)
                {
                    if (blockUp.tag == "Smooth")
                    {
                        blockUp.GetComponent<Smooth>().PushBlock("down");
                    }
                    else if (blockUp.tag == "Sticky")
                    {
                        if (blockUp.GetComponent<Sticky>().isInMotion)
                        {
                            return true;
                        }
                        else
                        {
                            return blockUp.GetComponent<Sticky>().PullBlock("down");
                        }
                    }
                    else if (blockUp.tag == "Clingy")
                    {
                        if (blockUp.GetComponent<Clingy>().isInMotion)
                        {
                            return true;
                        }
                        else
                        {
                            return blockUp.GetComponent<Clingy>().PushBlock("down");
                        }
                    }
                }
            }
        }

        //if the space is empty, all clear to move
        return true;
    }
}