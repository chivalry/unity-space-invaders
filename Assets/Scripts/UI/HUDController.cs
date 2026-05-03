using TMPro;
using UnityEngine;

public class HUDController : MonoBehaviour
{
    [SerializeField] private TMP_Text scoreText;
    [SerializeField] private TMP_Text livesText;

    void Start()
    {
        GameManager.Instance.OnScoreChanged += UpdateScore;
        GameManager.Instance.OnLivesChanged += UpdateLives;
        UpdateScore(GameManager.Instance.Score);
        UpdateLives(GameManager.Instance.Lives);
    }

    void OnDestroy()
    {
        GameManager.Instance.OnScoreChanged -= UpdateScore;
        GameManager.Instance.OnLivesChanged -= UpdateLives;
    }

    private void UpdateScore(int score) =>
        scoreText.text = $"SCORE: {score:D4}";

    private void UpdateLives(int lives) =>
        livesText.text = $"LIVES: {lives}";
}
