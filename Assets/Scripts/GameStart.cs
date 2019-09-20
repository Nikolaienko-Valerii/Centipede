using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStart : MonoBehaviour
{
    void Start()
    {
        gameObject.GetComponent<CentipedeSpawner>().Spawn(1, new Vector3(-8, -0.5f, 0), new Quaternion(0, 0, 0, 0));
    }

    void Update()
    {
        
    }
}
