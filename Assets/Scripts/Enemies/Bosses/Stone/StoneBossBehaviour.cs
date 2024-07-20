using UnityEngine;

public class StoneBossBehaviour : MonoBehaviour
{
    public GameObject activactionArea;

    public float shiftTime;
    public float enrageShiftTime;
    public float fallDelay;
    private float fallTimer;
    public float gravity = 3;
    public Transform enablePoint;
    public GameObject collisionEffect;

    public StoneBoss bossForm1prefab;
    private StoneBoss stoneForm1;
    public GameObject stoneForm1FixedBorder; //to avoid move player out of room on heavy hit collision
    private bool isStoneForm1 = false;
    public float stone1AttackDelay;
    private float stone1AttackTimer;
    private bool wasStone1Attacked;
    public float stone1MoveSpeed;
    private bool isStone1Dead = false;

    public StoneBoss bossForm2prefab;
    private StoneBoss stoneForm2;
    private bool isStoneForm2 = true;
    public FlyingAround form2SpikeBall;
    public float spikeBallEnragedMoveSpeed = 5.0f;
    public float spikeBallNormalMoveSpeed = 3.0f;
    private bool isStone2Dead = false;

    public PlayerController player;
    private int playerSide;

    private bool isEnraged = false;

    private bool isLootAppeared = false;
    public Collectable hpMaxPlus5;
    public Collectable stoneWeapon;
    public Transform item1Location;
    public Transform item2Location;

    private void OnEnable()
    {
        player.isBossEncounter = true;

        if (stoneForm1 == null)
        {
            stoneForm1 = Instantiate(bossForm1prefab, enablePoint.transform.position, enablePoint.transform.rotation, transform);
            stoneForm1.enablePoint = enablePoint;
        }

        if (stoneForm2 == null)
        {
            stoneForm2 = Instantiate(bossForm2prefab, enablePoint.transform.position, enablePoint.transform.rotation, transform);
            stoneForm2.enablePoint = enablePoint;
            form2SpikeBall = stoneForm2.GetComponentInChildren<FlyingAround>();
            form2SpikeBall.moveSpeed = spikeBallNormalMoveSpeed;
        }
     
        stoneForm1.gameObject.SetActive(true);
        stoneForm2.gameObject.SetActive(false);
        isStoneForm2 = true; //set form2 just before switch forms
        InvokeRepeating("SwitchForms", 0f, shiftTime);

        isStone1Dead = false;
        isStone2Dead = false;
    }

    void Update()
    {
        CheckPlayer();

        if (isStone1Dead && isStone2Dead)
        {
            if (isLootAppeared == false)
            {
                isLootAppeared = true;
                activactionArea.SetActive(false);
                Instantiate(hpMaxPlus5, item1Location.position, item1Location.rotation);
                Instantiate(stoneWeapon, item2Location.position, item2Location.rotation);
            }          
        }

        if (player.isActive == false || player.isBossEncounter == false)
        {
            return;
        }

        if (isStoneForm1 && stoneForm1)
        { 
            if (fallTimer > 0)
            {
                stoneForm1.GetComponent<Rigidbody2D>().gravityScale = 0;
                stoneForm1FixedBorder.gameObject.SetActive(false);
                fallTimer -= Time.deltaTime;
            }
            else
            {
                stoneForm1.GetComponent<Rigidbody2D>().gravityScale = gravity;
                stoneForm1FixedBorder.gameObject.SetActive(true);
                fallTimer = 0;

                if (!wasStone1Attacked)
                {
                    stone1AttackTimer -= Time.deltaTime;
                }

                if (stone1AttackTimer < 0)
                {
                    Form1Behaviour();
                    stone1AttackTimer = 0;
                    wasStone1Attacked = true;
                }
            }

            if(stoneForm1.GetComponent<EnemyHPController>().currentHP <= 0)
            {
                isStone1Dead = true;
            }
        }

        if (isStoneForm2 && stoneForm2)
        {
            stoneForm1FixedBorder.gameObject.SetActive(false);
            if (fallTimer > 0)
            {
                stoneForm2.GetComponent<Rigidbody2D>().gravityScale = 0;        
                fallTimer -= Time.deltaTime;
            }
            else
            {
                stoneForm2.GetComponent<Rigidbody2D>().gravityScale = gravity;
                fallTimer = 0;
            }

            if (stoneForm2.GetComponent<EnemyHPController>().currentHP <=0)
            {
                isStone2Dead = true;
                form2SpikeBall.gameObject.SetActive(false);
            }
        }

        if(!isEnraged && (!stoneForm1 || !stoneForm2))
        {
            isEnraged = true;
            CancelInvoke("SwitchForms");
            InvokeRepeating("SwitchForms", enrageShiftTime, enrageShiftTime);
            
            if (stoneForm2)
            {
                stoneForm1FixedBorder.gameObject.SetActive(false);
                stoneForm2.transform.position = new Vector3(player.transform.position.x, enablePoint.transform.position.y, transform.position.z);
                form2SpikeBall.moveSpeed = spikeBallEnragedMoveSpeed;
            }
        }
    }

    private void CheckPlayer()
    {         
        if (player.isBossEncounter == false)
        {
            CancelInvoke("SwitchForms");

            if (stoneForm1)
            {
                stoneForm1.GetComponentInChildren<EnemyHPController>().ResetHP();
                stoneForm1.areRocksFallen = false;
                stoneForm1.gameObject.SetActive(false);
            }

            if (stoneForm2)
            {
                stoneForm2.GetComponentInChildren<EnemyHPController>().ResetHP();
                stoneForm2.gameObject.SetActive(false);
            }

            stoneForm1FixedBorder.gameObject.SetActive(false);
            stone1AttackTimer = stone1AttackDelay;
            wasStone1Attacked = false;

            this.gameObject.SetActive(false);
        }
    }

    private void Form1Behaviour()
    {
        if (stoneForm1.transform.position.x > player.transform.position.x)
        {
            playerSide = -1;
        }
        else if (transform.position.x < player.transform.position.x)
        {
            playerSide = 1;
        }

        stoneForm1.GetComponent<Rigidbody2D>().linearVelocity = new Vector2(playerSide * stone1MoveSpeed, stoneForm1.GetComponent<Rigidbody2D>().linearVelocity.y);
    }

    private void CheckBossActivity()
    {
        if (stoneForm1 == null && stoneForm2 == null)
        {
            isStoneForm1 = false;
            isStoneForm2 = false;
            player.isBossEncounter = false;
        }
        else if (stoneForm1 == null && stoneForm2)
        {
            fallTimer = fallDelay;
            stoneForm2.gameObject.SetActive(true);
            stoneForm1FixedBorder.gameObject.SetActive(false);
            isStoneForm1 = false;
            isStoneForm2 = true;
        }
        else if (stoneForm1 && stoneForm2 == null)
        {
            fallTimer = fallDelay;
            stoneForm1.gameObject.SetActive(true);

            stoneForm1FixedBorder.gameObject.SetActive(true);
            isStoneForm1 = true;
            isStoneForm2 = false;
            stoneForm1.areRocksFallen = false;

            isEnraged = false;         
        }
    }

    private void SwitchForms()
    {
        CheckBossActivity();

        fallTimer = fallDelay;

        if (stoneForm1 == null || stoneForm2 == null)
        {
            if (stoneForm1)
            {
                stoneForm1.gameObject.SetActive(false);
                stoneForm1.gameObject.SetActive(true);

                stoneForm1FixedBorder.gameObject.SetActive(true);
                stoneForm1.transform.position = new Vector3(enablePoint.transform.position.x + 15, enablePoint.transform.position.y, transform.position.z);
                stone1AttackTimer = stone1AttackDelay;
                wasStone1Attacked = false;
                stoneForm1.areRocksFallen = false;
            }

            if (stoneForm2)
            {
                stoneForm1FixedBorder.gameObject.SetActive(false);
            }
        }
        else
        {
            if (isStoneForm1)
            {
                if (stoneForm2)
                {
                    stoneForm2.transform.position = new Vector3(player.transform.position.x, enablePoint.transform.position.y, transform.position.z);
                    stoneForm1FixedBorder.gameObject.SetActive(false);
                    stoneForm1.gameObject.SetActive(false);
                    stoneForm2.gameObject.SetActive(true);

                }
            }
            else
            {
                if (stoneForm1)
                {
                    stoneForm1.transform.position = new Vector3(enablePoint.transform.position.x + 15, enablePoint.transform.position.y, transform.position.z);
                    stoneForm1FixedBorder.gameObject.SetActive(true);
                    stoneForm1.gameObject.SetActive(true);
                    stoneForm2.gameObject.SetActive(false);

                    stone1AttackTimer = stone1AttackDelay;
                    wasStone1Attacked = false;
                    stoneForm1.areRocksFallen = false;
                }
            }

            isStoneForm1 = !isStoneForm1;
            isStoneForm2 = !isStoneForm2;
        }
    }
}
