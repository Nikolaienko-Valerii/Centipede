using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletDamageHandler : MonoBehaviour
{
    public bool isColliding = false;

    //private void OnCollisionEnter2D(Collision2D collision)
    //{
    //    isColliding = true;
    //}
    void Update()
    {
        if (isColliding)
        {
            Destroy(gameObject);
        }
    }
}
