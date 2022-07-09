using UnityEngine;

public class LevelMapController : MonoBehaviour
{
    public Transform levelDoorsParent;
    public Sprite uncompletedLevelSprite;
    public Sprite completedLevelSprite;

    private bool isRefreshed = false;

    void Start()
    {
        GameManager.LoadJsonData(GameManager.instance);
    }

    void Update()
    {
        if(!isRefreshed)
        {
            int index = 0;
            foreach (Transform door in levelDoorsParent)
            {
                if (GameManager.levelList[index] == true)
                {
                    door.gameObject.GetComponent<SpriteRenderer>().sprite = completedLevelSprite;
                }
                else
                {
                    door.gameObject.GetComponent<SpriteRenderer>().sprite = uncompletedLevelSprite;
                }
                index++;
            }

            isRefreshed = true;
        }
    }
}
