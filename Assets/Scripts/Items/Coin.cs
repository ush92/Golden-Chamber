using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour
{
    public int pointsToAdd;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag.Equals(Consts.PLAYER))
        {
            other.GetComponentInChildren<ScoreManager>().AddScore(pointsToAdd);

            Destroy(gameObject);
        }
    }
}
