using System;
using UnityEngine;

/// <summary>
/// Singleton that owns global game state: score, lives, and win/loss conditions.
/// Broadcasts state changes via events so UI and gameplay systems stay decoupled.
/// Also serves as the single audio playback point for all game sounds.
/// </summary>
public class GameManager : MonoBehaviour
{
    /// <summary>The single active instance of <see cref="GameManager"/> in the scene.</summary>
    public static GameManager Instance { get; private set; }

    /// <summary>Current player score.</summary>
    public int Score { get; private set; }

    /// <summary>Remaining player lives.</summary>
    public int Lives { get; private set; } = 3;

    /// <summary>Fired whenever <see cref="Score"/> changes, passing the new value.</summary>
    public event Action<int> OnScoreChanged;

    /// <summary>Fired whenever <see cref="Lives"/> changes, passing the new value.</summary>
    public event Action<int> OnLivesChanged;

    /// <summary>Fired when all lives are lost or the alien grid reaches the player's row.</summary>
    public event Action OnGameOver;

    /// <summary>Fired when all aliens are destroyed.</summary>
    public event Action OnGameWon;

    /// <summary>Sound played when the player fires.</summary>
    [SerializeField] private AudioClip shootClip;

    /// <summary>Sound played when an alien or UFO is destroyed.</summary>
    [SerializeField] private AudioClip alienExplodeClip;

    /// <summary>Sound played when the player loses a life.</summary>
    [SerializeField] private AudioClip playerDieClip;

    /// <summary>Sound played when the game ends in defeat.</summary>
    [SerializeField] private AudioClip gameOverClip;

    /// <summary>Sound played when the player clears all aliens.</summary>
    [SerializeField] private AudioClip gameWonClip;

    private AudioSource _audio;

    /// <summary>
    /// Enforces the singleton pattern and adds an <see cref="AudioSource"/> component
    /// for one-shot sound playback.
    /// </summary>
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

    /// <summary>Plays the player-shoot sound effect.</summary>
    public void PlayShoot() => _audio.PlayOneShot(shootClip);

    /// <summary>Plays the alien/UFO explosion sound effect.</summary>
    public void PlayAlienExplode() => _audio.PlayOneShot(alienExplodeClip);

    /// <summary>
    /// Adds <paramref name="points"/> to <see cref="Score"/> and fires <see cref="OnScoreChanged"/>.
    /// </summary>
    /// <param name="points">Points to add.</param>
    public void AddScore(int points)
    {
        Score += points;
        OnScoreChanged?.Invoke(Score);
    }

    /// <summary>
    /// Decrements <see cref="Lives"/>, plays the death sound, fires <see cref="OnLivesChanged"/>,
    /// and triggers <see cref="GameOver"/> if no lives remain.
    /// </summary>
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

    /// <summary>Plays the win sound and fires <see cref="OnGameWon"/>.</summary>
    public void GameWon()
    {
        _audio.PlayOneShot(gameWonClip);
        OnGameWon?.Invoke();
    }

    /// <summary>Plays the game-over sound and fires <see cref="OnGameOver"/>.</summary>
    public void GameOver()
    {
        _audio.PlayOneShot(gameOverClip);
        OnGameOver?.Invoke();
    }
}
