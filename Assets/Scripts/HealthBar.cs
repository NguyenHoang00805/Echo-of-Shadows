using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public Slider healthSlider;
    public TMP_Text healthBarText;

    Damageable playerDamageable;

    private void Awake()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player == null)
        {
            Debug.Log("no player found, make sure player has the right tag");
        }
        playerDamageable = player.GetComponent<Damageable>();        
    }

    // Start is called before the first frame update
    void Start()
    {
        healthSlider.value = calculateSliderPer(playerDamageable.Health, playerDamageable.MaxHealth);
        healthBarText.text = "HP " + playerDamageable.Health + "/" + playerDamageable.MaxHealth;
    }

    private float calculateSliderPer( float currentHealth, float maxHealth)
    {
        return currentHealth / maxHealth;
    }

    private void OnEnable()
    {
        playerDamageable.healthChanged.AddListener(OnPlayerHealthChanged);
    }

    private void OnDisable()
    {
        playerDamageable.healthChanged.RemoveListener(OnPlayerHealthChanged);
    }

    private void OnPlayerHealthChanged(int newH, int maxH)
    {
        healthSlider.value = calculateSliderPer(newH, maxH);
        healthBarText.text = "HP " + newH + "/" + maxH;
    }
}
