using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BonusController : MonoBehaviour
{
    public float FallSpeed = 5f;
    GameObject gameController;
    void Start()
    {
        gameController = GameObject.FindGameObjectWithTag("Controller");
    }

    void Update()
    {
        transform.position = transform.position + new Vector3(0, -FallSpeed * Time.deltaTime, 0);
        // check if it is still on the screen, if not - destroy
        float bottomY = Camera.main.transform.position.y - Camera.main.orthographicSize;
        if (transform.position.y < bottomY)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            gameController.GetComponent<GameControllerScript>().AddLife();
            Destroy(gameObject);
        }
    }
}
