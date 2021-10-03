using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tower : MonoBehaviour
{

    [Header("Assigned on start")] 
    [SerializeField] EnemyHandler enemyHandler;
    [SerializeField] ScoreHandler scoreHandler;

    [Header("Prefabs")]
    [SerializeField] ParticleSystem projectile;
    [SerializeField] Transform weapon;


    [Header("Parameters")]
    [SerializeField]float maxDistance = 40f;
    [SerializeField]int damage = 1 ;
    [SerializeField]int cost = 30 ;
    public int Cost { get => cost; set => cost = value; }

    [Header("Stats")] 
    [SerializeField]float score = 0f;

    GameObject closestEnemy;

    void Start()
    {
        enemyHandler = FindObjectOfType<EnemyHandler>();
        scoreHandler = FindObjectOfType<ScoreHandler>();

        //scoreHandler.ModifyWealth(-cost);
    }

    // Update is called once per frame
    void Update()
    {
        ShootProjectile(false);
        // enemies = enemyHandler.ReturnAllEnemies();
        float lowestDist = Mathf.Infinity;
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

    public void UpdateScore(float _score){
        score += _score;
    }
    public int GetDamage(){
        return damage;
    }
}
