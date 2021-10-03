using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    [Header("Assigned on start")] 
    [SerializeField] EnemyHandler enemyHandler;
    [SerializeField] ScoreHandler scoreHandler;

    [Header("Parameters")]
    [SerializeField] int maxHealth = 5;
    [SerializeField] int difficultyRamp = 1;

    [SerializeField] int wealthValue = 5;

    [Header("Stats")] 
    [SerializeField] int currentHealth;
    // Start is called before the first frame update
    void OnEnable()
    {
        enemyHandler = FindObjectOfType<EnemyHandler>();
        scoreHandler = FindObjectOfType<ScoreHandler>();
        currentHealth = maxHealth;
    }

    private void OnParticleCollision(GameObject damager)
    {
        ProcessHitFrom(damager);
    }

    private void ProcessHitFrom(GameObject damager)
    {
        // SpawnFX(damageVFX);

        // Debug.Log(damager.GetComponentInParent<Tower>().GetDamage());
        currentHealth -= damager.GetComponentInParent<Tower>().GetDamage();

        //UpdateHealthText(health);

        if(currentHealth <= 0)
        {
            ProcessDeathFrom(damager);
        }
    }

    private void ProcessDeathFrom(GameObject damager)
    {
        damager.GetComponentInParent<Tower>().UpdateScore(1f);

        // SpawnFX(deathFX);
        scoreHandler.ModifyWealth(wealthValue);
        enemyHandler.RemoveEnemy(gameObject);
        // Destroy(gameObject);
        gameObject.SetActive(false);
        maxHealth += difficultyRamp;
    }
}
