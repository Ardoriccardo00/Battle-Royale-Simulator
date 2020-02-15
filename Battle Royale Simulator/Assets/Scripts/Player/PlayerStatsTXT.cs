using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class PlayerStatsTXT : MonoBehaviour
{
    [Header("Stats")]
    string playerName;
    int kills = 0;

    [Header("Components")]
    [SerializeField] TextMeshProUGUI playerNameText;
    [SerializeField] TextMeshProUGUI playerKillsText;
    [SerializeField] Slider healthSlider;

    Health health;

    [SerializeField] Gradient healthGradient;
    [SerializeField] Image fill;

    [SerializeField] Weapon weapon;
    [SerializeField] Transform weaponPosition;

    private void Start()
    {
        health = GetComponent<Health>();
        healthSlider.maxValue = health.ReturnMaxHealth();
        fill.color = healthGradient.Evaluate(1f);
    }

    void Update()
    {
        playerNameText.text = playerName;
        playerKillsText.text = "Kills: " + kills;
        healthSlider.value = health.ReturnHealth();
        fill.color = healthGradient.Evaluate(healthSlider.normalizedValue);
    }

    public string ReturnPlayerName()
    {
        return playerName;
    }

    public void SetPlayerName(string name)
    {
        playerName = name;
    }

    public void AddKills()
    {
        kills++;
    }

    public int ReturnKills()
    {
        return kills;
    }

    public void SetWeapon(Weapon newWeapon)
    {
        newWeapon.transform.position = weaponPosition.position;
    }
}
