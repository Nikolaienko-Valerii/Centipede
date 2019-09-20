using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CentipedeMovement : MonoBehaviour
{
    public float Speed = 1.5f;
    public int Length = 5;
    public int Direction = 1; //1 = right, -1 = left, 0 = down
    void Start()
    {
        
    }

    void Update()
    {
        //Vector3 position = transform.position;
        //position.x += Direction * Speed;
        //transform.position = position;
    }
}
