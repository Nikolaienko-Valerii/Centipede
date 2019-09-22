using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreHandler : MonoBehaviour
{
    public Text ScoreText;
    public Text ResultText;
    string scoreTextBase = "your score: ";
    void Start()
    {
        int score = PlayerPrefs.GetInt("Score");
        ScoreText.text = scoreTextBase + score.ToString("D6");
    }
    void Update()
    {

    }
}
