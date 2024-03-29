﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    public float PlayerSpeed = 0.5f;
    float MovementFieldHeight = 4f;
    public float PlayerSize = 1f;
    float maxRight, maxDown; 
    
    void Start()
    {
        //calculation of bounds (our first cell center is (0,0) one line is for score and extra lives, so topleft point of camera is (-0.5,1.5)
        MovementFieldHeight = GameObject.FindGameObjectWithTag("Controller").GetComponent<GameControllerScript>().PlayerZoneHeight;
        maxDown = 1.5f - 2 * Camera.main.orthographicSize; 
        float screenRatio = (float)Screen.width / Screen.height;  
        float screenWidth = 2 * Camera.main.orthographicSize * screenRatio;
        maxRight = screenWidth - 0.5f;
    }

    void Update()
    {
        // moving player with axis (gamepad, arrows etc.)
        Vector3 position = transform.position;
        position.y += Input.GetAxis("Vertical") * PlayerSpeed;
        position.x += Input.GetAxis("Horizontal") * PlayerSpeed;

        // checking that player stays in desired area
        position.y = Mathf.Clamp(position.y, maxDown + PlayerSize / 2, maxDown + MovementFieldHeight - PlayerSize / 2);

        
        position.x = Mathf.Clamp(position.x, -0.5f + PlayerSize / 2, maxRight - PlayerSize / 2);
        //transform.position = position;
        gameObject.GetComponent<Rigidbody2D>().MovePosition(position);

    }
}
