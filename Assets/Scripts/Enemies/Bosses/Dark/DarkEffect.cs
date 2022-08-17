using UnityEngine;

public class DarkEffect : MonoBehaviour
{
    private PlayerController player;
    private float offset;
    private bool isGoingUp;

    void Start()
    {
        player = FindObjectOfType<PlayerController>();

        offset = 0f;
        transform.position = new Vector3(50, offset, 0);
        isGoingUp = true;
    }

    void Update()
    {
        if(player != null)
        {
            if(player.isBossEncounter)
            {
                transform.position = new Vector3(50, -40, 0);
            }
            else
            {
                if(isGoingUp)
                {
                    offset += Time.deltaTime * 2.0f;
                    if(offset >= 55f)
                    {
                        isGoingUp = false;
                    }
                }
                else
                {
                    offset -= Time.deltaTime;
                    if (offset <= 0f)
                    {
                        isGoingUp = true;
                    }
                }

                transform.position = new Vector3(50, offset, 0);
            }
        }
    }
}
