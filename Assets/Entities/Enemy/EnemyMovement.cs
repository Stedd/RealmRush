using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    [SerializeField] EnemyHandler enemyHandler;
    [SerializeField] List<Tile> path = new List<Tile>();
    [SerializeField] [Range(0f, 5f)]float speed = 1f;

    Vector3 startPosition;
    Vector3 endPosition;
    float travelPercent = 0f;

    void Start()
    {
        enemyHandler = FindObjectOfType<EnemyHandler>();
        enemyHandler.AddEnemyToAllEnemies(this.gameObject);
        StartCoroutine(FollowPath());
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
    }
}
