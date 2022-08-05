using UnityEngine;

public class FixedRotation : MonoBehaviour
{
    public float fixAngle;

    void Update()
    {
        if(transform.localScale.x == -1)
        {
            var rotationVector = transform.rotation.eulerAngles;
            rotationVector.z = -fixAngle + transform.rotation.z;
            transform.rotation = Quaternion.Euler(rotationVector);
        }
    }
}
