using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenFile : MonoBehaviour
{
    TextAsset questData;

    void Start()
    {
        questData = Resources.Load<TextAsset>("questdata");

        string[] data = questData.text.Split(new char[] { '\n' });
    }

    void Update()
    {
        
    }
}
