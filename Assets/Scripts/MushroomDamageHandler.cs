using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MushroomDamageHandler : MonoBehaviour
{
    public Sprite[] Sprites = new Sprite[3]; //to remove extra value from editor size of array is actually health
    GameObject GameController;
    int health;

    private void Start()
    {
        GameController = GameObject.FindGameObjectWithTag("Controller");
        health = Sprites.Length;
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.name == "Bullet(Clone)")
        {
            health--;
        }
    }

    private void ChangeSprite()
    {
        GetComponent<SpriteRenderer>().sprite = Sprites[health - 1];
    }

    void Update()
    {
        if (health <= 0)
        {
            Die();
        }
        else
        {
            ChangeSprite();
        }
    }

    void Die()
    {
        Destroy(gameObject);
        Vector3 pos = transform.position;
        int x = (int)pos.x;
        int y = (int)pos.y;
        GameController.GetComponent<MushroomController>().RemoveMushroom(x, y);
    }
}
