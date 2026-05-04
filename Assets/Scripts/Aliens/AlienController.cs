using System;
using UnityEngine;

/// <summary>
/// Controls an individual alien: sprite animation, point value, and death behavior.
/// Fires <see cref="OnDeath"/> before destroying itself so the grid can update its count.
/// </summary>
public class AlienController : MonoBehaviour
{
    /// <summary>Score points awarded to the player when this alien is destroyed.</summary>
    [SerializeField] private int pointValue = 10;

    /// <summary>Sprites cycled during the idle animation.</summary>
    [SerializeField] private Sprite[] animationFrames;

    /// <summary>Seconds between animation frame advances.</summary>
    [SerializeField] private float animationInterval = 0.5f;

    /// <summary>Fired when this alien dies, passing itself as the argument.</summary>
    public event Action<AlienController> OnDeath;

    private SpriteRenderer _spriteRenderer;
    private int _frameIndex;

    /// <summary>Score points awarded to the player when this alien is destroyed.</summary>
    public int PointValue => pointValue;

    /// <summary>Caches the SpriteRenderer component.</summary>
    void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    /// <summary>Starts the idle animation coroutine.</summary>
    void Start()
    {
        StartCoroutine(Animate());
    }

    /// <summary>Invokes <see cref="OnDeath"/> and destroys this game object.</summary>
    public void Die()
    {
        OnDeath?.Invoke(this);
        Destroy(gameObject);
    }

    /// <summary>
    /// Coroutine that advances the sprite frame every <see cref="animationInterval"/> seconds.
    /// </summary>
    private System.Collections.IEnumerator Animate()
    {
        while (true)
        {
            yield return new WaitForSeconds(animationInterval);
            if (animationFrames.Length > 1)
            {
                _frameIndex = (_frameIndex + 1) % animationFrames.Length;
                _spriteRenderer.sprite = animationFrames[_frameIndex];
            }
        }
    }
}
