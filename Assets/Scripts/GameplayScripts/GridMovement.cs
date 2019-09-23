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
    public float delay = 0.3f;
    IEnumerator walkingCoroutine;
    bool justBecameHead = false;
    void Start()
    {
        gameController = GameObject.FindGameObjectWithTag("Controller");
        animator = gameObject.GetComponent<Animator>();
        ApplyAnimations();
        walkingCoroutine = MoveCentipede();
        if (isHead)
        {
            StartCoroutine(walkingCoroutine);
        }
    }
    void Update()
    {
        
    }

    #region animations
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

    #endregion

    void Die()
    {
        if (nextSegment != null)
        {
            nextSegment.GetComponent<GridMovement>().BecomeHead();
        }
        Destroy(gameObject);
    }


    #region Movement
    //this will determine where to go next and start coroutine
    IEnumerator MoveCentipede()
    {
        while (true)
        {
            bool isForward = false;
            if (!justBecameHead)
            {
                Vector3 currentPosition = transform.position;
                int step = 1;
                if (!goingRight)
                    step = -step;
                int x = (int)currentPosition.x + step;
                int y = (int)currentPosition.y;
                isForward = gameController.GetComponent<GameControllerScript>().IsStepAvailable(x, y);
            }
            else
            {
                justBecameHead = false;
            }
            goingForward = isForward;
            yield return StartCoroutine(MoveHeadTellTaleNextStep(isForward));
        }
    }

    IEnumerator MoveHeadTellTaleNextStep(bool isForward)
    {
        if (nextSegment != null)
        {
            TellNextStep(nextSegment, isForward, goingRight, 0);
        }
        yield return StartCoroutine(MakeStep(isForward));
        if (!isForward)
        {
            goingForward = true;
            goingRight = !goingRight;
        }
    }

    public void TellNextStep(GameObject next, bool forward, bool right, int segmentNum)
    {
        segmentNum++;
        print(segmentNum);
        StartCoroutine(next.GetComponent<GridMovement>().MakeStep(next.GetComponent<GridMovement>().goingForward));
        if (next.GetComponent<GridMovement>().nextSegment != null)
        {
            bool myForward = next.GetComponent<GridMovement>().goingForward;
            bool myRight = next.GetComponent<GridMovement>().goingRight;
            next.GetComponent<GridMovement>().TellNextStep(next.GetComponent<GridMovement>().nextSegment, myForward, myRight, segmentNum);
        }
        next.GetComponent<GridMovement>().goingForward = forward;
        next.GetComponent<GridMovement>().goingRight = right;
    } 


    IEnumerator MakeStep(bool forward)
    {
        if (forward)
        {
            yield return StartCoroutine(GoForward());
        }
        else
        {
            yield return StartCoroutine(GoDown());
        }
        if (health == 0) //probably check after making a step
        {
            Die();
            int x = (int)transform.position.x;
            int y = (int)transform.position.y;
            gameController.GetComponent<GameControllerScript>().AddMushroom(x, y);
        }
        yield return new WaitForSeconds(delay);
    }
    //use this if nothing blocking way
    IEnumerator GoForward()
    {
        
        int step = 1;
        if (!goingRight)
        {
            step = -step;
        }
        var nextPosition = transform.position + new Vector3(step, 0);
        transform.position = nextPosition;
        yield return null;
    }

    //use this if something in the way or centipede splitted
    IEnumerator GoDown()
    {
        
        var nextPosition = transform.position + new Vector3(0, -1);
        transform.position = nextPosition;
        transform.Rotate(0, 0, 180);
        yield return null;
    }

    #endregion

    void BecomeHead()
    {
        isHead = true;
        goingForward = false; //TODO head ignores this
        animator.runtimeAnimatorController = Resources.Load("Head01") as RuntimeAnimatorController;
        justBecameHead = true;
        StartCoroutine(walkingCoroutine);
    }

    #region Collisions
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.name == "Bullet(Clone)" && !collision.gameObject.GetComponent<BulletDamageHandler>().isColliding) //use tag instead
        {
            collision.gameObject.GetComponent<BulletDamageHandler>().isColliding = true;
            health--;
        }
    }

    #endregion
}
