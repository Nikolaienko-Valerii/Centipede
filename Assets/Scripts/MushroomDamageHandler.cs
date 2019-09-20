using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MushroomDamageHandler : MonoBehaviour
{
    public Sprite[] Sprites = new Sprite[3]; //to remove extra value from editor size of array is actually health
    int health;

    private void Start()
    {
        health = Sprites.Length;
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        health--;
    }

    private void ChangeSprite()
    {
        GetComponent<SpriteRenderer>().sprite = Sprites[health - 1];
    }

    void Update()
    {
        if (health <= 0)
        {
            Destroy(gameObject);
        }
        else
        {
            ChangeSprite();
        }
    }
}
