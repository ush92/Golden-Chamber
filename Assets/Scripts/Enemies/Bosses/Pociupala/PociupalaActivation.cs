using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PociupalaActivation : MonoBehaviour
{
    public PociupalaBehaviour pociupala;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag.Equals(Consts.PLAYER))
        {
            pociupala.gameObject.SetActive(true);
        }
    }
}
