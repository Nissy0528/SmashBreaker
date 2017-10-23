using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapCreater : MonoBehaviour
{
    private string path = "test";
    MapReader reader = new MapReader();

    // Use this for initialization
    void Start()
    {
        reader.Load(path);
        Debug.Log(reader.GetString(0, 0));
    }

    // Update is called once per frame
    void Update()
    {

    }
}
