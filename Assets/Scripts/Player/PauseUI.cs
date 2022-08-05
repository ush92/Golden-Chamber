using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseUI : MonoBehaviour
{
    public Text levelName;
    public Text bestTime;
    public Text currentTime;

    public PlayerController playerController;

    void Start()
    {
        if (SceneManager.GetActiveScene().name.Equals(Consts.LEVEL_MAP))
        {
            bestTime.text = "";
            currentTime.text = "";
        }
        else
        {
            var timeInMinutes = GameManager.levelRecords[Consts.GetLevelIndex(SceneManager.GetActiveScene().name)][9] / 60;
            var timeRestInSeconds = GameManager.levelRecords[Consts.GetLevelIndex(SceneManager.GetActiveScene().name)][9] % 60;
            bestTime.text = Consts.RECORD_TIME + timeInMinutes.ToString() + " min " + timeRestInSeconds.ToString() + "s";
        }

        levelName.text = Consts.GetLevelName(SceneManager.GetActiveScene().name);
    }

    void Update()
    {
        if (!SceneManager.GetActiveScene().name.Equals(Consts.LEVEL_MAP))
        {
            currentTime.text = Consts.CURRENT_TIME + playerController.currentTime.text;
        }
    }
}
