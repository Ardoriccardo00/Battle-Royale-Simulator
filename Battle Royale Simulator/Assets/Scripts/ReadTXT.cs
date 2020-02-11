using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

[ExecuteInEditMode]
public class ReadTXT : MonoBehaviour
{
    string readPath;

    public List<string> stringList = new List<string>();

    void Awake()
    {
        readPath = Application.dataPath + "./names.txt";

        ReadFile(readPath);
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

    void Update()
    {
        
    }
}
