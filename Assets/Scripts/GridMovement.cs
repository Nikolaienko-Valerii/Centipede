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
    Animator animator;
    void Start()
    {
        gameController = GameObject.FindGameObjectWithTag("Controller");
        animator = gameObject.GetComponent<Animator>();
        ApplyAnimations();
    }
    void Update()
    {
        if (health == 0) //probably check after making a step
        {
            Die();
            int x = (int)transform.position.x;
            int y = (int)transform.position.y;
            gameController.GetComponent<MushroomController>().AddMushroom(x,y);
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            MakeStep();
        }
    }

    void ApplyAnimations()
    {
        if (!isHead)
        {
            animator.runtimeAnimatorController = Resources.Load("Tale01") as RuntimeAnimatorController;
        }
        else
        {
            animator.runtimeAnimatorController = Resources.Load("Head01") as RuntimeAnimatorController;
        }
    }

    void Die()
    {
        if (nextSegment != null)
        {
            nextSegment.GetComponent<GridMovement>().BecomeHead();
        }
        Destroy(gameObject);
    }

    //this will determine where to go next and start coroutine
    void MakeStep()
    {
        if (!isHead)
        {
            MakeStepAndTellNext(goingForward);
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
            MakeStepAndTellNext(isForward);
        }
    }

    void MakeStepAndTellNext(bool isForward)
    {
        if (isForward)
        {
            StartCoroutine(GoForward());
        }
        else
        {
            StartCoroutine(GoDown());
        }
        if (nextSegment != null)
        {
            nextSegment.GetComponent<GridMovement>().goingForward = isForward;
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
        float rotation = 0.5f;
        if (!goingRight)
        {
            rotation = -rotation;
        }
        float currentZRotation = transform.rotation.z;
        transform.rotation = new Quaternion(0, 0, currentZRotation + rotation, 0);
        Vector3 currentPosition = transform.position;
        int x = (int)currentPosition.x;
        int y = (int)currentPosition.y - 1;
        transform.position = new Vector3(x, y, 0);
        transform.rotation = new Quaternion(0, 0, currentZRotation + 2 * rotation, 0);
        goingRight = !goingRight;
        yield return null;
    }

    void BecomeHead()
    {
        isHead = true;
        goingForward = false; //TODO head ignores this
        animator.runtimeAnimatorController = Resources.Load("Head01") as RuntimeAnimatorController;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.name == "Bullet(Clone)") //use tag instead
        {
            health--;
        }
    }
}
