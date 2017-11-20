using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
public class SceneWarpZone : MonoBehaviour {

	/// <summary>
	/// 移動先のscene
	/// </summary>
	private string nextScene;

	private void Start()
	{
		GetComponent<Collider2D>().isTrigger = true;
		//一時的に消しておく
		gameObject.SetActive(false);
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		Warp();
	}
	/// <summary>
	/// 移動処理
	/// </summary>
	private void Warp()
	{
		SceneManager.LoadScene(nextScene);
	}
}
