using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmoothFollow : MonoBehaviour
{
    public Transform target;
    public Vector3 offset;
    public float damping;

    public bool isLookingUp;
    public float lookingUpOffset;
    private float lookingUpCurrentOffset;

    private Vector3 velocity = Vector3.zero;

    void FixedUpdate()
    {
        if (!isLookingUp)
        {
            Vector3 movePos = target.position + offset;
            transform.position = Vector3.SmoothDamp(transform.position, movePos, ref velocity, damping);

            lookingUpCurrentOffset = 0;
        }
        else
        {
            transform.position = new Vector3(target.position.x, target.position.y + lookingUpCurrentOffset, transform.position.z);

            if (lookingUpCurrentOffset >= lookingUpOffset)
            {
                lookingUpCurrentOffset = lookingUpOffset;
            }
            else if (lookingUpCurrentOffset < lookingUpOffset / 1.5f)
            {
                lookingUpCurrentOffset += 10 * Time.deltaTime;
            }
            else if (lookingUpCurrentOffset >= lookingUpOffset / 1.5f)
            {
                lookingUpCurrentOffset += 2 * Time.deltaTime;
            }
        }
    }
}
