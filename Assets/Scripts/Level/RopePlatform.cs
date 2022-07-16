using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RopePlatform : MonoBehaviour
{
    public float rotationSpeed;
    
    void Start()
    {

    }

    void Update()
    {
        transform.Rotate(transform.rotation.x, transform.rotation.y, rotationSpeed * Time.deltaTime);              
    }
}
