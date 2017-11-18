using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EyeMove : MonoBehaviour
{
    public float speed;//速度

    private GameObject player;
    private GameObject child;
    private Vector3 lookPos;

    // Use this for initialization
    void Start()
    {
        player = GameObject.Find("Chara");
        child = transform.Find("Eye2").gameObject;
        lookPos = player.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        LookPlayer();//プレイヤーを見る
    }

    /// <summary>
    /// プレイヤーを見る
    /// </summary>
    private void LookPlayer()
    {
        lookPos = player.transform.position;
        child.transform.position = Vector2.MoveTowards(child.transform.position, lookPos, speed * Time.deltaTime);

        Vector2 pos = child.transform.localPosition;
        Vector2 parentPos = transform.position;
        pos.x = Mathf.Clamp(pos.x, -0.065f, 0.065f);
        pos.y = Mathf.Clamp(pos.y,-0.065f, 0.065f);
        child.transform.localPosition = pos;
    }
}
