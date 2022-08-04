using UnityEngine;

public class EvilSun : MonoBehaviour
{
    private GameObject player;
    public float moveSpeed;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag(Consts.PLAYER);
    }

    void Update()
    {
        var newPos = new Vector3(player.transform.position.x, player.transform.position.y + 5.2f, transform.position.z);
        transform.position = Vector2.MoveTowards(transform.position, newPos, moveSpeed * Time.deltaTime);
    }
}
