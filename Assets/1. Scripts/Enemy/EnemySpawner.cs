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
            Debug.Log("�۵�1");
            StartCoroutine(instance.ChangingViewState(GameManager.GameState.OrthoMode));
        }
        else
        {
            Debug.Log("�۵�2");
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
        // �ʱ� ��ġ
        Vector3 startPosition = _currentEnemy.transform.position;

        // �̵� �ð�
        float duration = 3f;
        float elapsedTime = 0f;

        // 3�� ���� �̵�
        while (elapsedTime < duration)
        {
            // �ð� ����
            elapsedTime += Time.deltaTime;

            // Lerp�� ����Ͽ� ��ġ ����
            _currentEnemy.transform.position = Vector3.Lerp(startPosition, target, elapsedTime / duration);

            // ���� �����ӱ��� ���
            yield return null;
        }

        // ��Ȯ�� ��ǥ ������ ��ġ�� ���� (Lerp�� ���� ����Ȯ�� �� ����)
        _currentEnemy.transform.position = target;

        Enemy enemy = _currentEnemy.GetComponentInChildren<Enemy>();
        enemy.IsCanAttack = true;
    }

    private IEnumerator DeadMoveEnemy(Vector3 target)
    {
        // �ʱ� ��ġ
        Vector3 startPosition = _currentEnemy.transform.position;

        // �̵� �ð�
        float duration = 3f;
        float elapsedTime = 0f;

        // 3�� ���� �̵�
        while (elapsedTime < duration)
        {
            // �ð� ����
            elapsedTime += Time.deltaTime;

            // Lerp�� ����Ͽ� ��ġ ����
            _currentEnemy.transform.position = Vector3.Lerp(startPosition, target, elapsedTime / duration);

            // ���� �����ӱ��� ���
            yield return null;
        }

        Destroy(_currentEnemy);
    }
}
