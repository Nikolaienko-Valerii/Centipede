using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MushroomController : MonoBehaviour
{
    bool[,] MushroomsGrid;
    int gridHeight, gridWidth;
    public GameObject MushroomPrefab; 
    void Start()
    {
        gridHeight = (int)(2 * Camera.main.orthographicSize);
        gridWidth = gridHeight * 4 / 3; //pretty hardcoded but our ratio is always 4:3? so who cares :)
        MushroomsGrid = new bool[gridHeight, gridWidth];
        GenerateMushrooms();
    }

    void GenerateMushrooms()
    {
        //for (int i = 0; i < gridHeight; i++)
        //{
        //    MushroomsGrid[i, i] = true;
        //    Instantiate(MushroomPrefab, new Vector3(i, -i, 0), new Quaternion());
        //}
        AddMushroom(5,-7);
        AddMushroom(5,-8);
        AddMushroom(4,-8);
        AddMushroom(0,-8);
        AddMushroom(3,-9);
    }

    public void RemoveMushroom(int x, int y)
    {
        MushroomsGrid[x, -y] = false;
        print("Dead mushroom " + x + " " + -y);
    }

    public void AddMushroom(int x, int y)
    {
        MushroomsGrid[x, -y] = true;
        Instantiate(MushroomPrefab, new Vector3(x, y, 0), new Quaternion());
    }

    public bool IsStepAvailable(int x, int y)
    {
        if (x < 0 || x > gridWidth - 1)
        {
            return false;
        }
        return !MushroomsGrid[x, -y];
    }

    void Update()
    {
        
    }
}
