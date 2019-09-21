using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    public float PlayerSpeed = 0.5f;
    public float MovementFieldHeight = 4f;
    public float PlayerSize = 1f;
    float maxRight, maxDown; 
    
    void Start()
    {
        maxDown = 0.5f - 2 * Camera.main.orthographicSize; //because we moved camera to make our topleft point -0.5;0.5 to make first cell in zero coordinates
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
        transform.position = position;


    }
}
