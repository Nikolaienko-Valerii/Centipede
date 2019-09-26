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

    public int PlayerZoneHeight = 4;

    public Canvas canvas;
    public Image HealthImage;

    public Text ScoreText;

    public int Lifes = 3;
    public int MaxLifes = 5;

    public int Score = 0;
    public int MushroomScore = 3;
    public int CentipedeScore = 10;

    public int InitialCentipedeLength = 12;
    int levelNumber = 1;

    private CentipedeSpawner centipedeSpawner;
    private MushroomSpawner mushroomSpawner;
    private LevelsController levelsController;
    private PointsHandler pointsController;

    private GameObject[] centipedes; //do I really need this?

    private int AliveParts;
    private GameObject player;

    private bool levelLost = false;

    private Image[] healthBar;


    void Start()
    {
        CameraPositioning();
        InitializeControllers();
        InitializeGame();
        DisplayHealth();
        MushroomsGrid = mushroomSpawner.GenerateMushroomField(gridWidth, gridHeight, PlayerZoneHeight); 
        StartGame();
    }

    void CameraPositioning()
    {
        //grid cells are 1x1 size, top line is left for scores and lifes => top-left cell must be (0,1) => camera's top left angle (-0.5, 1.5);

        //if cam top left was in (0;0)
        float yPos = -Camera.main.orthographicSize;

        float ratio = (float)Screen.width / Screen.height; //to calculate cam halfwidth 
        float xPos = Camera.main.orthographicSize * ratio;

        
        Vector3 cameraPosition = new Vector3(xPos, yPos, -10); //-10 is default camera Z position
        cameraPosition += new Vector3(-0.5f, 1.5f, 0);
        Camera.main.transform.position = cameraPosition;
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
        print(x + " " + y);
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

    public bool IsOnField(int x, int y)
    {
        if (x < 0 || x > gridWidth - 1)
        {
            return false;
        }
        return true;
    }

    public void CentipedePartDestroyed()
    {
        AliveParts--;
        AddPoints(CentipedeScore);
        if (AliveParts <= 0)
        {
            NewLevel();
        }
    }

    void AddPoints(int value)
    {
        Score = pointsController.AddPoints(value, levelNumber);
        ScoreText.text = Score.ToString("D6");
    }

    public void AddLife()
    {
        Lifes = Mathf.Min(Lifes + 1, MaxLifes);
        healthBar[Lifes - 2].enabled = true; //-1 because displaying extra lifes, not all lifes and -1 more because of array starting from 0
    }

    #region Spawning Centipede

    void SpawnCentipedes()  
    {
        gameObject.GetComponent<AudioSource>().Play();
        AliveParts = InitialCentipedeLength;
        SpawnMainCentipede();
        if (levelNumber > 1)
        {
            SpawnHeads();
        }
    }

    //creating "big" centipede
    void SpawnMainCentipede()
    {
        int centipedeLength = Mathf.Max(InitialCentipedeLength - levelNumber + 1, 0);
        bool goingRight = ChooseDirection();

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

        for (int i = 0; i < centipedeLength; i++)
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
    }

    //creating "heads"
    void SpawnHeads()
    {
        int headsCount = Mathf.Min(levelNumber - 1, InitialCentipedeLength);
        int pointer = Mathf.Max(InitialCentipedeLength - levelNumber + 1, 0);

        for (int j = 0; j < headsCount; j++)
        {
            bool goingRight = ChooseDirection();

            int baseX;
            int baseY = -Random.Range(0, 4);  // this will randomize "heads" spawning more
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

            centipedes[pointer + j] = Instantiate(CentipedePrefab, new Vector3(baseX + j * step, baseY, 0), rotation);
            centipedes[pointer + j].GetComponent<SectionController>().goingRight = goingRight;
            centipedes[pointer + j].GetComponent<SectionController>().isHead = true;
            centipedes[pointer + j].GetComponent<SectionController>().delay /= 2;
        }
    }

    bool ChooseDirection()
    {
        if (Random.Range(0, 2) == 0)
        {
            return false;
        }
        return true;
    }

    #endregion

    #region GameControls

    void StartGame()
    {
        SpawnPlayer();
        SpawnCentipedes();
    }

    void NewLevel()
    {
        levelNumber++;
        SpawnCentipedes();
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
        DestroyBonuses();
        SpawnPlayer();
        SpawnCentipedes();
    }

    void GameOver()
    {
        PlayerPrefs.SetInt("Score", Score);
        SceneManager.LoadScene(2, LoadSceneMode.Single);
    }

    #endregion

    #region Destroying Objects and Spawning Player
    void DestroyAllCentipedes()
    {
        gameObject.GetComponent<AudioSource>().Stop();
        foreach (var part in centipedes)
        {
            Destroy(part);
        }
    }

    void DestroyPlayer()
    {
        Destroy(player);
    }

    void DestroyBonuses()
    {
        var bonuses = GameObject.FindGameObjectsWithTag("Bonus");
        foreach (var bonus in bonuses)
        {
            Destroy(bonus);
        }
    }

    void SpawnPlayer()
    {
        //Placing player on the bottom midle => yPos = camera.y - camera.orthographicSize + half player width (is 0.5 because everything is 1x1)
        float xPos = Camera.main.transform.position.x;
        float yPos = Camera.main.transform.position.y - Camera.main.orthographicSize + 0.5f; 

        player = Instantiate(PlayerPrefab, new Vector3(xPos, yPos, 0), new Quaternion());
    }
    #endregion
}
