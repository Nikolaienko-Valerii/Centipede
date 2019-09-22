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
    public float delay = 0.01f;
    void Start()
    {
        gameController = GameObject.FindGameObjectWithTag("Controller");
        animator = gameObject.GetComponent<Animator>();
        ApplyAnimations();
        StartCoroutine(MakeStep());
    }
    void Update()
    {
        if (health == 0) //probably check after making a step
        {
            Die();
            int x = (int)transform.position.x;
            int y = (int)transform.position.y;
            gameController.GetComponent<GameControllerScript>().AddMushroom(x,y);
        }
        //if (Input.GetKeyDown(KeyCode.Space))
        //{
        //    StartCoroutine(MakeStep());
        //}
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
    IEnumerator MakeStep()
    {
        if (isHead)
        {

            while (true)
            {
                //if (isHead)
                //{
                //    yield return StartCoroutine(MakeStepAndTellNext(goingForward));
                //}
                //else
                //{
                    Vector3 currentPosition = transform.position;
                    int step;
                    if (goingRight)
                        step = 1;
                    else
                        step = -1;
                    int x = (int)currentPosition.x + step;
                    int y = (int)currentPosition.y;
                    bool isForward = gameController.GetComponent<GameControllerScript>().IsStepAvailable(x, y);
                    goingForward = isForward;
                    yield return StartCoroutine(MakeStepAndTellNext(isForward));
                //}
            }
        }
    }

    IEnumerator MakeStepAndTellNext(bool isForward)
    {
        if (nextSegment != null)
        {
            StartCoroutine(nextSegment.GetComponent<GridMovement>().MakeStepAndTellNext(nextSegment.GetComponent<GridMovement>().goingForward));
        }
        if (isForward)
        {
            yield return StartCoroutine(GoForward());
        }
        else
        {
            yield return StartCoroutine(GoDown());
        }
    }

    //use this if nothing blocking way
    IEnumerator GoForward()
    {
        //Vector3 currentPosition = transform.position;
        //int step;
        //if (goingRight)
        //    step = 1;
        //else
        //    step = -1;
        //int x = (int)currentPosition.x + step;
        //int y = (int)currentPosition.y;
        //transform.position = new Vector3(x, y, 0);
        //yield return null;
        Vector3 curPos = transform.position;
        double step = 0.1;
        if (!goingRight)
            step = -step;
        for (int i = 0; i < 10; i++)
        {
            double x = curPos.x + (i + 1) * step;
            float y = curPos.y;
            transform.position = new Vector3((float)x, y, 0);
            yield return null;
        }
        if (!isHead)
        {
            goingForward = previousSegment.GetComponent<GridMovement>().goingForward;
        }
    }

    //use this if something in the way or centipede splitted
    IEnumerator GoDown()
    {
        //float rotation = 0.5f;
        //if (!goingRight)
        //{
        //    rotation = -rotation;
        //}
        //float currentZRotation = transform.rotation.z;
        //transform.rotation = new Quaternion(0, 0, currentZRotation + rotation, 0);
        //Vector3 currentPosition = transform.position;
        //int x = (int)currentPosition.x;
        //int y = (int)currentPosition.y - 1;
        //transform.position = new Vector3(x, y, 0);
        //transform.rotation = new Quaternion(0, 0, currentZRotation + 2 * rotation, 0);
        //goingRight = !goingRight;
        //yield return null;
        Vector3 curPos = transform.position;
        double step = 0.1;
        double rotationStep = 180 * step;  //because cell is 1, number of steps is 1/step and rotation step is 180/numOfSteps=180*step :)
        double currentRot = -180;
        if (goingRight)
        {
            rotationStep = -rotationStep;
            currentRot = 0;
        }
        goingRight = !goingRight;
        for (int i = 0; i < 10; i++)
        {
            float x = curPos.x;
            double y = curPos.y - (i + 1) * step;
            transform.position = new Vector3(x, (float)y, 0);
            transform.rotation = Quaternion.Euler(0, 0, (float)(currentRot + (i + 1) * rotationStep));
            yield return null;
        }
        if (!isHead)
        {
            goingForward = previousSegment.GetComponent<GridMovement>().goingForward;
        }
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
