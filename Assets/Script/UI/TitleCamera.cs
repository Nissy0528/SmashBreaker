using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleCamera : MonoBehaviour {

	/// <summary>
	///振動時間（設定用） 
	/// </summary>
	[SerializeField]
	private float shakeTime;
	/// <summary>
	///振動の幅
	/// </summary>
	[SerializeField]
	private float shakeRange;

	private GameObject parent;//親オブジェクト
	private Camera cam;//カメラ
	private Vector3 savePosition;//振動する前の座標
	private Vector3 screenMinPos;//画面の左下
	private Vector3 screenMaxPos;//画面の右上
	private Vector3 offset;
	//振動する幅
	private Vector2 lowRange;
	private Vector2 maxRange;
	private float lifeTime;//振動時間
	private bool isShake;//trueなら振動開始

	// Use this for initialization
	void Start()
	{
		parent = transform.parent.gameObject;
		cam = GetComponent<Camera>();
		screenMinPos = cam.ScreenToWorldPoint(Vector3.zero);//画面の左下の座標
		screenMaxPos = cam.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, 1));//画面の右下の座標
		savePosition = parent.transform.position;//振動前の座標取得
		lifeTime = 0.0f;
	}

	// Update is called once per frame
	void Update()
	{
		screenMinPos = cam.ScreenToWorldPoint(Vector3.zero);//画面の左下の座標
		screenMaxPos = cam.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, 1));//画面の右下の座標

		Shake();//振動
	}

	/// <summary>
	/// 振動開始
	/// </summary>
	public void SetShake(float setTime = 0.0f)
	{
		if (isShake) return;

		//振動の範囲設定
		lowRange = new Vector2(savePosition.x - shakeRange, savePosition.y - shakeRange);
		maxRange = new Vector2(savePosition.x + shakeRange, savePosition.y + shakeRange);
		if (setTime == 0.0f)
		{
			lifeTime = shakeTime;
		}
		else
		{
			lifeTime = setTime;
		}

		isShake = true;
	}

	/// <summary>
	/// 振動
	/// </summary>
	private void Shake()
	{
		if (Time.timeScale == 0.0f) return;

		//振動時間が0になったら振動終了
		if (lifeTime < 0.0f)
		{
			Stop();
		}

		//振動時間が設定されたら振動
		if (lifeTime > 0.0f)
		{
			lifeTime -= Time.deltaTime;
			float x_val = Random.Range(lowRange.x, maxRange.x);
			float y_val = Random.Range(lowRange.y, maxRange.y);
			parent.transform.position = new Vector3(x_val, y_val, parent.transform.position.z);
		}
	}

	/// <summary>
	/// 画面の左下
	/// </summary>
	/// <returns></returns>
	public Vector3 ScreenMin
	{
		get { return screenMinPos; }
	}

	/// <summary>
	/// 画面の右下
	/// </summary>
	/// <returns></returns>
	public Vector3 ScreenMax
	{
		get { return screenMaxPos; }
	}

	/// <summary>
	/// 振動終了
	/// </summary>
	public bool IsShakeFinish
	{
		get { return lifeTime == 0.0f; }
	}

	/// <summary>
	/// 振動停止
	/// </summary>
	public void Stop()
	{
		parent.transform.position = savePosition;
		isShake = false;
		lifeTime = 0.0f;
	}
}
