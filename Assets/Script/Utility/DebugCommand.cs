using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Diagnostics;
public class DebugCommand : MonoBehaviour
{

    private void Update()
    {
        GameClear();
    }

    [Conditional("UNITY_EDITOR")]
    public static void GameClear()
    {
        if (Input.GetKeyDown(KeyCode.U))
        {
            Boss boss = GameObject.FindGameObjectWithTag("Boss").GetComponent<Boss>();
            boss.HP = 0;
        }

        if (Input.GetKeyDown(KeyCode.M))
        {
            FindObjectOfType<Player>().AddSP(10, false);
        }

        if(Input.GetKeyDown(KeyCode.K))
        {
            FindObjectOfType<Player>().Damage();
        }

    }

	[Conditional("UNITY_EDITOR")]
	public static void DebugLog(string text)
	{
		UnityEngine.Debug.Log(text);
	}
}
