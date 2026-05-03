using System;
using UnityEngine;

public class AlienController : MonoBehaviour
{
    [SerializeField] private int pointValue = 10;
    [SerializeField] private Sprite[] animationFrames;
    [SerializeField] private float animationInterval = 0.5f;

    public event Action<AlienController> OnDeath;

    private SpriteRenderer _spriteRenderer;
    private int _frameIndex;

    void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Start()
    {
        StartCoroutine(Animate());
    }

    public int PointValue => pointValue;

    public void Die()
    {
        OnDeath?.Invoke(this);
        Destroy(gameObject);
    }

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
