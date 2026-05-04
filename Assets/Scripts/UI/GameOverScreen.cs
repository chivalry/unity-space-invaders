using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Manages the end-of-game overlay shown on both victory and defeat.
/// Subscribes to <see cref="GameManager"/> events to activate itself and sets
/// the title text to reflect the outcome.
/// </summary>
public class GameOverScreen : MonoBehaviour
{
    /// <summary>Displays "GAME OVER" or "YOU WIN" depending on the outcome.</summary>
    [SerializeField] private TMP_Text titleText;

    /// <summary>Displays the player's final score.</summary>
    [SerializeField] private TMP_Text finalScoreText;

    /// <summary>Reloads the first scene to restart the game.</summary>
    [SerializeField] private UnityEngine.UI.Button playAgainButton;

    /// <summary>Calls <see cref="Application.Quit"/> to exit the application.</summary>
    [SerializeField] private UnityEngine.UI.Button quitButton;

    /// <summary>
    /// Subscribes to game-over and game-won events, wires button callbacks,
    /// and hides the screen until an event fires.
    /// </summary>
    void Start()
    {
        GameManager.Instance.OnGameOver += ShowGameOver;
        GameManager.Instance.OnGameWon += ShowGameWon;
        playAgainButton.onClick.AddListener(() => SceneManager.LoadScene(0));
        quitButton.onClick.AddListener(() => Application.Quit());
        gameObject.SetActive(false);
    }

    /// <summary>Unsubscribes from <see cref="GameManager"/> events to prevent dangling delegates.</summary>
    void OnDestroy()
    {
        GameManager.Instance.OnGameOver -= ShowGameOver;
        GameManager.Instance.OnGameWon -= ShowGameWon;
    }

    /// <summary>Sets the title to "GAME OVER" and displays the screen.</summary>
    private void ShowGameOver()
    {
        titleText.text = "GAME OVER";
        Show();
    }

    /// <summary>Sets the title to "YOU WIN" and displays the screen.</summary>
    private void ShowGameWon()
    {
        titleText.text = "YOU WIN";
        Show();
    }

    /// <summary>Updates the final score display and activates the overlay.</summary>
    private void Show()
    {
        finalScoreText.text = $"SCORE: {GameManager.Instance.Score:D4}";
        gameObject.SetActive(true);
    }
}
