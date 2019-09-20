using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    public float PlayerSpeed = 0.5f;
    public float MovementFieldHeight = 4f;
    public float PlayerSize = 1f;
    
    void Start()
    {
        
    }

    void Update()
    {
        // moving player with axis (gamepad, arrows etc.)
        Vector3 position = transform.position;
	    position.y += Input.GetAxis("Vertical") * PlayerSpeed;
	    position.x += Input.GetAxis("Horizontal") * PlayerSpeed;

        // checking that player stays in desired area
        position.y = Mathf.Clamp(position.y, -Camera.main.orthographicSize + PlayerSize/2, -Camera.main.orthographicSize + MovementFieldHeight - PlayerSize/2);

        float screenRatio = (float)Screen.width / Screen.height;
        float screenWidth = Camera.main.orthographicSize * screenRatio;
        position.x = Mathf.Clamp(position.x, -screenWidth + PlayerSize/2, screenWidth - PlayerSize/2);
        transform.position = position; 


    }
}
