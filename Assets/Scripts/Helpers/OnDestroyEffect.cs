using UnityEngine;

public class OnDestroyEffect : MonoBehaviour
{
    public GameObject destructEffect;

    public void FakeDestroy()
    {
        Instantiate(destructEffect, transform.position, transform.rotation);
    }
}
