using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameControllerScript : MonoBehaviour
{
    bool[,] MushroomsGrid;
    int gridHeight, gridWidth;
    public GameObject MushroomPrefab;
    public GameObject PlayerPrefab;
    public GameObject CentipedePrefab;
    public GameObject HealthImagePrefab;

    public int Lifes = 3;
    public int Score = 0;
    public int MushroomScore = 3;
    public int CentipedeScore = 10;

    public int InitialCentipedeLength = 12;
    int currentCentipedeLength;
    int headsCount;



    void Start()
    {
        gridHeight = (int)(2 * Camera.main.orthographicSize);
        gridWidth = gridHeight * 4 / 3; //pretty hardcoded but our ratio is always 4:3? so who cares :)
        gridHeight--; //because first line is for scores etc.
        currentCentipedeLength = InitialCentipedeLength;
        headsCount = InitialCentipedeLength - currentCentipedeLength;
        MushroomsGrid = new bool[gridWidth, gridHeight];
        GenerateMushrooms();
        StartGame();
    }

    void GenerateMushrooms()
    {
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
        if (y <= -gridHeight)
        {
            LostLife();
        }
        //print(x +" "+ y + " " + -gridHeight);
        return !MushroomsGrid[x, -y];
    }

    void Update()
    {
        
    }

    void AddPoints()
    {
        //this must handle how much points to add and add them
    }

    void SpawnCentipede(bool goingRight)  //TODO spawn "heads" correctly, also randomize spawn positions/times
    {
        int baseX;
        GameObject[] centipedeParts = new GameObject[InitialCentipedeLength];
        Quaternion rotation;
        int step;
        if (goingRight)
        {
            rotation = new Quaternion();
            baseX = -1;
            step = -1;
        }
        else
        {
            rotation = new Quaternion(0, 0, -1, 0);
            baseX = gridWidth;
            step = 1;
        }

        //creating "big" centipede
        for (int i = 0; i < currentCentipedeLength; i++)
        {
            centipedeParts[i] = Instantiate(CentipedePrefab, new Vector3(baseX + i * step, 0, 0), rotation);
            centipedeParts[i].GetComponent<GridMovement>().goingRight = goingRight;
            if (i != 0)
            {
                centipedeParts[i].GetComponent<GridMovement>().previousSegment = centipedeParts[i - 1];
                centipedeParts[i - 1].GetComponent<GridMovement>().nextSegment = centipedeParts[i];
            }
            else
                centipedeParts[i].GetComponent<GridMovement>().isHead = true;
        }
        //creating "heads"
        int pointer = currentCentipedeLength;
        for (int j = 0; j < headsCount; j++)
        {
            centipedeParts[pointer + j] = Instantiate(CentipedePrefab, new Vector3(baseX + (pointer + j) * step, 0, 0), rotation);
            centipedeParts[pointer + j].GetComponent<GridMovement>().isHead = true;
        }
    }

    void StartGame()
    {
        SpawnPlayer();
        SpawnCentipede(false);
    }

    void NewLevel()
    {
        if (currentCentipedeLength > 0)
        {
            currentCentipedeLength--;
            headsCount++;
        }
        SpawnCentipede(true);
    }

    void LostLife()
    {
        Lifes--;
        print(Lifes);
        if (Lifes == 0)
        {
            GameOver();
        }
        else
        {
            RestartLevel();
        }
    }

    void RestartLevel()
    {
        DestroyAllCentipedes();
        SpawnPlayer();
        SpawnCentipede(true);
    }

    void DestroyAllCentipedes()
    {
        //Destroy centipedes here without adding points and spawning mushrooms
    }

    void SpawnPlayer()
    {
        Instantiate(PlayerPrefab, new Vector3(11.5f, -17, 0), new Quaternion()); //TODO calculate position
    }

    void GameOver()
    {
        PlayerPrefs.SetInt("Score", Score);
        SceneManager.LoadScene(2, LoadSceneMode.Single);
    }
}
