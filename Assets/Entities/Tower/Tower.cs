using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tower : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] List<EnemyMovement> enemies;

    void Start()
    {
        enemies.Add(FindObjectOfType<EnemyMovement>());
    }

    // Update is called once per frame
    void Update()
    {
        transform.LookAt(enemies[0].transform.position);
    }
        
}
