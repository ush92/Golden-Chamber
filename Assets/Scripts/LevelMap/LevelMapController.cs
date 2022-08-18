using UnityEngine;

public class LevelMapController : MonoBehaviour
{
    public Transform levelDoorsParent;
    public Sprite uncompletedLevelSprite;
    public Sprite completedLevelSprite;

    public Transform levels3Block;
    public Transform levels4Block;

    public Transform epicTreasure;

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

            levels4Block.gameObject.SetActive(false);
            epicTreasure.gameObject.SetActive(true);

            foreach (Transform door in levelDoorsParent)
            {
                if (GameManager.levelList[index] == true)
                {
                    door.gameObject.GetComponent<SpriteRenderer>().sprite = completedLevelSprite;
                }
                else
                {
                    door.gameObject.GetComponent<SpriteRenderer>().sprite = uncompletedLevelSprite;
                    epicTreasure.gameObject.SetActive(false);
                    if (index != Consts.GetLevelIndex(Consts.LEVEL4_1))
                    {
                        levels4Block.gameObject.SetActive(true);
                    }
                }
                index++;
            }

            levels3Block.gameObject.SetActive(true);
            if (GameManager.levelList[Consts.GetLevelIndex(Consts.LEVEL1_3)] == true &&
                GameManager.levelList[Consts.GetLevelIndex(Consts.LEVEL2_3)] == true)
            {
                levels3Block.gameObject.SetActive(false);
            }

            isRefreshed = true;
        }
    }
}
