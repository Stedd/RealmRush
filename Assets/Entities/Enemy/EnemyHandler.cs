using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHandler : MonoBehaviour
{
    [Header("Assigned on start")]
    [SerializeField] PathFinder pathFinder;
    [SerializeField] GridManager gridManager;


    [Header("Parameters")]
    [SerializeField] [Range(0.1f, 60f)] float spawnRate = 60f;
    [SerializeField] int objectPoolSize = 15;

    [Header("Prefabs")]
    [SerializeField] GameObject objectPool;
    [SerializeField] List<GameObject> enemyPrefabs = new List<GameObject>();

    [Header("Lists")]
    [SerializeField] List<Node> path = new List<Node>();
    [SerializeField] List<GameObject> enemyPools = new List<GameObject>();
    [SerializeField] List<GameObject> allEnemies = new List<GameObject>();


    public List<Node> Path { get { return path; } }

    private void Awake()
    {
        gridManager = FindObjectOfType<GridManager>();
    }

    private void Start()
    {
        //gridManager.CalculateNewPath();

        PopulateObjectPools();

        StartCoroutine(Spawner());
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.N))
        {
            SpawnNewEnemy();
        }
    }

    public void SetPath(List<Node> _nodes)
    {
        path.Clear();
        
        foreach (Node _node in _nodes)
        {
            path.Add(_node);
        }

        UpdateEnemyPath();
    }    

    void UpdateEnemyPath()
    {
        foreach (GameObject _pool in enemyPools)
        {
            foreach (EnemyMovement _enemy in _pool.GetComponentsInChildren<EnemyMovement>())
            {
                _enemy.SetPath(path);
            }
        }
    }    

    void PopulateObjectPools()
    {
        foreach (GameObject enemy in enemyPrefabs)
        {
            GameObject newPool = Instantiate(objectPool, transform);
            newPool.transform.name = $"ObjectPool:{enemy.name}";
            ObjectPool poolScript = newPool.GetComponent<ObjectPool>();
            enemyPools.Add(newPool);

            for (int i = 0; i < objectPoolSize; i++)
            {
                enemy.SetActive(false);
                poolScript.AddObject(enemy);
            }
        }
    }

    public void AddEnemyToAllEnemies(GameObject _enemy)
    {
        allEnemies.Add(_enemy);
    }

    public void RemoveEnemy(GameObject _enemy)
    {
        allEnemies.Remove(_enemy);
    }

    public List<GameObject> ReturnAllEnemies()
    {
        return allEnemies;
    }

    void SpawnNewEnemy()
    {
        int spawnEnemyIndex = Mathf.RoundToInt(Random.Range(-0.49f, 2.49f));
        enemyPools[spawnEnemyIndex].GetComponent<ObjectPool>().EnableFirstAvailableObject();
    }

    IEnumerator Spawner()
    {
        while (true)
        {
            SpawnNewEnemy();
            yield return new WaitForSeconds(spawnRate);
        }
    }

    public Vector2Int GetCoordinatesFromPosition(Vector3 position)
    {
        Vector2Int coordinates = new Vector2Int();
        coordinates.x = Mathf.RoundToInt(position.x / UnityEditor.EditorSnapSettings.move.x);
        coordinates.y = Mathf.RoundToInt(position.z / UnityEditor.EditorSnapSettings.move.z);

        return coordinates;
    }

    public void NotifyEnemiesOfNewPath()
    {
        BroadcastMessage("RecalculatePath", SendMessageOptions.DontRequireReceiver);
    }
}
