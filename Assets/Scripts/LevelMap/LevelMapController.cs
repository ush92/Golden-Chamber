using UnityEngine;

public class LevelMapController : MonoBehaviour
{
    public Transform levelDoorsParent;
    public Sprite uncompletedLevelSprite;
    public Sprite completedLevelSprite;

    public Transform levels3Block;
    public Transform levels4Block;
    public Transform levels5Block;

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

            levels5Block.gameObject.SetActive(false);

            foreach (Transform door in levelDoorsParent)
            {
                if (GameManager.levelList[index] == true)
                {
                    door.gameObject.GetComponent<SpriteRenderer>().sprite = completedLevelSprite;
                }
                else
                {
                    door.gameObject.GetComponent<SpriteRenderer>().sprite = uncompletedLevelSprite;

                    if (index != Consts.GetLevelIndex(Consts.LEVEL5_1))
                    {
                        levels5Block.gameObject.SetActive(true);
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

            levels4Block.gameObject.SetActive(true);
            if (GameManager.levelList[Consts.GetLevelIndex(Consts.LEVEL1_3)] == true && GameManager.levelList[Consts.GetLevelIndex(Consts.LEVEL2_3)] == true &&
                GameManager.levelList[Consts.GetLevelIndex(Consts.LEVEL3_1)] == true && GameManager.levelList[Consts.GetLevelIndex(Consts.LEVEL3_2)] == true &&
                GameManager.levelList[Consts.GetLevelIndex(Consts.LEVEL3_3)] == true && GameManager.levelList[Consts.GetLevelIndex(Consts.LEVEL3_4)] == true)
            {
                levels4Block.gameObject.SetActive(false);
            }

            isRefreshed = true;
        }
    }
}
