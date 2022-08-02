using UnityEngine;

public class StoneBoss : MonoBehaviour
{
    public GameObject collisionEffect;
    public Transform enablePoint;
    public FallingDmgObject fallingStone;

    private float leftRoom;
    private float rightRoom;

    public bool areRocksFallen = false;

    private void Start()
    {
        //check room dimensions for falling rocks area
        leftRoom = -27f;
        rightRoom = 3f;
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag.Equals(Consts.PLAYER) || other.gameObject.tag.Equals(Consts.GROUND))
        {
            Instantiate(collisionEffect, transform.position, transform.rotation);

            if (name == "StoneBoss1" && other.gameObject.tag.Equals(Consts.GROUND) && !areRocksFallen)
            {
                areRocksFallen = true;

                for (int i = 0; i < 8; i++)
                {
                    var rockPosition = new Vector3(Random.Range(leftRoom, rightRoom), enablePoint.transform.position.y + 0.8f, enablePoint.transform.position.z);
                    
                    Instantiate(fallingStone, rockPosition, enablePoint.transform.rotation);
                    Instantiate(collisionEffect, rockPosition, enablePoint.transform.rotation);
                }
            }

        }
    }
}
