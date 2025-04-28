using TMPro;
using UnityEngine;

public class Score : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI scoreText;
    private int score = 0;
    public void UpdateScore(int newScore)
    {
        score = newScore;
        if (scoreText != null)
        {
            scoreText.text = "Score: " + score;
        }
    }

    public int getScore()
    {
        return score;
    }
}
