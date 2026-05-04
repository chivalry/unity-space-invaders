using TMPro;
using UnityEngine;

/// <summary>
/// Updates the on-screen HUD by subscribing to <see cref="GameManager"/> score
/// and lives events, and initializes the display from current state on startup.
/// </summary>
public class HUDController : MonoBehaviour
{
    /// <summary>Text element that displays the current score.</summary>
    [SerializeField] private TMP_Text scoreText;

    /// <summary>Text element that displays remaining lives.</summary>
    [SerializeField] private TMP_Text livesText;

    /// <summary>
    /// Subscribes to score and lives change events and populates both displays
    /// with the current values from <see cref="GameManager"/>.
    /// </summary>
    void Start()
    {
        GameManager.Instance.OnScoreChanged += UpdateScore;
        GameManager.Instance.OnLivesChanged += UpdateLives;
        UpdateScore(GameManager.Instance.Score);
        UpdateLives(GameManager.Instance.Lives);
    }

    /// <summary>Unsubscribes from <see cref="GameManager"/> events to prevent dangling delegates.</summary>
    void OnDestroy()
    {
        GameManager.Instance.OnScoreChanged -= UpdateScore;
        GameManager.Instance.OnLivesChanged -= UpdateLives;
    }

    /// <summary>Updates the score display, zero-padded to four digits.</summary>
    /// <param name="score">The new score value.</param>
    private void UpdateScore(int score) =>
        scoreText.text = $"SCORE: {score:D4}";

    /// <summary>Updates the lives display.</summary>
    /// <param name="lives">The new lives value.</param>
    private void UpdateLives(int lives) =>
        livesText.text = $"LIVES: {lives}";
}
