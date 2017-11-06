using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossBulletShooter : MonoBehaviour
{

	/// <summary>
	/// 弾丸用のPrefab
	/// </summary>
	[SerializeField]
	private GameObject[] bullet;


	// Use this for initialization
	void Start()
	{
	}

	 void Update()
	{
		//テスト用
		Test();	
	}


	private float t_rate = 2f;
	private float t_count = 0f;
	/// <summary>
	/// テスト用
	/// </summary>
	private void Test()
	{
		t_count += Time.deltaTime;

		if(t_rate < t_count)
		{
			t_count = 0;
			Shot(90f);
		}

	}


	/// <summary>
	/// 発射処理
	/// </summary>
	/// <param name="rotation">発射角(実数)</param>
 
	private void Shot(float rotation)
	{
		Quaternion rot = Quaternion.Euler(transform.rotation.eulerAngles + Vector3.forward * rotation);
		Vector3 popPoint = transform.position + rot * (Vector3.up);
		var bInstant = Instantiate<GameObject>(bullet[0], popPoint, Quaternion.identity);
		bInstant.transform.rotation = rot;
	}

	/// <summary>
	/// 発射処理
	/// </summary>
	/// <param name="index">発射する弾丸</param>
	///<param name="rotation">発射角(実数)</param> 
	private void Shot( int index , float rotation)
	{
		Quaternion rot = Quaternion.Euler(transform.rotation.eulerAngles + Vector3.forward * rotation);
		Vector3 popPoint = transform.position + rot * ( Vector3.up);
		var bInstant = Instantiate<GameObject>(bullet[index], popPoint, Quaternion.identity);
		bInstant.transform.rotation = rot;
	}
}
