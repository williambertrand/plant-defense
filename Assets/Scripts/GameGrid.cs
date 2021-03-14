using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameGrid : MonoBehaviour
{

    public static GameGrid Instance;
    public GameObject gridCell;

    public int width;
    public int height;


    // Full "Boad" sizes, includes space around the area where
    // a player can place plants
    public int boardWidth;
    public int boardHeight;

    public Vector3 origin;


    // Size of a single cell
    private int dx = 1;
    private int dy = 1;


    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        origin = new Vector3(0, 0, 0);

        DrawGrid();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public Vector3 PosToWorldLocation(Point p)
    {
        return new Vector3(p.x * dx + 0.5f, p.y * dy + 0.5f);
    }

    public Point WorldLocationToPos(Vector3 loc)
    {
        return new Point (
            (int) Mathf.Floor(loc.x / dx),
            (int) Mathf.Floor(loc.y / dy)
        );
    }

    private void DrawGrid()
    {
        for(int i = -width / 2; i < width / 2; i++)
        {
            for(int j = -height / 2; j < height / 2; j++)
            {
                Point p = new Point(i, j);
                Instantiate(gridCell, PosToWorldLocation(p), Quaternion.identity, transform);
            }
        }
    }

    public bool CheckBounds(Point p)
    {
        return (
            p.x < width / 2 && p.x  >= -width / 2 &&
            p.y < height / 2 && p.y >= -height / 2
        );
    }
}
