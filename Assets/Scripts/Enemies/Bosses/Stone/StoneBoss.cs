using UnityEngine;

public class StoneBoss : MonoBehaviour
{
    public GameObject collisionEffect;
    public Transform enablePoint;
    public FallingDmgObject fallingStone;

    private float leftRoom;
    private float rightRoom;

    public bool areRocksFallen = false;
    public int fallenRocksCount;

    public float fadeAnimationTime = 1.0f;
    private float fadeAnimationCounter;

    private void Start()
    {
        //check room dimensions for falling rocks area
        leftRoom = -27f;
        rightRoom = 3f;

        transform.name = transform.name.Replace("(Clone)", "").Trim();
    }

    private void OnEnable()
    {
        fadeAnimationCounter = fadeAnimationTime;
        GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 0);
    }

    private void Update()
    {
        if(fadeAnimationCounter > 0)
        {
            fadeAnimationCounter -= Time.deltaTime;
            GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 1 - fadeAnimationCounter);
        }
        else
        {
            fadeAnimationCounter = 0;
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag.Equals(Consts.PLAYER) || other.gameObject.tag.Equals(Consts.GROUND))
        {
            Instantiate(collisionEffect, transform.position, transform.rotation);

            if (name == "StoneBoss1" && other.gameObject.tag.Equals(Consts.GROUND) && !areRocksFallen)
            {
                areRocksFallen = true;

                for (int i = 0; i < fallenRocksCount; i++)
                {
                    var rockPosition = new Vector3(Random.Range(leftRoom, rightRoom), enablePoint.transform.position.y + 0.8f, enablePoint.transform.position.z);
                    
                    Instantiate(fallingStone, rockPosition, enablePoint.transform.rotation);
                    Instantiate(collisionEffect, rockPosition, enablePoint.transform.rotation);
                }
            }

        }
    }
}
