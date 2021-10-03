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
    [SerializeField] int startWealth = 100;
    [Header("Stats")]
    [SerializeField] int currentHealth;
    [SerializeField] int currentWealth;
    public int CurrentWealth {get {return currentWealth;}}

    void Start()
    {
        currentHealth = startHealth;
        currentWealth = startWealth;
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
        currentWealth += _amount;
        UpdateGUI();
        // Debug.Log($"Wealth modification. Change:{_amount}. Current: {wealthAmount}");
    }

    void Reload()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentSceneIndex);
    }

    void UpdateGUI(){
        dispayBalance.text = $"Gold: {currentWealth.ToString()}";
    }

}
