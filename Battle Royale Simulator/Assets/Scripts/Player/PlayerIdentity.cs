using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

[ExecuteInEditMode]
public class PlayerIdentity : MonoBehaviour
{
    [Header("Stats")]
    [SerializeField] string playerName;
    [SerializeField] Sprite playerAvatar;
    [SerializeField] int kills = 0;

    [Header("Components")]
    [SerializeField] TextMeshProUGUI playerNameText;
    [SerializeField] TextMeshProUGUI playerHealthText;
    [SerializeField] TextMeshProUGUI playerKillsText;
    [SerializeField] Image playerImage;

    public PlayerProfile profile;

    void Start()
    {
        /*playerNameText.text = "";
        playerImage.sprite = playerAvatar;
        playerHealthText.text = "";
        playerKillsText.text = "";*/

        playerName = profile.playerName;
        playerAvatar = profile.playerAvatar;
        GetComponent<MeshRenderer>().material = profile.playerColor;
    }

    void Update()
    {
        playerHealthText.text = "Health: " + Convert.ToString(GetComponent<Health>().ReturnHealth());
        playerNameText.text = playerName;
        playerKillsText.text = "Kills: " + kills;
        playerImage.sprite = playerAvatar;
    }

    public string ReturnPlayerName()
    {
        return playerName;
    }

    public void SetPlayerName(string name)
    {
        playerName = name;
    }

    public void SetPlayerAvatar(Sprite avatar)
    {
        playerAvatar = avatar;
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
