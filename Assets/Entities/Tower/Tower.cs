using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tower : MonoBehaviour
{

    [Header("Assigned on start")]
    [SerializeField] EnemyHandler enemyHandler;
    [SerializeField] ScoreHandler scoreHandler;

    [Header("Prefabs")]
    [SerializeField] ParticleSystem _projectile;
    [SerializeField] Transform weapon;

    [Header("WeaponParameters")]
    [SerializeField] float maxDistance = 40f;
    [SerializeField] int damage = 1;
    [SerializeField] float fireRate = 1;
    [SerializeField] float projectileSpeed = 5;

    [Header("BuildParameters")]
    [SerializeField] int cost = 30;


    [Header("Stats")]
    [SerializeField] float score = 0f;

    #region Privates
    private GameObject closestEnemy;
    #endregion

    #region Publics
    public float MaxDistance { get; set; }
    public int Damage { get => damage; set => damage = value; }
    public int FireRate { get; set; }
    public int Cost { get => cost; set => cost = value; }

    #endregion

    void Start()
    {
        enemyHandler = FindObjectOfType<EnemyHandler>();
        scoreHandler = FindObjectOfType<ScoreHandler>();
        UpdateWeaponParameters(fireRate, projectileSpeed);
    }

    private void UpdateWeaponParameters(float _fireRate, float _projectileSpeed)
    {
        var main = _projectile.main;
        main.startSpeed = _projectileSpeed;

        var emission = _projectile.emission;
        emission.rateOverTime = _fireRate;
    }

    // Update is called once per frame
    void Update()
    {
        ShootProjectile(false);
        FindAndShootClosestEnemy();
    }

    private void FindAndShootClosestEnemy()
    {
        // enemies = enemyHandler.ReturnAllEnemies();
        float lowestDist = Mathf.Infinity;
        bool targetFound = false;
        foreach (GameObject enemy in enemyHandler.ReturnAllEnemies())
        {
            float distanceToCurrentTarget = Vector3.Magnitude(enemy.transform.position - transform.position);
            if (distanceToCurrentTarget < lowestDist && distanceToCurrentTarget < maxDistance)
            {
                targetFound = true;
                lowestDist = distanceToCurrentTarget;
                closestEnemy = enemy;
            }
        }
        if (targetFound)
        {
            weapon.transform.LookAt(closestEnemy.transform.position);
            ShootProjectile(true);
        }
    }

    void ShootProjectile(bool _state)
    {
        var emissionModule = _projectile.emission;
        //emissionModule.rateOverTime = fireRate;
        emissionModule.enabled = _state;
    }

    public void UpdateScore(float _score)
    {
        score += _score;
    }
}
