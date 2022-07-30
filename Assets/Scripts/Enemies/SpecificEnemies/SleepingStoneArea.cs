using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SleepingStoneArea : MonoBehaviour
{
    public SleepingStone stone;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag.Equals(Consts.PLAYER))
        {
            stone.isSleepingNow = false;
        }
    }
}
