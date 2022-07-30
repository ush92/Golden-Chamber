using UnityEngine;

public class OneWayTile : MonoBehaviour
{
    private PlatformEffector2D pEffector;
    public PlayerController player;

    void Start()
    {
        pEffector = GetComponent<PlatformEffector2D>();
        player = FindObjectOfType<PlayerController>();
    }

    private void Update()
    {
        if(player.oneWayTileCounter > 0)
        {
            pEffector.rotationalOffset = 180f;
        }
        else
        {
            pEffector.rotationalOffset = 0f;
        }
    }

}
