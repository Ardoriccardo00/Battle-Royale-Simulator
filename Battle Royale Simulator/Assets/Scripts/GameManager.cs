using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.IO;
using System;

public class GameManager : MonoBehaviour
{
#region Singleton
    public static GameManager Instance { get; private set; }
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

    [Header("Components")]
    [SerializeField] GameObject playerPrefab;
    [SerializeField] GameObject chestPrefab;
    [SerializeField] TextMeshProUGUI winnerText;
    [SerializeField] TextMeshProUGUI playersAliveText;
    [SerializeField] ScoreBoardButton[] scoreBoardsButtons;
    
    [Header("Variables")]
    [SerializeField] Vector3 worldSize;
    bool firstBlood = false;
    bool matchHasStarted = false;

    public List<string> stringList;
    public List<PlayerStatsTXT> playersList = new List<PlayerStatsTXT>();

    void Start()
    {
        Load();

        winnerText.text = "";

        /*for(int i = 0; i < scoreBoardsButtons.Length; i++)
        {
            scoreBoardsButtons[i].GetComponent<ScoreBoardButton>();
        }*/
    }

#region LoadFile
    private class SaveObject
    {
        public List<string> txtFileContent;
    }

    void Load()
    {
        if(File.Exists(Application.dataPath + "/names.txt"))
        {
            string saveString = File.ReadAllText(Application.dataPath + "/names.txt");
            print("Loaded" + saveString);

            SaveObject saveObject = JsonUtility.FromJson<SaveObject>(saveString);

            stringList = saveObject.txtFileContent;
        }
        else print("no savr");
    }
#endregion
    void Update()
    {
        if (matchHasStarted)
        {
            CheckForPlayers();
            playersList.Sort((x, y) => y.ReturnKills().CompareTo(x.ReturnKills()));
            UpdateScoreboard();
            if (firstBlood == false) CheckFirstBlood();
        }
    }

#region Spawning
    public void SpawnWithTxt()
    {
        matchHasStarted = true;

        SpawnChests();

        for (int i = 0; i < stringList.Count; i++)
        {
            float x, y, z;
            x = UnityEngine.Random.Range(-worldSize.x / 2, worldSize.x / 2);
            y = UnityEngine.Random.Range(-worldSize.y / 2, worldSize.y / 2);
            z = UnityEngine.Random.Range(-worldSize.z / 2, worldSize.z / 2);

            Vector3 spawnPosition = new Vector3(x, 1, z);

            PlayerStatsTXT playerToInstantiate = Instantiate(playerPrefab, spawnPosition, playerPrefab.transform.rotation).GetComponent<PlayerStatsTXT>();
            playerToInstantiate.SetPlayerName(stringList[i]);
            playersList.Add(playerToInstantiate);
        }
    }

    void SpawnChests()
    {
        int maxNumber = UnityEngine.Random.Range(3, stringList.Count / 3);
        maxNumber = Mathf.RoundToInt(maxNumber);

        for (int i = 0; i < maxNumber; i++)
        {
            float x, y, z;
            x = UnityEngine.Random.Range(-worldSize.x / 2, worldSize.x / 2);
            y = UnityEngine.Random.Range(-worldSize.y / 2, worldSize.y / 2);
            z = UnityEngine.Random.Range(-worldSize.z / 2, worldSize.z / 2);

            Vector3 spawnPosition = new Vector3(x, .5f, z);

            Instantiate(chestPrefab, spawnPosition, chestPrefab.transform.rotation);
        }
    }
    #endregion

    void CheckForPlayers()
    {
        if (playersList.Count == 1)
        {
            winnerText.text = (playersList[0].ReturnPlayerName() + " Has won!");
        }
    }

    void CheckFirstBlood()
    {
        if (playersList.Count < stringList.Count)
        {
            print("first blood");
            firstBlood = true;
        }
    }

    void UpdateScoreboard()
    {
        for (int i = 0; i < scoreBoardsButtons.Length; i++)
        {
            scoreBoardsButtons[i].SetPlayerToTarget(playersList[i]);
        }
    }

#region Public Functions
    public void RemovePlayer(PlayerStatsTXT player)
    {
        playersList.Remove(player);
    }

    public void SetMatchHasStarted(bool value)
    {
        matchHasStarted = value;
    }

    public bool ReturnMatchHasStarted()
    {
        return matchHasStarted;
    }

    public int ReturnNumberPlayersAlive()
    {
        return playersList.Count;
    }

    #endregion

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawCube(new Vector3(0, 0, 0), worldSize);
    }

    
}
