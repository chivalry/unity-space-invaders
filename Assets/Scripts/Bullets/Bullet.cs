using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private float speed = 12f;
    [SerializeField] private bool isPlayerBullet = true;

    void Update()
    {
        Vector3 direction = isPlayerBullet ? Vector3.up : Vector3.down;
        transform.Translate(direction * speed * Time.deltaTime);

        if (transform.position.y > 5.5f || transform.position.y < -5.5f)
            Destroy(gameObject);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        // Collision handling added in Phase 4
    }
}
