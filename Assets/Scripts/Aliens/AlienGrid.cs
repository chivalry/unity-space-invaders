using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Spawns and manages the full alien grid. Drives lateral movement that accelerates
/// as aliens die, steps the grid down when it reaches a screen edge, and fires
/// alien bullets at random intervals.
/// </summary>
public class AlienGrid : MonoBehaviour
{
    /// <summary>Prefabs used for alien rows, indexed by type (0 = common, 2 = rare).</summary>
    [SerializeField] private GameObject[] alienPrefabs;

    /// <summary>Number of rows in the grid.</summary>
    [SerializeField] private int rows = 5;

    /// <summary>Number of columns in the grid.</summary>
    [SerializeField] private int cols = 11;

    /// <summary>Horizontal distance between aliens.</summary>
    [SerializeField] private float horizontalSpacing = 1.3f;

    /// <summary>Vertical distance between alien rows.</summary>
    [SerializeField] private float verticalSpacing = 1.0f;

    /// <summary>World X position of the leftmost alien column at spawn.</summary>
    [SerializeField] private float startX = -6.5f;

    /// <summary>World Y position of the top alien row at spawn.</summary>
    [SerializeField] private float startY = 3.0f;

    /// <summary>Units the grid drops downward each time it reverses direction.</summary>
    [SerializeField] private float stepDown = 0.4f;

    /// <summary>Move interval in seconds when all aliens are alive.</summary>
    [SerializeField] private float baseMoveInterval = 0.8f;

    /// <summary>Minimum move interval in seconds (reached when one alien remains).</summary>
    [SerializeField] private float minMoveInterval = 0.05f;

    /// <summary>Units each alien moves horizontally per step.</summary>
    [SerializeField] private float moveDistance = 0.3f;

    /// <summary>Absolute X position at which the grid reverses direction.</summary>
    [SerializeField] private float edgeLimit = 8.5f;

    /// <summary>Y position below which aliens reaching it trigger a game over.</summary>
    [SerializeField] private float alienDeathY = -3.5f;

    /// <summary>Prefab instantiated when an alien fires.</summary>
    [SerializeField] private GameObject alienBulletPrefab;

    /// <summary>Minimum seconds between alien shots.</summary>
    [SerializeField] private float minFireInterval = 0.8f;

    /// <summary>Maximum seconds between alien shots.</summary>
    [SerializeField] private float maxFireInterval = 3.0f;

    private readonly List<AlienController> _livingAliens = new();
    private float _moveInterval;
    private int _direction = 1;

    /// <summary>Spawns aliens, sets the initial move interval, and starts movement and fire coroutines.</summary>
    void Start()
    {
        SpawnAliens();
        _moveInterval = baseMoveInterval;
        StartCoroutine(MoveAliens());
        StartCoroutine(AlienFireLoop());
    }

    /// <summary>Instantiates the full grid of aliens and subscribes to each one's death event.</summary>
    private void SpawnAliens()
    {
        for (int row = 0; row < rows; row++)
        {
            GameObject prefab = PrefabForRow(row);
            for (int col = 0; col < cols; col++)
            {
                Vector3 pos = new(
                    startX + col * horizontalSpacing,
                    startY - row * verticalSpacing,
                    0f);
                GameObject obj = Instantiate(prefab, pos, Quaternion.identity, transform);
                AlienController alien = obj.GetComponent<AlienController>();
                alien.OnDeath += OnAlienDied;
                _livingAliens.Add(alien);
            }
        }
    }

    /// <summary>
    /// Returns the alien prefab assigned to a given row.
    /// Row 0 uses the highest-value prefab; rows 1–2 use the mid-value prefab; remaining rows use the base prefab.
    /// </summary>
    /// <param name="row">Zero-based row index from the top of the grid.</param>
    /// <returns>The <see cref="GameObject"/> prefab for that row.</returns>
    private GameObject PrefabForRow(int row)
    {
        if (row == 0) return alienPrefabs[2];
        if (row <= 2) return alienPrefabs[1];
        return alienPrefabs[0];
    }

    /// <summary>
    /// Called when any alien dies. Removes it from the living list, triggers a win if
    /// the grid is cleared, and recalculates move speed proportional to survivors remaining.
    /// </summary>
    /// <param name="alien">The <see cref="AlienController"/> that just died.</param>
    private void OnAlienDied(AlienController alien)
    {
        _livingAliens.Remove(alien);
        if (_livingAliens.Count == 0)
        {
            GameManager.Instance.GameWon();
            return;
        }
        _moveInterval = Mathf.Lerp(
            minMoveInterval,
            baseMoveInterval,
            _livingAliens.Count / (float)(rows * cols));
    }

    /// <summary>
    /// Coroutine that fires a bullet from a random living alien at a random interval
    /// between <see cref="minFireInterval"/> and <see cref="maxFireInterval"/>.
    /// </summary>
    private IEnumerator AlienFireLoop()
    {
        while (true)
        {
            yield return new WaitForSeconds(
                Random.Range(minFireInterval, maxFireInterval));
            if (_livingAliens.Count == 0) yield break;
            AlienController shooter =
                _livingAliens[Random.Range(0, _livingAliens.Count)];
            Instantiate(
                alienBulletPrefab,
                shooter.transform.position,
                Quaternion.identity);
        }
    }

    /// <summary>
    /// Coroutine that advances the grid one step every <see cref="_moveInterval"/> seconds.
    /// </summary>
    private IEnumerator MoveAliens()
    {
        while (true)
        {
            yield return new WaitForSeconds(_moveInterval);
            MoveStep();
        }
    }

    /// <summary>
    /// Moves all living aliens horizontally by <see cref="moveDistance"/>. If any alien
    /// reaches <see cref="edgeLimit"/>, reverses direction, drops the grid by
    /// <see cref="stepDown"/>, and triggers game over if any alien crosses
    /// <see cref="alienDeathY"/>.
    /// </summary>
    private void MoveStep()
    {
        foreach (AlienController alien in _livingAliens)
            alien.transform.position += new Vector3(moveDistance * _direction, 0f, 0f);

        bool hitEdge = false;
        foreach (AlienController alien in _livingAliens)
        {
            if (alien.transform.position.x > edgeLimit ||
                alien.transform.position.x < -edgeLimit)
            {
                hitEdge = true;
                break;
            }
        }

        if (hitEdge)
        {
            _direction *= -1;
            foreach (AlienController alien in _livingAliens)
                alien.transform.position -= new Vector3(0f, stepDown, 0f);

            foreach (AlienController alien in _livingAliens)
            {
                if (alien.transform.position.y < alienDeathY)
                {
                    GameManager.Instance.GameOver();
                    return;
                }
            }
        }
    }
}
