using UnityEngine;

public class FlyingAround : MonoBehaviour
{
    public float moveSpeed;
    public bool shouldAttack = false;
    public bool withReturn;

    public Transform startingPoint;
    private GameObject player;
    public Animator enemyAnimator;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag(Consts.PLAYER);
    }

    private void OnDisable()
    {
        transform.position = startingPoint.transform.position;
    }

    void Update()
    {
        if (!player)
        {
            return;
        }

        Animate();
 
        if (shouldAttack)
        {
            Attack();
        }
        else
        {
            ReturnStartPoint();
        }
    }

    private void ReturnStartPoint()
    {
        transform.position = Vector2.MoveTowards(transform.position, startingPoint.position, moveSpeed * Time.deltaTime);
    }

    private void Attack()
    {
        transform.position = Vector2.MoveTowards(transform.position, player.transform.position, moveSpeed * Time.deltaTime);
    }

    private void Animate()
    {
        if(Vector2.Distance(transform.position, player.transform.position) <= 3.0f)
        {
            enemyAnimator.SetBool("nearPlayer", true);
        }
        else
        {
            enemyAnimator.SetBool("nearPlayer", false);
        }
             
        if(transform.position.x > player.transform.position.x)
        {
           // transform.rotation = Quaternion.Euler(0, 0, 0);
            transform.localScale = new Vector3(-1f, 1f, 1f);
        }
        else
        {
            //transform.rotation = Quaternion.Euler(0, 180, 0);
            transform.localScale = new Vector3(1f, 1f, 1f);
        }
    }
}
