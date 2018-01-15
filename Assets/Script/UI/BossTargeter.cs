using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// bossへの矢印
/// </summary>
public class BossTargeter : MonoBehaviour
{
	/// <summary>
	/// 目標
	/// </summary>
	[SerializeField]
	private GameObject target;

	/// <summary>
	/// プレイヤー
	/// </summary>
	private GameObject player;

	/// <summary>
	/// 原点
	/// </summary>
	private Transform origin;

	/// <summary>
	/// 矢印
	/// </summary>
	[SerializeField]
	private GameObject cursor;


	// Use this for initialization
	void Start()
	{
		if (target == null)
		{
			target = FindObjectOfType<BarrierBoss>().gameObject;
		}

		player = FindObjectOfType<Player>().gameObject;

		var c = Instantiate<GameObject>(cursor);

		c.transform.SetParent(transform);
		c.transform.localPosition = Vector2.up;

	}

	// Update is called once per frame
	void Update()
	{
		cursor.SetActive(true);
		if (target == null || player.GetComponent<Player>().IsDead())
		{
			cursor.SetActive(false);
			return;
		}
		transform.position = player.transform.position;

		Vector3 tp = target.transform.position;
		Vector3 direction = (tp - transform.position).normalized;
		transform.rotation = Quaternion.FromToRotation(Vector3.up, direction);
	}
}
