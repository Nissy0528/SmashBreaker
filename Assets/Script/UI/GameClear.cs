using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameClear : MonoBehaviour
{
    public GameObject time;

    private float gameTime;

    // Use this for initialization
    void Start()
    {
        gameTime = Mathf.Round(GameManager.time * 100) / 100;
        time.GetComponent<Text>().text = "Time : " + gameTime;
    }

    // Update is called once per frame
    void Update()
    {

    }
}
