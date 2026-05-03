using UnityEngine;

public class ShieldPixel : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("PlayerBullet") || other.CompareTag("AlienBullet"))
            Destroy(gameObject);
    }
}
