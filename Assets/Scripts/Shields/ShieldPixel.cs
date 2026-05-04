using UnityEngine;

/// <summary>
/// Represents a single destructible pixel of a shield. Destroys itself when hit
/// by any bullet, allowing shields to erode progressively.
/// </summary>
public class ShieldPixel : MonoBehaviour
{
    /// <summary>
    /// Destroys this pixel when it is hit by a player or alien bullet.
    /// </summary>
    /// <param name="other">The collider that entered this pixel's trigger.</param>
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("PlayerBullet") || other.CompareTag("AlienBullet"))
            Destroy(gameObject);
    }
}
