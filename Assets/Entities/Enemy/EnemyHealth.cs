using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    [SerializeField] EnemyHandler enemyHandler;
    [SerializeField] float maxHealth = 5f;
    [SerializeField] float currentHealth;
    // Start is called before the first frame update
    void Start()
    {
        enemyHandler = FindObjectOfType<EnemyHandler>();
        currentHealth = maxHealth;
    }

    private void OnParticleCollision(GameObject other)
    {
        ProcessHit(other);
    }

    private void ProcessHit(GameObject other)
    {
        // SpawnFX(damageVFX);

        // Debug.Log(other.GetComponentInParent<Tower>().GetDamage());
        currentHealth -= other.GetComponentInParent<Tower>().GetDamage();

        //UpdateHealthText(health);

        if(currentHealth <= 0)
        {
            ProcessDeath(other);
        }
    }

    private void ProcessDeath(GameObject other)
    {
        other.GetComponentInParent<Tower>().UpdateScore(1f);

        // SpawnFX(deathFX);
        enemyHandler.RemoveEnemy(this.gameObject);
        Destroy(gameObject);
    }
}
