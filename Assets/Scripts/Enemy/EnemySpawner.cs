using System.Collections;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Pool;

public class EnemySpawner : MonoBehaviour
{
    [Header("Spawn Settings")]
    public int rowCount;
    public int colCount;

    private int row = 0;
    private int col = 0;

    public Vector2 initialSpawnPosition = new(-4.45f, 2.43f);


    [Header("Time")]
    [SerializeField, Range(0, 1)]
    private float spawnIntervalTime;


    [Header("Enemy Object")]
    [SerializeField]
    private Enemy enemy;
    private IObjectPool<Enemy> objectPool;
    public int enemyCount = 0;

    private void Start()
    {
        Assert.IsNotNull(enemy, "Enemy object is null.");

        objectPool = new ObjectPool<Enemy>(CreateEnemy, OnGetFromPool, OnReleaseToPool, OnDestroyPooledObject);
    }

    IEnumerator IESpawnEnemy()
    {
        while (col < colCount)
        {
            while (row < rowCount)
            {
                Enemy spawned = objectPool.Get();
                Transform sprite = spawned.transform.Find("Sprite");
                Vector2 size = sprite.GetComponent<SpriteRenderer>().size / 1.8f;

                spawned.transform.position = initialSpawnPosition + size * new Vector2(row, -col);

                row++;

                yield return new WaitForSeconds(spawnIntervalTime);
            }

            row = 0;
            col++;
        }

        EventBus.Publish<GameStateEvent>(new(GameState.CHOOSE_DIRECTION));
        col = 0;
    }

    private void OnEnable()
    {
        EventBus.Subscribe<GameStateEvent>(OnGameStateChanged);
    }

    private void OnDisable()
    {
        EventBus.Unsubscribe<GameStateEvent>(OnGameStateChanged);
    }

    private void OnGameStateChanged(GameStateEvent stateEvent)
    {
        if (stateEvent.state == GameState.SPAWNING_ENEMIES)
        {
            if (enemyCount <= 0) StartCoroutine(IESpawnEnemy());
            else EventBus.Publish<GameStateEvent>(new(GameState.CHOOSE_DIRECTION));
        }
    }

    /* --- Object Pool Methods --------------------------------------- */

    private Enemy CreateEnemy()
    {
        Enemy instance = Instantiate(enemy);
        instance.objectPool = objectPool;
        instance.transform.parent = transform;

        return instance;
    }

    private void OnGetFromPool(Enemy obj)
    {
        enemyCount++;
        obj.gameObject.SetActive(true);
    }

    private void OnReleaseToPool(Enemy obj)
    {
        enemyCount--;
        obj.gameObject.SetActive(false);

        if (enemyCount <= 0)
        {
            EventBus.Publish<GameStateEvent>(new(GameState.FINISHED));
        }

        if (enemyCount > 0)
        {
            EventBus.Publish<EnemyKilledEvent>(new());
        }
    }

    private void OnDestroyPooledObject(Enemy obj)
    {
        Destroy(obj.gameObject);
    }
}
