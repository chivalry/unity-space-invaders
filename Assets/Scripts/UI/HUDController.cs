using TMPro;
using UnityEngine;

public class HUDController : MonoBehaviour
{
    [SerializeField] private TMP_Text scoreText;
    [SerializeField] private TMP_Text livesText;

    void OnEnable()
    {
        GameManager.Instance.OnScoreChanged += UpdateScore;
        GameManager.Instance.OnLivesChanged += UpdateLives;
    }

    void OnDisable()
    {
        GameManager.Instance.OnScoreChanged -= UpdateScore;
        GameManager.Instance.OnLivesChanged -= UpdateLives;
    }

    void Start()
    {
        UpdateScore(GameManager.Instance.Score);
        UpdateLives(GameManager.Instance.Lives);
    }

    private void UpdateScore(int score) =>
        scoreText.text = $"SCORE: {score:D4}";

    private void UpdateLives(int lives) =>
        livesText.text = $"LIVES: {lives}";
}
