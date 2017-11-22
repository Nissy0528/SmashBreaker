using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
public class SceneWarpZone : MonoBehaviour
{

	/// <summary>
	/// 移動先のscene
	/// </summary>
	[SerializeField]
	private string nextScene = "Main";

	private void Start()
	{
		GetComponent<Collider2D>().isTrigger = true;

	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.GetComponent<Player>() != null)
		{
			Warp();

		}
	}
	/// <summary>
	/// 移動処理
	/// </summary>
	private void Warp()
	{

		SceneManager.LoadScene(nextScene);
	}
}
