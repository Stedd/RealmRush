using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    [SerializeField] List<Waypoint> path = new List<Waypoint>();
    [SerializeField] float moveInterval = 1f;

    void Start()
    {
        StartCoroutine(FollowPath());
    }

    IEnumerator FollowPath()
    {
        foreach (Waypoint waypoint in path)
        {
            Debug.Log(waypoint.name);
            
            transform.localPosition = waypoint.transform.position;
            yield return new WaitForSeconds(moveInterval);
        }
    }
}
