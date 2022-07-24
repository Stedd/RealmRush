using System;
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
    [SerializeField] float weaponRange = 40f;
    [SerializeField] int damage = 1;
    [SerializeField] float fireRate = 1;
    [SerializeField] float projectileSpeed = 5;
    [SerializeField] TargetStrategy targetStrategy = TargetStrategy.LowestHealth;

    [Header("BuildParameters")]
    [SerializeField] int cost = 30;
    [SerializeField] float buildTimer = 1;


    [Header("Stats")]
    [SerializeField] float score = 0f;

    #region Privates
    [SerializeField] enum TargetStrategy { ClosestEnemy, LowestHealth };
    private GameObject targetEnemy;
    [SerializeField] private Transform[] buildingParts;
    #endregion

    #region Publics
    public float MaxDistance { get; set; }
    public int Damage { get => damage; set => damage = value; }
    public int FireRate { get; set; }
    public int Cost { get => cost; set => cost = value; }

    #endregion

    private void Awake()
    {
        buildingParts = GetComponentsInChildren<Transform>();
    }

    void Start()
    {
        enemyHandler = FindObjectOfType<EnemyHandler>();
        scoreHandler = FindObjectOfType<ScoreHandler>();
        UpdateWeaponParameters(fireRate, projectileSpeed);

        for (int i = 1; i < buildingParts.Length; i++)
        {
            buildingParts[i].gameObject.SetActive(false);
        }
        StartCoroutine(Build());
    }

    IEnumerator Build()
    {
        for (int i = 1; i < buildingParts.Length; i++)
        {
            buildingParts[i].gameObject.SetActive(true);
            yield return new WaitForSeconds(buildTimer);
        }
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
        FindAndShootEnemy();
    }

    private void FindAndShootEnemy()
    {
        float bestValue = Mathf.Infinity;
        bool targetFound = false;
        List<GameObject> enemies = enemyHandler.ReturnAllEnemies();

        foreach (GameObject enemy in enemies)
        {
            float distanceToTarget = Vector3.Magnitude(enemy.transform.position - transform.position);

            bool withinRange = distanceToTarget < weaponRange;
            if (withinRange)
            {
                if (targetStrategy == TargetStrategy.ClosestEnemy)
                {
                    bool isClosest = distanceToTarget < bestValue;
                    if (isClosest)
                    {
                        targetFound = true;
                        bestValue = distanceToTarget;
                        targetEnemy = enemy;
                    }
                }
                if (targetStrategy == TargetStrategy.LowestHealth)
                {
                    float enemyHealth = enemy.GetComponent<EnemyHealth>().Health;

                    bool isLowestHealth = enemyHealth < bestValue;
                    if (isLowestHealth)
                    {
                        targetFound = true;
                        bestValue = enemyHealth;
                        targetEnemy = enemy;
                    }
                }
            }
        }

        if (targetFound)
        {
            weapon.transform.LookAt(targetEnemy.transform.position);
            ShootProjectile(true);
        }
    }

    void ShootProjectile(bool _state)
    {
        var emissionModule = _projectile.emission;
        emissionModule.enabled = _state;
    }

    public void UpdateScore(float _score)
    {
        score += _score;
    }
}
