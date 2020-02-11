using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;
using System.IO;

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
    [SerializeField] TextMeshProUGUI winnerText;
    [SerializeField] TextMeshProUGUI playersAliveText;
    [SerializeField] Button[] scoreBoardsButtons;
    [Header("Variables")]
    [SerializeField] Vector3 worldSize;
    bool firstBlood = false;
    bool matchHasStarted = false;
    string readPath;

    

    public List<string> stringList = new List<string>();
    List<PlayerStatsTXT> playersList = new List<PlayerStatsTXT>();

    void Start()
    {
        
        foreach(string name in stringList)
        {
            stringList.Remove(name);
        }

        winnerText.text = "";

        TextAsset file = (TextAsset)Resources.Load("names.txt");
        file = new StringReader(file.text);
        //readPath = Application.dataPath + "./names.txt";
        //Texture text = (Texture)Resources.Load("/Images/SomeImage.png");
        //ReadFile(file);
    }

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

    private void ReadFile(string readPath)
    {
        StreamReader sReader = new StreamReader(readPath);

        while (!sReader.EndOfStream)
        {
            string line = sReader.ReadLine();
            stringList.Add(line);
        }
        sReader.Close();
    }

    public void SpawnWithTxt()
    {
        matchHasStarted = true;

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

    public void RemovePlayer(PlayerStatsTXT player)
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
        Gizmos.DrawCube(new Vector3(0, 0, 0), worldSize);
    }
}
