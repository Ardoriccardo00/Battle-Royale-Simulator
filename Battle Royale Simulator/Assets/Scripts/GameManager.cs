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

    [Header("UI Components")]
    [SerializeField] TextMeshProUGUI winnerText;
    [SerializeField] TextMeshProUGUI playersAliveText;
    [SerializeField] ScoreBoardButton[] scoreBoardsButtons;
    
    [Header("Variables")]
    [SerializeField] Vector3 worldSize;
    bool firstBlood = false;
    bool matchHasStarted = false;

    public List<string> stringList;
    public List<Color> colorList;
    public List<PlayerStatsTXT> playersList = new List<PlayerStatsTXT>();

    void Start()
    {
        Load();
        winnerText.text = "";
    }

#region LoadFile
    private class SaveObject
    {
        public List<string> namesFromTxt;
        public List<Color> colorsFromTxt;
    }

    void Save()
    {
        print(Application.dataPath);

        SaveObject saveObject = new SaveObject
        {
            //colorsFromTxt = someList
        };

        string json = JsonUtility.ToJson(saveObject);
        string path = Application.dataPath + "/names.txt";
        File.WriteAllText(path, json);
        print(saveObject.colorsFromTxt);
    }

    void Load()
    {
        if(File.Exists(Application.dataPath + "/names.txt"))
        {
            string saveString = File.ReadAllText(Application.dataPath + "/names.txt");
            print("Loaded" + saveString);

            SaveObject saveObject = JsonUtility.FromJson<SaveObject>(saveString);

            stringList = saveObject.namesFromTxt;
            colorList = saveObject.colorsFromTxt;
            foreach(Color color in colorList)
            {
                print(color);
            }
        }
        else print("no savr");
    }
#endregion

    void Update()
    {
        if (matchHasStarted)
        {
            CheckForPlayers();
            UpdateScoreboard();
            if (firstBlood == false) CheckFirstBlood();
        }
    }

#region Spawning
    public void SpawnWithTxt()
    {
        matchHasStarted = true;

        for (int i = 0; i < stringList.Count; i++)
        {
            float x, y, z;
            x = UnityEngine.Random.Range(-worldSize.x / 2, worldSize.x / 2);
            y = UnityEngine.Random.Range(-worldSize.y / 2, worldSize.y / 2);
            z = UnityEngine.Random.Range(-worldSize.z / 2, worldSize.z / 2);

            Vector3 spawnPosition = new Vector3(x, .5f, z);

            PlayerStatsTXT playerToInstantiate = Instantiate(playerPrefab, spawnPosition, playerPrefab.transform.rotation).GetComponent<PlayerStatsTXT>();
            var playerMaterial = playerToInstantiate.GetComponent<MeshRenderer>();
            playerToInstantiate.SetPlayerName(stringList[i]);
            playerMaterial.material.color = colorList[UnityEngine.Random.Range(0,colorList.Count)];
            playersList.Add(playerToInstantiate);
        }
    }

    public void SpawnChests()
    {
        int maxNumber = UnityEngine.Random.Range(stringList.Count/4 + 1, stringList.Count - stringList.Count/2);
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
        playersList.Sort((x, y) => y.ReturnKills().CompareTo(x.ReturnKills()));

        for (int i = 0; i < scoreBoardsButtons.Length; i++)
        {
            scoreBoardsButtons[i].SetPlayerToTarget(playersList[i]);
        }

        playersAliveText.text = "Remaining players: " + Convert.ToString(playersList.Count);
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
