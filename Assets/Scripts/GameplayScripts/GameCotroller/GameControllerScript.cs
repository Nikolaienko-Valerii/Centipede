using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameControllerScript : MonoBehaviour
{
    bool[,] MushroomsGrid;
    int gridHeight, gridWidth;
    public GameObject MushroomPrefab;
    public GameObject PlayerPrefab;
    public GameObject CentipedePrefab;

    public Canvas canvas;
    public Image HealthImage;

    public Text ScoreText;

    public int Lifes = 3;
    public int MaxLifes = 5;

    public int Score = 0;
    public int MushroomScore = 3;
    public int CentipedeScore = 10;

    public int InitialCentipedeLength = 12;
    int currentCentipedeLength;
    int headsCount;

    private CentipedeSpawner centipedeSpawner;
    private MushroomSpawner mushroomSpawner;
    private LevelsController levelsController;
    private PointsHandler pointsController;

    private GameObject[] centipedes;
    private GameObject player;

    private bool levelLost = false;

    private Image[] healthBar;


    void Start()
    {
        InitializeControllers();
        InitializeGame();
        DisplayHealth();
        MushroomsGrid = mushroomSpawner.GenerateMushroomField(gridWidth, gridHeight);
        StartGame();
    }

    void InitializeControllers()
    {
        centipedeSpawner = GetComponent<CentipedeSpawner>();
        mushroomSpawner = GetComponent<MushroomSpawner>();
        levelsController = GetComponent<LevelsController>();
        pointsController = GetComponent<PointsHandler>();
    }

    void InitializeGame()
    {
        gridHeight = (int)(2 * Camera.main.orthographicSize);
        gridWidth = gridHeight * 4 / 3; //pretty hardcoded but our ratio is always 4:3? so who cares :)
        gridHeight--; //because first line is for scores etc.

        currentCentipedeLength = InitialCentipedeLength;
        headsCount = InitialCentipedeLength - currentCentipedeLength;

        centipedes = new GameObject[InitialCentipedeLength];
    }

    void DisplayHealth()
    {
        healthBar = new Image[MaxLifes];
        Vector3 imageStep = new Vector3(HealthImage.rectTransform.rect.width + 1, 0);
        Vector3 basePosition = ScoreText.rectTransform.position + new Vector3(ScoreText.rectTransform.rect.width, 0);
        Vector3 ImageCenter = new Vector3(HealthImage.rectTransform.rect.width / 2, -HealthImage.rectTransform.rect.width / 2);
        basePosition += ImageCenter;
        for (int i = 0; i < MaxLifes; i++)
        {
            healthBar[i] = Instantiate(HealthImage, basePosition + i * imageStep, new Quaternion(), canvas.transform);
            healthBar[i].enabled = false;
        }
        for (int j = 0; j < Lifes-1; j++)
        {
            healthBar[j].enabled = true;
        }
    }

    public void RemoveMushroom(int x, int y)
    {
        MushroomsGrid[x, -y] = false;
        AddPoints(MushroomScore);
    }

    public void AddMushroom(int x, int y)
    {
        MushroomsGrid[x, -y] = true;
        Instantiate(MushroomPrefab, new Vector3(x, y, 0), new Quaternion());
    }

    public bool IsStepAvailable(int x, int y)
    {
        if (y <= -gridHeight)
        {
            LostLife();
            return false;
        }
        if (x < 0 || x > gridWidth - 1)
        {
            return false;
        }
        return !MushroomsGrid[x, -y];
    }

    public void CentipedePartDestroyed()
    {
        AddPoints(CentipedeScore);
    }

    void AddPoints(int value)
    {
        Score = pointsController.AddPoints(value);
        ScoreText.text = Score.ToString("D6");
    }

    void SpawnCentipede(bool goingRight)  //TODO spawn "heads" correctly, also randomize spawn positions/times
    {
        int baseX;
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
            centipedes[i] = Instantiate(CentipedePrefab, new Vector3(baseX + i * step, 0, 0), rotation);
            centipedes[i].GetComponent<SectionController>().goingRight = goingRight;
            if (i != 0)
            {
                centipedes[i].GetComponent<SectionController>().previousSegment = centipedes[i - 1];
                centipedes[i - 1].GetComponent<SectionController>().nextSegment = centipedes[i];
            }
            else
                centipedes[i].GetComponent<SectionController>().isHead = true;
        }
        //creating "heads"
        int pointer = currentCentipedeLength;
        for (int j = 0; j < headsCount; j++)
        {
            centipedes[pointer + j] = Instantiate(CentipedePrefab, new Vector3(baseX + (pointer + j) * step, 0, 0), rotation);
            centipedes[pointer + j].GetComponent<SectionController>().isHead = true;
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

    public void LostLife()
    {
        if (!levelLost)
        {
            levelLost = true;
            Lifes--;
            if (Lifes == 0)
            {
                GameOver();
            }
            else
            {

                healthBar[Lifes-1].enabled = false;
                RestartLevel();
            }
        }
    }

    void RestartLevel()
    {
        levelLost = false;
        DestroyAllCentipedes();
        DestroyPlayer();
        SpawnPlayer();
        SpawnCentipede(true);
    }

    void DestroyAllCentipedes()
    {
        foreach (var part in centipedes)
        {
            Destroy(part);
        }
    }

    void DestroyPlayer()
    {
        Destroy(player);
    }

    void SpawnPlayer()
    {
        player = Instantiate(PlayerPrefab, new Vector3(11.5f, -17, 0), new Quaternion()); //TODO calculate position
        player.tag = "Player";
    }

    void GameOver()
    {
        PlayerPrefs.SetInt("Score", Score);
        SceneManager.LoadScene(2, LoadSceneMode.Single);
    }
}
