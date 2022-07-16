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
            Debug.Log(Vector2.Distance(transform.position, player.transform.position));
        }
        
        
        if(transform.position.x > player.transform.position.x)
        {
            transform.rotation = Quaternion.Euler(0, 0, 0);
        }
        else
        {
            transform.rotation = Quaternion.Euler(0, 180, 0);
        }
    }
}
