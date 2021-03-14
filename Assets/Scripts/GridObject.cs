using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public struct Point
{
    public int x;
    public int y;

    public Point(int x, int y)
    {
        this.x = x;
        this.y = y;
    }

    public Point(Point p)
    {
        this.x = p.x;
        this.y = p.y;
    }

    public override string ToString()
    {
        return "(" + this.x + ","  + this.y + ")";
    }

}

public class GridObject : MonoBehaviour
{

    // All GridObjects have a Position on the grid
    // To get its world positon use the GameGrid.posToWorldLocation 
    public Point gridPositon;
    
    public void SetPosition(Point pos)
    {
        this.transform.position = GameGrid.Instance.PosToWorldLocation(pos);
        this.gridPositon = pos;
    }


    /* For testing */
    private void OnDrawGizmos()
    {
        Handles.Label(transform.position, "Pos: (" + gridPositon.x + ", " + gridPositon.y + ")");
    }

    public void UpdatePosition()
    {
        gridPositon = GameGrid.Instance.WorldLocationToPos(transform.position);
        // Debug.Log(transform.position + " => " + gridPositon);
    }



}
