using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Title : MonoBehaviour
{

	[SerializeField]
	private string nextScene;

	[SerializeField]
	private Button startButton;

	// Use this for initialization
	void Start()
	{
		if (startButton == null)
		{
			startButton = GameObject.Find("start").GetComponent<Button>();
		}
		startButton.onClick.AddListener(() => StartPush());
        GameManager.stageNum = 0;
        GameManager.time = 0.0f;
	}

	// Update is called once per frame
	void Update()
	{

	}

	/// <summary>
	/// ゲーム開始処理
	/// </summary>
	private void StartPush()
	{
		SceneManager.LoadSceneAsync(nextScene);
	}
}
