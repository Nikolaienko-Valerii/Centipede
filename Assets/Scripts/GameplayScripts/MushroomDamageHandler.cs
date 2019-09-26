using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MushroomDamageHandler : MonoBehaviour
{
    public Sprite[] Sprites = new Sprite[3]; //to remove extra value from editor size of array is actually health
    public GameObject BonusPrefab;
    public int bonusChance = 100;

    GameObject gameController;
    
    int health;
    

    private void Start()
    {
        gameController = GameObject.FindGameObjectWithTag("Controller");
        health = Sprites.Length;
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Bullet" && !collision.gameObject.GetComponent<BulletDamageHandler>().isColliding)
        {
            collision.gameObject.GetComponent<BulletDamageHandler>().isColliding = true;
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
        if (IsBonusDropped())
        {
            Instantiate(BonusPrefab, transform.position, new Quaternion());
        }
        Destroy(gameObject);
        Vector3 pos = transform.position;
        int x = (int)pos.x;
        int y = (int)pos.y;
        gameController.GetComponentInParent<GameControllerScript>().RemoveMushroom(x, y);
    }

    bool IsBonusDropped()
    {
        int value = Random.Range(0, 100);
        if (value<bonusChance)
        {
            return true;
        }
        return false;
    }
}
