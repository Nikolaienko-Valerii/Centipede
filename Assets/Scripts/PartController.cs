using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PartController : MonoBehaviour
{
    public GameObject NextPart;
    public GameObject PreviousPart;
    public int health = 1;
    public float speed = 0.3f;

    Animator animator;
    void Start()
    {
        animator = gameObject.GetComponent<Animator>();
        if (NextPart!=null)
        {
            animator.runtimeAnimatorController = Resources.Load("Tale01") as RuntimeAnimatorController;
        }
        else
        {
            animator.runtimeAnimatorController = Resources.Load("Head01") as RuntimeAnimatorController;
        }
    }

    void Update()
    {
        GoForward();
        if (health <= 0)
        {
            Destroy(gameObject);
        }
        if (NextPart==null)
        {
            animator.runtimeAnimatorController = Resources.Load("Head01") as RuntimeAnimatorController;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.name == "Bullet(Clone)")
        {
            health--;
        }
        else
        {
            StartCoroutine(GoDown());
            gameObject.GetComponent<Collider2D>().enabled = false;
            print(collision.gameObject.name);
        }
    }

    private int CalculateLength(GameObject Head)
    {
        if (Head.GetComponent<PartController>().PreviousPart == null)
        {
            return 1;
        }
        else
        {
            return 1 + CalculateLength(Head.GetComponent<PartController>().PreviousPart);
        }
    }

    void GoForward()
    {
        Vector3 position = transform.position;
        Vector3 velocity = new Vector3(speed, 0, 0);
        Quaternion rotation = transform.rotation;
        position += rotation * velocity;
        transform.position = position;
    }

    IEnumerator GoDown()
    {
        Quaternion rotation = transform.rotation;
        float zRotation = rotation.eulerAngles.z;
        float rotationSpeed = (speed * 180) / (Mathf.PI * 0.5f); //needed rotation per tick to move down one line while rotating
        float yCoord = transform.position.y;
        float yDestination = yCoord - 1;  //because of float we won't move actually one line so we will compensate this; 
        int direction;
        float totalRotation = 0;
        if (zRotation == 0)
        {
            direction = -1;
            print(zRotation);
        }
        else
        {
            direction = 1;
            print(zRotation);
        }
        while (totalRotation < 180)
        {
            totalRotation += rotationSpeed;
            if (totalRotation > 180)
            {
                totalRotation = 180;
                if (direction == -1)
                {
                    transform.rotation = new Quaternion(0, 0, -1, 0);
                }
                else
                {
                    transform.rotation = new Quaternion(0, 0, 0, 0);  //probably bad idea, but because of floats we never get zero, and I am not so good with Quaternions directly
                }
                if (transform.position.y < yDestination) //fixing y position, probably not good too, but because of floats we moving a bit more down than nedeed
                {
                    Vector3 pos = transform.position;
                    pos.y = yDestination;
                    transform.position = pos;
                }
                gameObject.GetComponent<Collider2D>().enabled = true;
            }
            else
            {
                rotation = Quaternion.Euler(0, 0, zRotation + direction * totalRotation);
                transform.rotation = rotation;
            }
            yield return null;
        }
    }
}
