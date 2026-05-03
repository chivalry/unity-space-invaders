using System;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public int Score { get; private set; }
    public int Lives { get; private set; } = 3;

    public event Action<int> OnScoreChanged;
    public event Action<int> OnLivesChanged;
    public event Action OnGameOver;
    public event Action OnGameWon;

    [SerializeField] private AudioClip shootClip;
    [SerializeField] private AudioClip alienExplodeClip;
    [SerializeField] private AudioClip playerDieClip;
    [SerializeField] private AudioClip gameOverClip;
    [SerializeField] private AudioClip gameWonClip;

    private AudioSource _audio;

    void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        _audio = gameObject.AddComponent<AudioSource>();
    }

    public void PlayShoot() => _audio.PlayOneShot(shootClip);
    public void PlayAlienExplode() => _audio.PlayOneShot(alienExplodeClip);

    public void AddScore(int points)
    {
        Score += points;
        OnScoreChanged?.Invoke(Score);
    }

    public void LoseLife()
    {
        Lives--;
        OnLivesChanged?.Invoke(Lives);
        _audio.PlayOneShot(playerDieClip);
        if (Lives <= 0)
        {
            _audio.PlayOneShot(gameOverClip);
            OnGameOver?.Invoke();
        }
    }

    public void GameWon()
    {
        _audio.PlayOneShot(gameWonClip);
        OnGameWon?.Invoke();
    }

    public void GameOver()
    {
        _audio.PlayOneShot(gameOverClip);
        OnGameOver?.Invoke();
    }
}
