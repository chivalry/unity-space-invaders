using UnityEngine;

public class UfoController : MonoBehaviour
{
    [SerializeField] private GameObject ufoPrefab;
    [SerializeField] private float spawnInterval = 25f;
    [SerializeField] private float speed = 3f;
    [SerializeField] private float spawnY = 4.5f;
    [SerializeField] private float despawnX = 10f;
    [SerializeField] private int[] pointValues = { 50, 100, 150, 300 };

    private GameObject _activeUfo;
    private float _direction;

    void Start()
    {
        InvokeRepeating(nameof(SpawnUfo), spawnInterval, spawnInterval);
    }

    void Update()
    {
        if (_activeUfo == null) return;

        _activeUfo.transform.position +=
            new Vector3(speed * _direction * Time.deltaTime, 0f, 0f);

        if (Mathf.Abs(_activeUfo.transform.position.x) > despawnX)
        {
            Destroy(_activeUfo);
            _activeUfo = null;
        }
    }

    private void SpawnUfo()
    {
        if (_activeUfo != null) return;

        _direction = Random.value > 0.5f ? 1f : -1f;
        float spawnX = -_direction * despawnX;
        _activeUfo = Instantiate(
            ufoPrefab,
            new Vector3(spawnX, spawnY, 0f),
            Quaternion.identity);

        UfoHit hit = _activeUfo.GetComponent<UfoHit>();
        hit.Setup(pointValues[Random.Range(0, pointValues.Length)], this);
    }

    public void OnUfoDestroyed()
    {
        _activeUfo = null;
    }
}
