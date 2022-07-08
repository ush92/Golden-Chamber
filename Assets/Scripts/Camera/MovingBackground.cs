using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class MovingBackground : MonoBehaviour
{
    private int index = 0;
    private float speed = 0.1f;

    void FixedUpdate()
    {
        index++;

        if(index >= 0 && index < 200)
        {
            transform.position = new Vector3(transform.position.x + speed, transform.position.y + speed, transform.position.z);
        }
        else if (index >= 200 && index < 400)
        {
            transform.position = new Vector3(transform.position.x - speed, transform.position.y + speed, transform.position.z);
        }
        else if (index >= 400 && index < 600)
        {
            transform.position = new Vector3(transform.position.x - speed, transform.position.y - speed, transform.position.z);
        }
        else if (index >= 600 && index < 800)
        {
            transform.position = new Vector3(transform.position.x + speed, transform.position.y - speed, transform.position.z);
        }
        else
        {
            index = 0;
        }
    }
}
