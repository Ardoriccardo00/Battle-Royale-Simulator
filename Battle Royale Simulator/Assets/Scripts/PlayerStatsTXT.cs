using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class PlayerStatsTXT : MonoBehaviour
{
    [Header("Stats")]
    [SerializeField] string playerName;
    [SerializeField] int kills = 0;

    [Header("Components")]
    [SerializeField] TextMeshProUGUI playerNameText;
    [SerializeField] TextMeshProUGUI playerHealthText;
    [SerializeField] TextMeshProUGUI playerKillsText;
    Health health;

    private void Start()
    {
        health = GetComponent<Health>();
    }

    void Update()
    {
        playerHealthText.text = "Health: " + Convert.ToString(health.ReturnHealth());
        playerNameText.text = playerName;
        playerKillsText.text = "Kills: " + kills;
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
}
