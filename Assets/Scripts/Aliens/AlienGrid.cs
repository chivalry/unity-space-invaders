using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlienGrid : MonoBehaviour
{
    [SerializeField] private GameObject[] alienPrefabs;
    [SerializeField] private int rows = 5;
    [SerializeField] private int cols = 11;
    [SerializeField] private float horizontalSpacing = 1.3f;
    [SerializeField] private float verticalSpacing = 1.0f;
    [SerializeField] private float startX = -6.5f;
    [SerializeField] private float startY = 3.0f;
    [SerializeField] private float stepDown = 0.4f;
    [SerializeField] private float baseMoveInterval = 0.8f;
    [SerializeField] private float minMoveInterval = 0.05f;
    [SerializeField] private float moveDistance = 0.3f;
    [SerializeField] private float edgeLimit = 8.5f;
    [SerializeField] private float alienDeathY = -3.5f;
    [SerializeField] private GameObject alienBulletPrefab;
    [SerializeField] private float minFireInterval = 0.8f;
    [SerializeField] private float maxFireInterval = 3.0f;

    private readonly List<AlienController> _livingAliens = new();
    private float _moveInterval;
    private int _direction = 1;

    void Start()
    {
        SpawnAliens();
        _moveInterval = baseMoveInterval;
        StartCoroutine(MoveAliens());
        StartCoroutine(AlienFireLoop());
    }

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

    private GameObject PrefabForRow(int row)
    {
        if (row == 0) return alienPrefabs[2];
        if (row <= 2) return alienPrefabs[1];
        return alienPrefabs[0];
    }

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

    private IEnumerator MoveAliens()
    {
        while (true)
        {
            yield return new WaitForSeconds(_moveInterval);
            MoveStep();
        }
    }

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
