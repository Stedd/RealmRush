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
    [SerializeField] List<Node> path;

    Vector3 startPosition;
    Vector3 endPosition;
    float travelPercent = 0f;

    private IEnumerator followPath;

    void Awake()
    {
        enemyHandler = FindObjectOfType<EnemyHandler>();
        scoreHandler = FindObjectOfType<ScoreHandler>();
    }

    void OnEnable()
    {
        transform.localPosition = GetVector3(path[0].coordinates);
        transform.LookAt(GetVector3(path[1].coordinates));
        enemyHandler.AddEnemyToAllEnemies(gameObject);


        SetPath(enemyHandler.Path);

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
    }

    IEnumerator FollowPath()
    {
        //if (newPath) { yield break; }

        foreach (Node waypoint in path)
        {
            startPosition = transform.position;
            endPosition = GetVector3(waypoint.coordinates);
            travelPercent = 0;
            transform.LookAt(endPosition);

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
        StopCoroutine(followPath);
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
