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
			Destroy(FindObjectOfType<Boss>().gameObject);
		}

        if(Input.GetKeyDown(KeyCode.S))
        {
            FindObjectOfType<Player>().AddSP(10);
        }

	}
}
