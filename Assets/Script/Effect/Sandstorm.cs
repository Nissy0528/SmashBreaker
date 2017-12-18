using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sandstorm : MonoBehaviour
{
    public float scrollSpeed;
    public float activeTime;
    public float negativeTime;
    public GameObject mainCamera;

    private SpriteRenderer sprite;
    private float time;

    // Use this for initialization
    void Start()
    {
        sprite = GetComponent<SpriteRenderer>();
        sprite.enabled = false;
        time = activeTime;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 cameraPos = mainCamera.transform.position;
        transform.position = new Vector3(cameraPos.x, cameraPos.y, 0.0f);
        ShowChange();
        Scroll();
    }

    /// <summary>
    /// スクロール
    /// </summary>
    private void Scroll()
    {
        if (!sprite.enabled)
        {
            sprite.material.mainTextureOffset = Vector2.zero;
            return;
        }

        float offset = scrollSpeed * Time.time;
        sprite.material.mainTextureOffset = new Vector2(offset, -offset);
    }

    /// <summary>
    /// 表示切替
    /// </summary>
    private void ShowChange()
    {
        if (time > 0.0f)
        {
            time -= Time.deltaTime;
            return;
        }

        sprite.enabled = !sprite.enabled;

        if (sprite.enabled)
        {
            time = negativeTime;
        }
        else
        {
            time = activeTime;
        }
    }
}
