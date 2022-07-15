using UnityEngine;
using UnityEngine.InputSystem.HID;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CompleteLevelScreen : MonoBehaviour
{
    public Text levelName;
    public Text recordTime;
        
    void Awake()
    {
        if (!SceneManager.GetActiveScene().name.Equals(Consts.LEVEL_MAP))
        {
            levelName.text = Consts.FINISH + Consts.GetLevelName(SceneManager.GetActiveScene().name);

            var timeInMinutes = GameManager.levelRecords[Consts.GetLevelIndex(SceneManager.GetActiveScene().name)][9] / 60;
            var timeInSeconds = GameManager.levelRecords[Consts.GetLevelIndex(SceneManager.GetActiveScene().name)][9] % 60;
            recordTime.text = timeInMinutes.ToString() + " min " + timeInSeconds.ToString() + "s";
        }
    }

    public void UpdateLevelTimeRecord(int levelIndex, int time)
    {
        var tmp = recordTime.text;
        if (time < GameManager.levelRecords[levelIndex][9] || GameManager.levelRecords[levelIndex][9] == 0)
        {
            GameManager.levelRecords[levelIndex][9] = time;
            recordTime.text = tmp;
        }
    }
}
