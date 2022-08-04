using UnityEngine;

public class KeepLocalScale : MonoBehaviour
{
    public Transform transformToCopy;


    void Update()
    {
        transform.localScale = transformToCopy.localScale;
    }
}
