using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
    public int score;

    Text scoreText;

    private void Start()
    {
        scoreText = GetComponent<Text>();
        score = 0;
    }

    private void Update()
    {
        if(score < 0)
        {
            score = 0;
        }

        scoreText.text = score.ToString();
    }

    public void AddScore (int scoreToAdd)
    {
        score += scoreToAdd;
    }

    public void ResetScore(int scoreToAdd)
    {
        score = 0;
    }
}
