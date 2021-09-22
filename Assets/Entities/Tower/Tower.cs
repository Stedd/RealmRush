using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tower : MonoBehaviour
{
    [SerializeField] ParticleSystem projectile;
    [SerializeField] Transform weapon;
    [SerializeField] EnemyHandler enemyHandler;
    // [SerializeField] List<GameObject> enemies;

    [SerializeField]float maxDistance = 40f;

    GameObject closestEnemy;

    void Start()
    {
        enemyHandler = FindObjectOfType<EnemyHandler>();
    }

    // Update is called once per frame
    void Update()
    {
        ShootProjectile(false);
        // enemies = enemyHandler.ReturnAllEnemies();
        float lowestDist = 10000;
        bool targetFound = false;
        foreach (GameObject enemy in enemyHandler.ReturnAllEnemies())
        {
            float distanceToCurrentTarget = Vector3.Magnitude(enemy.transform.position-transform.position);
            if (distanceToCurrentTarget<lowestDist && distanceToCurrentTarget<maxDistance)
            {
                targetFound = true;
                lowestDist = distanceToCurrentTarget;
                closestEnemy = enemy;
            }
        }
        if(targetFound)
        {
            weapon.transform.LookAt(closestEnemy.transform.position);
            ShootProjectile(true);
        }
    }
    void ShootProjectile(bool _state)
    {
        var emissionModule = projectile.emission;
        emissionModule.enabled = _state;
    }
}
