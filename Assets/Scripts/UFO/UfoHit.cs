using UnityEngine;

public class UfoHit : MonoBehaviour
{
    private int _pointValue;
    private UfoController _spawner;

    public void Setup(int pointValue, UfoController spawner)
    {
        _pointValue = pointValue;
        _spawner = spawner;
    }

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
