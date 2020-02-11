using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MatchManager : MonoBehaviour
{
    #region Singleton
    public static MatchManager Instance { get; private set; }
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            //dont destroy on load if needed
        }
        else
        {
            Destroy(gameObject);
        }
    }
    #endregion

    [SerializeField] GameObject playerPrefab;
    [SerializeField] Vector3 worldSize;
    [SerializeField] TextMeshProUGUI winnerText;
    [SerializeField] PlayerProfile[] profiles;
    [SerializeField] TextMeshProUGUI playersAliveText;
    [SerializeField] Button[] scoreBoardsButtons;
    bool firstBlood = false;
    bool matchHasStarted = false;
    
    List<PlayerIdentity> playersList = new List<PlayerIdentity>();

    void Start()
    {
        winnerText.text = "";
    }

    public void SpawnWithProfiles()
    {
        for (int i = 0; i < profiles.Length; i++)
        {
            float x, y, z;
            x = UnityEngine.Random.Range(-worldSize.x / 2, worldSize.x / 2);
            y = UnityEngine.Random.Range(-worldSize.y / 2, worldSize.y / 2);
            z = UnityEngine.Random.Range(-worldSize.z / 2, worldSize.z / 2);

            Vector3 spawnPosition = new Vector3(x, 1, z);

            PlayerIdentity playerToInstantiate = Instantiate(playerPrefab, spawnPosition, playerPrefab.transform.rotation).GetComponent<PlayerIdentity>();
            playerToInstantiate.profile = profiles[i];
            playersList.Add(playerToInstantiate);
        }
    }

    private void Update()
    {
        if (matchHasStarted)
        {
            CheckForPlayers();
            playersList.Sort((x, y) => y.ReturnKills().CompareTo(x.ReturnKills()));
            UpdateScoreboard();
            if (firstBlood == false) CheckFirstBlood();
        }       
    }

    void CheckForPlayers()
    {
        PlayerIdentity[] playersArray = FindObjectsOfType<PlayerIdentity>();
        int playersAlive = playersArray.Length;
        playersAliveText.text = "Remaining Players: " + Convert.ToString(playersAlive);

        if (playersArray.Length == 1)
        {
            winnerText.text = (playersArray[0].ReturnPlayerName() + " Has won!");
        }      
    }

    void CheckFirstBlood()
    {
        if(playersList.Count < profiles.Length)
        {
            print("first blood");
            firstBlood = true;
        }
    }

    void UpdateScoreboard()
    {
        for(int i = 0; i< scoreBoardsButtons.Length; i++)
        {
            try
            {
                TextMeshProUGUI buttonText = scoreBoardsButtons[i].GetComponentInChildren<TextMeshProUGUI>();

                buttonText.text = playersList[i].ReturnPlayerName()
                    + " "
                    + playersList[i].ReturnKills();
            }
            catch { }         
        }
    }

    public void RemovePlayer(PlayerIdentity player)
    {
        playersList.Remove(player);
    }

    public void SetMatchHasStarted(bool value)
    {
        matchHasStarted = value;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawCube(new Vector3(0,0,0), worldSize); 
    }
}

/*public void SpawnWithProfiles()
    {
        foreach (PlayerProfile profile in profiles)
        {
            float x, y, z;
            x = UnityEngine.Random.Range(-worldSize.x / 2, worldSize.x / 2);
            y = UnityEngine.Random.Range(-worldSize.y / 2, worldSize.y / 2);
            z = UnityEngine.Random.Range(-worldSize.z / 2, worldSize.z / 2);

            Vector3 spawnPosition = new Vector3(x, 1, z);

            PlayerIdentity prefabIdentity = playerPrefab.GetComponent<PlayerIdentity>();
            prefabIdentity.SetPlayerName(profile.name);
            prefabIdentity.SetPlayerAvatar(profile.playerAvatar);
            prefabIdentity.GetComponent<MeshRenderer>().material = profile.playerColor;
            prefabIdentity.transform.name = profile.name;

            Instantiate(prefabIdentity, spawnPosition, prefabIdentity.transform.rotation);
        }
    }*/
