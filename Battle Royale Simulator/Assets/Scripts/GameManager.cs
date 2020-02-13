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
    [SerializeField] Button[] scoreBoardsButtons;

    [SerializeField] Text test;
    
    [Header("Variables")]
    [SerializeField] Vector3 worldSize;
    bool firstBlood = false;
    bool matchHasStarted = false;

    public List<string> stringList; /*= new List<string>();*/
    public List<PlayerStatsTXT> playersList = new List<PlayerStatsTXT>();

    List<string> someList = new List<string>();

    /*[SerializeField] AssetReference txtFile;
    [SerializeField] TextAsset txt;*/

    void Start()
    {
        someList.Add("ciao");
        someList.Add("aaaaa");
        Load();

        //Clear the list when the scene is loaded
        /*foreach(string name in stringList)
        {
            stringList.Remove(name);
        }*/

        winnerText.text = "";
        /*TextAsset txt = (TextAsset)Resources.Load("names", typeof(TextAsset));
        stringList = new List<string>(txt.text.Split('\n')); //.text.split*/
    }

    void Load()
    {
        if(File.Exists(Application.dataPath + "/names.txt"))
        {
            string saveString = File.ReadAllText(Application.dataPath + "/names.txt");
            print("Loaded" + saveString);

            SaveObject saveObject = JsonUtility.FromJson<SaveObject>(saveString);

            saveObject.txtFileContent.Add("ciao");
            saveObject.txtFileContent.Add("AAAAA");

            stringList = saveObject.txtFileContent;
        }
        else print("no savr");
    }

    void Update()
    {
        //if(Input.GetKeyDown(KeyCode.S)) Save();

        if(Input.GetKeyDown(KeyCode.L)) Load();

        if (matchHasStarted)
        {
            CheckForPlayers();
            playersList.Sort((x, y) => y.ReturnKills().CompareTo(x.ReturnKills()));
            UpdateScoreboard();
            if (firstBlood == false) CheckFirstBlood();
        }
    }

    /*void LoadDone(UnityEngine.AsyncOperation.IAsyncoperations<TextAsset> obj)
    {
        txt = obj.result;
        print("finish load asset");
    }*/

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

    void Save()
    {
        print(Application.dataPath);
        SaveObject saveObject = new SaveObject
        {
            txtFileContent = someList
        };
        string json = JsonUtility.ToJson(saveObject);
        string path = Application.dataPath + "/names.txt";
        File.WriteAllText(path, json);
        test.text = path;
    }



    private class SaveObject
    {
        public List<string> txtFileContent;       
    }
}
