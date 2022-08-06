using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnDestroyEffect : MonoBehaviour
{
    public GameObject destructEffect;

    private void OnDestroy()
    {
        Instantiate(destructEffect, transform.position, transform.rotation);
    }

    public void FakeDestroy()
    {
        Instantiate(destructEffect, transform.position, transform.rotation);
    }
}
