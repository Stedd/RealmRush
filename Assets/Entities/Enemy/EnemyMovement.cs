using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    [Header("Parameters")]
    [SerializeField] [Range(0f, 5f)] float speed = 1f;
    [SerializeField] int damage = 1;


    [SerializeField] EnemyHandler enemyHandler;
    [SerializeField] ScoreHandler scoreHandler;
    [SerializeField] PathFinder pathFinder;
    [SerializeField] List<Node> path;

    Vector3 startPosition;
    Vector3 endPosition;
    float travelPercent = 0f;

    private IEnumerator followPath;

    void Awake()
    {
        enemyHandler = FindObjectOfType<EnemyHandler>();
        scoreHandler = FindObjectOfType<ScoreHandler>();
        pathFinder = FindObjectOfType<PathFinder>();
    }

    void OnEnable()
    {
        transform.localPosition = GetVector3(pathFinder.GetStartPosition());
        SetPath(pathFinder.CalculateNewPath(enemyHandler.GetCoordinatesFromPosition(gameObject.transform.position)));
        transform.LookAt(GetVector3(path[1].coordinates));
        enemyHandler.AddEnemyToAllEnemies(gameObject);

        CoroutineStarter();
    }


    void RecalculatePath()
    {
        if (followPath != null)
        {
            //Debug.Log("Stopping Coroutine");
            StopCoroutine(followPath);

        }
        //Debug.Log($"{this.name} Recalculating path");
        SetPath(pathFinder.CalculateNewPath(enemyHandler.GetCoordinatesFromPosition(gameObject.transform.position)));
        CoroutineStarter();
    }

    private void CoroutineStarter()
    {
        followPath = FollowPath();
        StartCoroutine(followPath);
    }
    public void SetPath(List<Node> _path)
    {
        path.Clear();

        foreach (Node _node in _path)
        {
            path.Add(_node);
        }
        //regenerate start to finish path to not interfere with building
        pathFinder.CalculateNewPath();
    }

    IEnumerator FollowPath()
    {

        for (int i = 0; i < path.Count; i++)
        {
            startPosition = transform.position;
            endPosition = GetVector3(path[i].coordinates);
            travelPercent = 0;
            transform.LookAt(endPosition);
            float distance = Vector3.Distance(startPosition, endPosition);
            if (Vector3.Distance(startPosition, endPosition) < 10)
            {
                travelPercent = 1 - (distance / 10);
            }

            // Debug.Log($"start: {startPosition}. end: {endPosition}");
            while (travelPercent < 1f)
            {
                travelPercent += Time.deltaTime * speed;
                transform.position = Vector3.Lerp(startPosition, endPosition, travelPercent);
                yield return new WaitForEndOfFrame();
            }
        }

        HandleReachedEndOfPath();
    }

    void HandleReachedEndOfPath()
    {
        scoreHandler.ModifyHealth(-damage);
        scoreHandler.ModifyWealth(-100);
        enemyHandler.RemoveEnemy(gameObject);
        //Destroy(gameObject);
        gameObject.SetActive(false);
    }

    private Vector3 GetVector3(Vector2Int _coord)
    {
        return new Vector3((float)_coord.x, 0f, (float)_coord.y) * 10f;
    }

}
