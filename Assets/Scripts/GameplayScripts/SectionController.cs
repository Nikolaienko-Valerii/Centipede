using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SectionController : MonoBehaviour
{
    public bool onField = false;
    public bool isHead = false;
    public bool goingRight = true;
    public GameObject previousSegment;
    public GameObject nextSegment;
    public bool goingForward = true;
    public int health = 1;
    public RuntimeAnimatorController HeadAnimation;
    public RuntimeAnimatorController TaleAnimation;
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

    #region animations
    void ApplyAnimations()
    {
        if (!isHead)
        {
            animator.runtimeAnimatorController = TaleAnimation;
        }
        else
        {
            animator.runtimeAnimatorController = HeadAnimation;
        }
    }

    #endregion


    #region Movement

    IEnumerator MoveCentipede()  //moving all centipede parts to their next position
    {
        yield return new WaitForSeconds(delay);
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
                if (onField)
                {
                    isForward = gameController.GetComponent<GameControllerScript>().IsStepAvailable(x, y);
                }
                else
                {
                    isForward = true;
                    onField = gameController.GetComponent<GameControllerScript>().IsOnField(x,y);
                }
                
            }
            else
            {
                justBecameHead = false;
            }
            goingForward = isForward;
            yield return StartCoroutine(MoveHeadTellTaleNextStep(isForward));
        }
    }

    IEnumerator MoveHeadTellTaleNextStep(bool isForward) // all tale parts receiving next direction and moving after that
    {
        if (nextSegment != null)
        {
            TellNextStep(nextSegment, isForward, goingRight, onField);
        }
        yield return StartCoroutine(MakeStep(isForward));
        if (!isForward)
        {
            goingForward = true;
            goingRight = !goingRight;
        }
    }

    public void TellNextStep(GameObject next, bool forward, bool right, bool isOnField)
    {
        StartCoroutine(next.GetComponent<SectionController>().MakeStep(next.GetComponent<SectionController>().goingForward));
        if (next.GetComponent<SectionController>().nextSegment != null)
        {
            bool myForward = next.GetComponent<SectionController>().goingForward;
            bool myRight = next.GetComponent<SectionController>().goingRight;
            bool myOnField = next.GetComponent<SectionController>().onField;
            next.GetComponent<SectionController>().TellNextStep(next.GetComponent<SectionController>().nextSegment, myForward, myRight, myOnField);
        }
        next.GetComponent<SectionController>().goingForward = forward;
        next.GetComponent<SectionController>().goingRight = right;
        next.GetComponent<SectionController>().onField = isOnField;
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
        if (health == 0) //checking is part still alive or not
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

    void Die()
    {
        if (nextSegment != null)
        {
            nextSegment.GetComponent<SectionController>().BecomeHead();
        }
        gameController.GetComponent<GameControllerScript>().CentipedePartDestroyed();
        Destroy(gameObject);
    }

    void BecomeHead()
    {
        isHead = true;
        goingForward = false; 
        animator.runtimeAnimatorController = HeadAnimation;
        justBecameHead = true;
        StartCoroutine(walkingCoroutine);
    }

    #region Collisions
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Bullet" && !collision.gameObject.GetComponent<BulletDamageHandler>().isColliding) //use tag instead
        {
            collision.gameObject.GetComponent<BulletDamageHandler>().isColliding = true;
            health--;

            gameObject.GetComponent<AudioSource>().Play();
        }
        else
        {
            if (collision.gameObject.tag == "Player")
            {
                gameController.GetComponent<GameControllerScript>().LostLife();
            }
        }
    }

    #endregion
}
