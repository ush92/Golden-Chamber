using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PolarityDebuf : MonoBehaviour
{
    public bool isPlus;
    public Sprite plus;
    public Sprite minus;

    void Update()
    {
        if(isPlus)
        {
            GetComponent<SpriteRenderer>().sprite = plus;
        }
        else
        {
            GetComponent<SpriteRenderer>().sprite = minus;
        }
    }
}
