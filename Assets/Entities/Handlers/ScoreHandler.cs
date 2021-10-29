using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class ScoreHandler : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] TextMeshProUGUI dispayBalance;

    [Header("Parameters")]
    [SerializeField] int startHealth = 5;
    [SerializeField] int startBalance = 100;
    [Header("Stats")]
    [SerializeField] int currentHealth;
    [SerializeField] int currentBalance;
    public int CurrentBalance {get {return currentBalance;}}

    void Start()
    {
        currentHealth = startHealth;
        currentBalance = startBalance;
        UpdateGUI();
    }

    public void ModifyHealth(GameObject enemy)
    {
        currentHealth -= 1;
        CheckIfYouLost();
    }

    public void ModifyHealth(int _amount)
    {
        currentHealth += _amount;
        CheckIfYouLost();
    }

    void CheckIfYouLost(){
        if(currentHealth <= 0)
        {
            Debug.Log("You lost");
            Reload();
        }
    }

    public void ModifyWealth(int _amount){
        currentBalance += _amount;
        UpdateGUI();
        // Debug.Log($"Wealth modification. Change:{_amount}. Current: {wealthAmount}");
    }

    void Reload()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentSceneIndex);
    }

    void UpdateGUI(){
        dispayBalance.text = $"Gold: {currentBalance.ToString()}";
    }

}
