using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CentipedeSpawner : MonoBehaviour
{
    public GameObject CentipedePrefab;
    public GameObject TalePrefab;
    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public GameObject Spawn(int length, Vector3 position, Quaternion rotation)
    {
        GameObject centipede = Instantiate(CentipedePrefab, position, rotation);
        GameObject[] parts;
        if (length > 0)
        {
            parts = new GameObject[length];
            parts[0] = Instantiate(TalePrefab, centipede.transform);
            for (int i = 1; i < length; i++)
            {
                parts[i] = Instantiate(TalePrefab, centipede.transform);
                Vector3 pos = parts[i].transform.position;
                pos.x -= i;
                parts[i].transform.position = pos;

                parts[i].GetComponent<PartController>().NextPart = parts[i - 1];
                parts[i - 1].GetComponent<PartController>().PreviousPart = parts[i];
            }
        }
        return centipede;
    }

    public GameObject CreateByHead(GameObject head)
    {
        GameObject centipede = Instantiate(CentipedePrefab, head.transform.position, head.transform.rotation);
        ChangeParent(head, centipede);
        return centipede;
    }

    void ChangeParent(GameObject centipedePart, GameObject centipede)
    {
        centipedePart.transform.SetParent(centipede.transform);
        GameObject previousPart = centipedePart.GetComponent<PartController>().PreviousPart;
        if (previousPart != null)
        {
            ChangeParent(previousPart, centipede);
        }
    }
}
