using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BouncingPad : MonoBehaviour
{
    public SpriteRenderer padSR;
    public Sprite downPad, upPad;

    public float stayUpTime;
    public float bouncePower;
    private float upCounter;

    void Start()
    {
        
    }

    void Update()
    {
        if(upCounter > 0)
        {
            upCounter -= Time.deltaTime;
        }

        if (upCounter <= 0)
        {
            padSR.sprite = downPad;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag.Equals(Consts.PLAYER))
        {
            upCounter = stayUpTime;
            padSR.sprite = upPad;

            var playerRB = other.GetComponent<Rigidbody2D>();
            playerRB.velocity = new Vector2(playerRB.velocity.x, bouncePower);
        }
    }
}
