using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverScreen : MonoBehaviour
{
    [SerializeField] private TMP_Text titleText;
    [SerializeField] private TMP_Text finalScoreText;
    [SerializeField] private UnityEngine.UI.Button playAgainButton;
    [SerializeField] private UnityEngine.UI.Button quitButton;

    void Start()
    {
        GameManager.Instance.OnGameOver += ShowGameOver;
        GameManager.Instance.OnGameWon += ShowGameWon;
        playAgainButton.onClick.AddListener(() => SceneManager.LoadScene(0));
        quitButton.onClick.AddListener(() => Application.Quit());
        gameObject.SetActive(false);
    }

    void OnDestroy()
    {
        GameManager.Instance.OnGameOver -= ShowGameOver;
        GameManager.Instance.OnGameWon -= ShowGameWon;
    }

    private void ShowGameOver()
    {
        titleText.text = "GAME OVER";
        Show();
    }

    private void ShowGameWon()
    {
        titleText.text = "YOU WIN";
        Show();
    }

    private void Show()
    {
        finalScoreText.text = $"SCORE: {GameManager.Instance.Score:D4}";
        gameObject.SetActive(true);
    }
}
