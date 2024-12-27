using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static GameManager;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private List<GameObject> _enemies;
    [SerializeField] private List<Vector3> _moveTarget;

    public GameObject CurrentEnemy => _currentEnemy;
    private GameObject _currentEnemy;

    private int allEnemyCount;
    public int enemyCount = 0;

    private void Awake()
    {
        allEnemyCount = _enemies.Count;
    }

    void Start()
    {
        SpawnEnemy();
    }

    private void SpawnEnemy()
    {
        _currentEnemy = Instantiate(_enemies[enemyCount], transform.position, Quaternion.identity);

        Enemy enemy = _currentEnemy.GetComponentInChildren<Enemy>();

        if(enemy != null)
        {
            enemy.OnEnemyDied += HandleEnemyDeath;
        }
        Debug.Log(enemy.Type);
        GameManager instance = GameManager.Instance;
        if (enemy.Type == Enemy.EnemyType.Normal)
        {
            Debug.Log("작동1");
            StartCoroutine(instance.ChangingViewState(GameManager.GameState.OrthoMode));
        }
        else
        {
            Debug.Log("작동2");
            StartCoroutine(instance.ChangingViewState(GameManager.GameState.PerspMode));
        }
        StartCoroutine(StartMoveEnemy(_moveTarget[enemyCount]));
    }

    private void HandleEnemyDeath(Enemy enemy)
    {
        enemy.OnEnemyDied -= HandleEnemyDeath;
        enemyCount++;

        StartCoroutine(DeadMoveEnemy(transform.position));

        if (enemyCount < allEnemyCount)
        {
            Invoke("SpawnEnemy", 4f);
        }
        else
        {
            GameManager.Instance.MoveClearScene();
        }
    }

    private IEnumerator StartMoveEnemy(Vector3 target)
    {
        // 초기 위치
        Vector3 startPosition = _currentEnemy.transform.position;

        // 이동 시간
        float duration = 3f;
        float elapsedTime = 0f;

        // 3초 동안 이동
        while (elapsedTime < duration)
        {
            // 시간 갱신
            elapsedTime += Time.deltaTime;

            // Lerp를 사용하여 위치 갱신
            _currentEnemy.transform.position = Vector3.Lerp(startPosition, target, elapsedTime / duration);

            // 다음 프레임까지 대기
            yield return null;
        }

        // 정확히 목표 지점에 위치를 설정 (Lerp로 인해 부정확할 수 있음)
        _currentEnemy.transform.position = target;

        Enemy enemy = _currentEnemy.GetComponentInChildren<Enemy>();
        enemy.IsCanAttack = true;
    }

    private IEnumerator DeadMoveEnemy(Vector3 target)
    {
        // 초기 위치
        Vector3 startPosition = _currentEnemy.transform.position;

        // 이동 시간
        float duration = 3f;
        float elapsedTime = 0f;

        // 3초 동안 이동
        while (elapsedTime < duration)
        {
            // 시간 갱신
            elapsedTime += Time.deltaTime;

            // Lerp를 사용하여 위치 갱신
            _currentEnemy.transform.position = Vector3.Lerp(startPosition, target, elapsedTime / duration);

            // 다음 프레임까지 대기
            yield return null;
        }

        Destroy(_currentEnemy);
    }
}
