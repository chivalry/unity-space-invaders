using UnityEngine;

/// <summary>
/// Spawns a UFO at a fixed interval, moves it across the top of the screen,
/// and despawns it if it exits the play field without being shot.
/// Each UFO is assigned a random point value and a random travel direction.
/// </summary>
public class UfoController : MonoBehaviour
{
    /// <summary>Prefab instantiated for each UFO appearance.</summary>
    [SerializeField] private GameObject ufoPrefab;

    /// <summary>Seconds between UFO spawn attempts.</summary>
    [SerializeField] private float spawnInterval = 25f;

    /// <summary>Units per second the UFO travels horizontally.</summary>
    [SerializeField] private float speed = 3f;

    /// <summary>World Y position at which the UFO travels.</summary>
    [SerializeField] private float spawnY = 4.5f;

    /// <summary>Absolute X position at which the UFO is despawned if not destroyed.</summary>
    [SerializeField] private float despawnX = 10f;

    /// <summary>Pool of point values randomly assigned to each UFO on spawn.</summary>
    [SerializeField] private int[] pointValues = { 50, 100, 150, 300 };

    private GameObject _activeUfo;
    private float _direction;

    /// <summary>Starts the repeating spawn timer.</summary>
    void Start()
    {
        InvokeRepeating(nameof(SpawnUfo), spawnInterval, spawnInterval);
    }

    /// <summary>
    /// Moves the active UFO each frame and destroys it when it passes <see cref="despawnX"/>.
    /// </summary>
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

    /// <summary>
    /// Instantiates the UFO prefab at a random side of the screen with a random
    /// point value, then configures its <see cref="UfoHit"/> component.
    /// Does nothing if a UFO is already active.
    /// </summary>
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

    /// <summary>
    /// Called by <see cref="UfoHit"/> when the UFO is destroyed by a player bullet,
    /// so this controller can clear its reference without a null-check delay.
    /// </summary>
    public void OnUfoDestroyed()
    {
        _activeUfo = null;
    }
}
