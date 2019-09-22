using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CentipedeController : MonoBehaviour
{
    public GameObject TalePrefab;
    GameObject head;

    void Start()
    {
        
    }

    void Update()
    {
        if (gameObject.transform.childCount == 0)
        {
            Destroy(gameObject);
        }
    }
}
