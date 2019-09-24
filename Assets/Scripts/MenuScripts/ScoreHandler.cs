using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreHandler : MonoBehaviour
{
    public Text ScoreText;
    public Text ResultText;
    void Start()
    {
        int score = PlayerPrefs.GetInt("Score");
        ScoreText.text = score.ToString("D6");
    }
    void Update()
    {

    }
}
