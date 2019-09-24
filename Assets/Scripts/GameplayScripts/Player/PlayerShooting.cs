using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShooting : MonoBehaviour
{
    public GameObject Bullet;
    public GameObject ShootingPoint;
    public float Delay = 0.3f;
    float timer = 0f;
    void Update()
    {
        timer -= Time.deltaTime;

        if (Input.GetButton("Fire1") & timer <= 0)
        {
            Instantiate(Bullet, ShootingPoint.transform.position, ShootingPoint.transform.rotation).tag = "Bullet";
            timer = Delay;
            gameObject.GetComponent<AudioSource>().Play();
        }
    }
}
