using UnityEngine;

/// <summary>
/// Moves a bullet vertically each frame and handles collision with aliens or the player.
/// Direction is determined by <see cref="isPlayerBullet"/>: player bullets travel up,
/// alien bullets travel down.
/// </summary>
public class Bullet : MonoBehaviour
{
    /// <summary>Units per second the bullet travels.</summary>
    [SerializeField] private float speed = 12f;

    /// <summary>
    /// When true the bullet travels upward and damages aliens;
    /// when false it travels downward and damages the player.
    /// </summary>
    [SerializeField] private bool isPlayerBullet = true;

    /// <summary>
    /// Moves the bullet along its travel direction and destroys it if it leaves the play field.
    /// </summary>
    void Update()
    {
        Vector3 direction = isPlayerBullet ? Vector3.up : Vector3.down;
        transform.Translate(direction * speed * Time.deltaTime);

        if (transform.position.y > 5.5f || transform.position.y < -5.5f)
            Destroy(gameObject);
    }

    /// <summary>
    /// Handles collision events. Player bullets destroy aliens and award score;
    /// alien bullets call <see cref="GameManager.LoseLife"/>.
    /// </summary>
    /// <param name="other">The collider this bullet entered.</param>
    void OnTriggerEnter2D(Collider2D other)
    {
        if (isPlayerBullet && other.CompareTag("Alien"))
        {
            AlienController alien = other.GetComponent<AlienController>();
            GameManager.Instance.AddScore(alien.PointValue);
            GameManager.Instance.PlayAlienExplode();
            alien.Die();
            Destroy(gameObject);
        }
        else if (!isPlayerBullet && other.CompareTag("Player"))
        {
            GameManager.Instance.LoseLife();
            Destroy(gameObject);
        }
    }
}
