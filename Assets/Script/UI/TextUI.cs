using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextUI : MonoBehaviour
{
    private RectTransform rectTransfrom;
    private Vector3 position;
    private Text text;

    // Use this for initialization
    void Start()
    {
        rectTransfrom = GetComponent<RectTransform>();
        GameObject canvas = GameObject.Find("Canvas");
        rectTransfrom.SetParent(canvas.transform, false);
        rectTransfrom.position = RectTransformUtility.WorldToScreenPoint(Camera.main, position);
        text = GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        Color color = text.color;
        color.a -= Time.deltaTime;
        text.color = color;

        if(text.color.a<=0.0f)
        {
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// 生成座標設定
    /// </summary>
    public void SetPos(Vector3 pos)
    {
        position = pos;
    }
}
