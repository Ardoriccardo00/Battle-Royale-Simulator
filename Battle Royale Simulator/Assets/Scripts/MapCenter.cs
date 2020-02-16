using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapCenter : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        print(other.transform.name + " has reached the center");
    }
}
