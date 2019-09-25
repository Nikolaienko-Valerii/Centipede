using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MushroomSpawner : MonoBehaviour
{
    int MushroomProbability = 6;
    public GameObject MushroomPrefab;

    public bool[,] GenerateMushroomField(int width, int height, int playerZoneHeight)
    {
        bool[,] field = new bool[width, height];
        for (int w = 0; w < width; w++)
        {
            for (int h = 0; h < height - playerZoneHeight; h++)
            {
                field[w, h] = hasMushroom(MushroomProbability);
                if (field[w, h])
                {
                    Instantiate(MushroomPrefab, new Vector3(w, -h), new Quaternion());
                }
            }
        }
        return field;
    }

    public bool hasMushroom(int percentChance)
    {
        int value = Random.Range(0, 100);
        if (value < percentChance)
        {
            return true;
        }
        return false;
    }
}
