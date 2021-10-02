using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    [SerializeField] EnemyHandler enemyHandler;
    [SerializeField] ScoreHandler scoreHandler;
    [SerializeField] List<Tile> path;
    [SerializeField] [Range(0f, 5f)]float speed = 1f;

    Vector3 startPosition;
    Vector3 endPosition;
    float travelPercent = 0f;

    void Start()
    {
        enemyHandler = FindObjectOfType<EnemyHandler>();
        enemyHandler.AddEnemyToAllEnemies(gameObject);
        
        scoreHandler = FindObjectOfType<ScoreHandler>();
        StartCoroutine(FollowPath());
    }

    public void SetPath(List<Tile> _path){
        path = _path ;
    }

    IEnumerator FollowPath()
    {
        foreach (Tile waypoint in path)
        {
            startPosition   = transform.position;
            endPosition     = waypoint.transform.position;
            travelPercent   = 0;
            transform.LookAt(endPosition);

            // Debug.Log($"start: {startPosition}. end: {endPosition}");
            while(travelPercent <1f)
            {
                travelPercent += Time.deltaTime*speed;
                transform.position = Vector3.Lerp(startPosition, endPosition, travelPercent);
                yield return new WaitForEndOfFrame();
            }
        }

        HandleReachedEndOfPath();
    }

    void HandleReachedEndOfPath()
    {
        scoreHandler.EnemyReachedGoal(gameObject);
        enemyHandler.RemoveEnemy(gameObject);
        Destroy(gameObject);
    }

}
