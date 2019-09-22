using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletMovement : MonoBehaviour
{
    public float Speed = 5f;
    void Update()
    {
        Vector3 position = transform.position;
        position.y += Speed;
        transform.position = position;
    }
}
