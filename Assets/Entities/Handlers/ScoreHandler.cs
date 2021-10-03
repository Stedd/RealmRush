using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreHandler : MonoBehaviour
{
    [Header("Parameters")]
    [SerializeField] int maxHealth = 5;
    [SerializeField] int wealthStartAmount = 100;
    [Header("Stats")]
    [SerializeField] int currentHealth;
    [SerializeField] int wealthAmount;

    void Start()
    {
        currentHealth = maxHealth;
        wealthAmount = wealthStartAmount;
    }

    public void EnemyReachedGoal(GameObject enemy)
    {
        currentHealth -= 1;

        //UpdateHealthText(health);

        if(currentHealth <= 0)
        {
            Debug.Log("You lost");
        }
    }

    public void ModifyWealth(int _amount){
        wealthAmount += _amount;
        // Debug.Log($"Wealth modification. Change:{_amount}. Current: {wealthAmount}");
    }
}
