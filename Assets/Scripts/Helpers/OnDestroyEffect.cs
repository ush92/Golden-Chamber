using UnityEngine;

public class OnDestroyEffect : MonoBehaviour
{
    public GameObject destructEffect;
    private bool isDestroyed = false;

    public void FakeDestroy()
    {
        isDestroyed = true;
    }
    private void Update()
    {
        if(isDestroyed)
        {
            Instantiate(destructEffect, transform.position, transform.rotation);
            Destroy(gameObject);
        }
    }
}
