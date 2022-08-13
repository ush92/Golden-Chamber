using System.Collections.Generic;
using UnityEngine;

public class PoisonBehaviour : MonoBehaviour
{
    public BossActivation activationArea;
    public Animator animator;
    public Transform basePosition;
    public EnemyHPController bossHP;
    public PlayerController player;

    public GameObject toxinDebuf;
    private GameObject currentDebuff;
    public List<GameObject> toxinClouds;
    public GameObject debuffParticles;

    public float biteAnimationCooldown;
    private float biteAnimationTimer;

    public Collectable poisonWeaponLoot;

    private void OnEnable()
    {
        player.isBossEncounter = true;

        transform.position = basePosition.transform.position;
        InvokeRepeating("InfectPlayer", 5, 9.0f);
        biteAnimationTimer = 0;
    }

    private void Update()
    {
        if(biteAnimationTimer > 0)
        {
            biteAnimationTimer -= Time.deltaTime;
        }

        if (biteAnimationTimer < 0)
        {
            biteAnimationTimer = 0;
        }

        if (!player.isBossEncounter || !player.isActive)
        {
            CancelInvoke();

            Destroy(currentDebuff);
            foreach (var cloud in toxinClouds)
            {
                Destroy(cloud);
            }

            GetComponentInChildren<EnemyHPController>().ResetHP();
            gameObject.SetActive(false);
        }
    }

    void InfectPlayer()
    {
        if (player != null)
        {
            animator.SetTrigger("Shoot");
            Instantiate(debuffParticles, transform.position, transform.rotation);
            currentDebuff = Instantiate(toxinDebuf);
            currentDebuff.transform.parent = player.transform;
            currentDebuff.transform.position = new Vector3(player.transform.position.x, player.transform.position.y + 0.7f, player.transform.position.z);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag.Equals(Consts.PLAYER) && biteAnimationTimer == 0)
        {
            animator.SetTrigger("Bite");
            biteAnimationTimer = biteAnimationCooldown;
        }
    }

    private void OnDestroy()
    {
        if (bossHP.currentHP <= 0)
        {
            player.isBossEncounter = false;
            Instantiate(poisonWeaponLoot, transform.position, transform.rotation);

            activationArea.gameObject.SetActive(false);

            Destroy(currentDebuff);
            foreach (var cloud in toxinClouds)
            {
                Destroy(cloud);
            }
        }
    }
}
