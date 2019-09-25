using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointsHandler : MonoBehaviour
{
    private int currentPoints = 0;

    public int GetPoints()
    {
        return currentPoints;
    }

    public int AddPoints(int value, int level)
    {
        currentPoints += value * level;
        return currentPoints;
    }

    public void SavePoints()
    {
        PlayerPrefs.SetInt("Score", currentPoints);
        if (currentPoints > GetHighScore())
        {
            SaveHighScore();
        }
    }

    public int GetHighScore()
    {
        return PlayerPrefs.GetInt("HighScore", 0);
    }

    private void SaveHighScore()
    {
        PlayerPrefs.SetInt("HighScore", currentPoints);
    }
}
