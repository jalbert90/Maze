using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Maze : MonoBehaviour
{
    [System.Serializable]
    public class Cell
    {
        public bool visited;
        public GameObject north;
        public GameObject south;
        public GameObject east;
        public GameObject west;
    }

    public GameObject wall;
    public float wallLength = 1.0f;
    public int xGrids = 5;
    public int yGrids = 5;
    private Vector3 initialPosition;
    private GameObject wallHolder;
    private Cell[] cells;
    public int currentCell = 0;
    public int totalCells;

    // Start is called before the first frame update
    void Start()
    {
        CreateWalls();
    }

    void CreateWalls()
    {
        wallHolder = new GameObject();
        wallHolder.name = "Maze";

        initialPosition = new Vector3((-xGrids / 2) + wallLength / 2, 0, (-yGrids / 2) + wallLength / 2);
        Vector3 myPosition = initialPosition;
        GameObject tempWall;

        // For x-axis
        for (int i=0; i<yGrids; i++)
        {
            for (int j=0; j <= xGrids; j++)
            {
                myPosition = new Vector3(initialPosition.x + (wallLength * j) - wallLength / 2, 0, initialPosition.z + (wallLength * i) - wallLength / 2);
                tempWall = Instantiate(wall, myPosition, Quaternion.Euler(0,0,0)) as GameObject;
                tempWall.transform.parent = wallHolder.transform;
            }
        }

        // For y-axis
        for (int i = 0; i <= yGrids; i++)
        {
            for (int j = 0; j < xGrids; j++)
            {
                myPosition = new Vector3(initialPosition.x + (wallLength * j), 0, initialPosition.z + (wallLength * i) - wallLength);
                tempWall = Instantiate(wall, myPosition, Quaternion.Euler(0, 90, 0)) as GameObject;
                tempWall.transform.parent = wallHolder.transform;
            }
        }

        CreateCells();
    }

    void CreateCells()
    {
        totalCells = xGrids * yGrids;
        int children = wallHolder.transform.childCount;
        GameObject[] allWalls = new GameObject[children];
        cells = new Cell[totalCells];
        int zParalellWallNumber = 0;
        int xParalellWallCount = 0;
        int termCount = 0;
        int xParalellWallNumberStart = (xGrids + 1) * yGrids;

        // Gets all the children
        for (int i = 0; i < children; i++)
        {
            allWalls[i] = wallHolder.transform.GetChild(i).gameObject;
        }

        // Assigns walls to the cells
        for (int cellnumber = 0; cellnumber < cells.Length; cellnumber++)
        {
            cells[cellnumber] = new Cell();
            if (termCount == xGrids)
            {
                zParalellWallNumber++;
                termCount = 0;
            }
            cells[cellnumber].west = allWalls[zParalellWallNumber];
            cells[cellnumber].south = allWalls[xParalellWallNumberStart + xParalellWallCount];
            cells[cellnumber].north = allWalls[xParalellWallNumberStart + xParalellWallCount + xGrids];
            xParalellWallCount++;
            zParalellWallNumber++;
            termCount++;
            cells[cellnumber].east = allWalls[zParalellWallNumber];
        }

        CreateMaze();
    }

    void CreateMaze()
    {
        GiveMeNeighbour();
    }

    void GiveMeNeighbour()
    {
        int[] neighbours = new int[4];
        int neighbourLength = 0;
        int check = 0;
        check = (currentCell + 1) / xGrids;
        check -= 1;
        check *= xGrids;
        check += xGrids;

        // Check east
        if ((currentCell + 1) != check)
        {
            if (!cells[currentCell + 1].visited)
            {
                neighbours[neighbourLength] = currentCell + 1;
                neighbourLength++;
            }
        }

        // Check west
        if (currentCell != check)
        {
            if (!cells[currentCell + 1].visited)
            {
                neighbours[neighbourLength] = currentCell - 1;
                neighbourLength++;
            }
        }

        // Check north
        if ((currentCell + xGrids) < totalCells)
        {
            if (!cells[currentCell + xGrids].visited)
            {
                neighbours[neighbourLength] = currentCell + xGrids;
                neighbourLength++;
            }
        }

        // Check south
        if ((currentCell - xGrids) >= 0)
        {
            if (!cells[currentCell - xGrids].visited)
            {
                neighbours[neighbourLength] = currentCell - xGrids;
                neighbourLength++;
            }
        }

        for (int i = 0; i < neighbourLength; i++)
            Debug.Log(neighbours[i]);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
