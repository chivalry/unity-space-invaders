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

    void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    public void AddScore(int points)
    {
        Score += points;
        OnScoreChanged?.Invoke(Score);
    }

    public void LoseLife()
    {
        Lives--;
        OnLivesChanged?.Invoke(Lives);
        if (Lives <= 0)
            OnGameOver?.Invoke();
    }

    public void GameWon()
    {
        OnGameWon?.Invoke();
    }

    public void GameOver()
    {
        OnGameOver?.Invoke();
    }
}
