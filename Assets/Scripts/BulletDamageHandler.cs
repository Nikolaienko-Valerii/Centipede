using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletDamageHandler : MonoBehaviour
{
    int health = 1;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        health--;
    }
    void Update()
    {
        if (health <= 0)
        {
            Destroy(gameObject);
        }
    }
}
