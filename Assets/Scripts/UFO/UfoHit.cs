using UnityEngine;

/// <summary>
/// Attached to a UFO instance to handle player bullet collisions.
/// Awards score and notifies <see cref="UfoController"/> when the UFO is destroyed.
/// </summary>
public class UfoHit : MonoBehaviour
{
    private int _pointValue;
    private UfoController _spawner;

    /// <summary>
    /// Initializes this UFO's point reward and back-reference to its spawner.
    /// Must be called immediately after instantiation.
    /// </summary>
    /// <param name="pointValue">Score points awarded when this UFO is destroyed.</param>
    /// <param name="spawner">The <see cref="UfoController"/> that spawned this UFO.</param>
    public void Setup(int pointValue, UfoController spawner)
    {
        _pointValue = pointValue;
        _spawner = spawner;
    }

    /// <summary>
    /// Awards <see cref="_pointValue"/> to the player, notifies the spawner, and
    /// destroys both the bullet and this UFO when hit by a player bullet.
    /// </summary>
    /// <param name="other">The collider that entered this trigger.</param>
    void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("PlayerBullet")) return;

        GameManager.Instance.AddScore(_pointValue);
        GameManager.Instance.PlayAlienExplode();
        _spawner.OnUfoDestroyed();
        Destroy(other.gameObject);
        Destroy(gameObject);
    }
}
