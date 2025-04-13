using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallaxController : MonoBehaviour
{
    private float startPos, length;
    public Camera cam;
    public float parallaxSpeed;

    void Start()
    {
        cam = Camera.main;
        startPos = transform.position.x;
        length = GetComponent<SpriteRenderer>().bounds.size.x;
    }
    void FixedUpdate()
    {
        float distance = cam.transform.position.x * parallaxSpeed;
        float backgroundMovement = cam.transform.position.x * (1 - parallaxSpeed);
        transform.position = new Vector3(startPos + distance, transform.position.y, transform.position.z);

        if(backgroundMovement > startPos + length)
        {
            startPos += length;
        }
        else if (backgroundMovement < startPos - length)
        {
            startPos -= length;
        } 
    }
}
