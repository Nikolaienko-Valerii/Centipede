using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridMovement : MonoBehaviour
{
    public bool isHead = false;
    public bool goingRight = true;
    public GameObject previousSegment;
    public GameObject nextSegment;
    public bool goingForward = true;
    public int health = 1;
    GameObject gameController;
    void Start()
    {
        gameController = GameObject.FindGameObjectWithTag("Controller");
    }
    void Update()
    {
        if (health == 0) //probably check after making a step
        {
            Die();
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            MakeStep();
        }
    }

    void Die()
    {
        nextSegment.GetComponent<GridMovement>().BecomeHead();
        Destroy(gameObject);
    }

    //this will determine where to go next and start coroutine
    void MakeStep()
    {
        if (!isHead)
        {
            if (goingForward)
            {
                StartCoroutine(GoForward());
                nextSegment.GetComponent<GridMovement>().goingForward = true;
            }
            else
            {
                StartCoroutine(GoDown());
                nextSegment.GetComponent<GridMovement>().goingForward = false;
            }
        }
        else
        {
            Vector3 currentPosition = transform.position;
            int step;
            if (goingRight)
                step = 1;
            else
                step = -1;
            int x = (int)currentPosition.x + step;
            int y = (int)currentPosition.y;
            bool isForward = gameController.GetComponent<MushroomController>().IsStepAvailable(x, y);
            if (isForward)
            {
                StartCoroutine(GoForward());
                nextSegment.GetComponent<GridMovement>().goingForward = true;
            }
            else
            {
                StartCoroutine(GoDown());
                nextSegment.GetComponent<GridMovement>().goingForward = false;
            }
        }
    }

    //use this if nothing blocking way
    IEnumerator GoForward()
    {
        Vector3 currentPosition = transform.position;
        int step;
        if (goingRight)
            step = 1;
        else
            step = -1;
        int x = (int)currentPosition.x + step;
        int y = (int)currentPosition.y;
        transform.position = new Vector3(x, y, 0);
        yield return null;
    }

    //use this if something in the way or centipede splitted
    IEnumerator GoDown()
    {
        Vector3 currentPosition = transform.position;
        int x = (int)currentPosition.x;
        int y = (int)currentPosition.y - 1;
        transform.position = new Vector3(x, y, 0);
        goingRight = !goingRight;
        yield return null;
    }

    void BecomeHead()
    {
        isHead = true;
        //after finishing step? go down
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.name == "Bullet(Clone)") //use tag instead
        {
            health--;
        }
    }
}
