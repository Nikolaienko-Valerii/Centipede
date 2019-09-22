using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletSelfDestroy : MonoBehaviour
{
    void Update()
    {
        if (transform.position.y > Camera.main.orthographicSize)
            Destroy(gameObject);
    }
}
